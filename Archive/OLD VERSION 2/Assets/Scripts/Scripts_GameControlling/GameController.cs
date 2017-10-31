using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController acces;

	protected GameObject gameoverExplosion;
	
	// GUI references
	protected GameObject hudItems;
	public GameObject gameOverItems;
	public GameObject stageCompleteItems;
	protected GameObject gameOverText;
	protected GameObject viewObstructor;
	protected GameObject scoreText;

	protected int fontSize;
	
	public void SetReferences (GameObject setHudItems, GameObject setGameOverItems, GameObject setStageCompleteItems, GameObject setGameOverText, GameObject setViewObstructor, GameObject setScoreText, GameObject setExplosion)
	{
		hudItems = setHudItems;
		gameOverItems = setGameOverItems;
		stageCompleteItems = setStageCompleteItems;
		gameOverText = setGameOverText;
		viewObstructor = setViewObstructor;
		scoreText = setScoreText;

		gameoverExplosion = setExplosion;
	}
	
	// Switching scenes
	public string sceneOnStageComplete = "none";
	public string newScene = "none";
	
	// Bools for one-shot functions
	protected bool goCall = true;
	protected bool scCall = true;
	protected bool ssCall = true;
	protected bool creatingLevel = false;
	protected bool changingScene = false;
	
	// Level data
	protected int EnemyTanks;
	protected int TanksLeft;
	public int respawnCost;

	public bool coop = true;
	public bool playerPoints = false;

	public int enemyTanks
	{
		get
		{
			return EnemyTanks;
		}
		set
		{
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetEnemyTanks", RPCMode.AllBuffered, value);
			}
			else
			{
				SetEnemyTanks (value);
			}
		}
	}
	[RPC] protected void SetEnemyTanks (int value)
	{
		EnemyTanks = value;
	}

	public int tanksLeft
	{
		get
		{
			return TanksLeft;
		}
		set
		{
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetTanksLeft", RPCMode.AllBuffered, value);
			}
			else
			{
				SetTanksLeft (value);
			}
		}
	}
	[RPC] protected void SetTanksLeft (int value)
	{
		TanksLeft = value;
	}
	
	// Values for score handeling
	protected float showScore;
	protected float pointsPerLevel = 0;

	// States
	public bool gameStarting = false;
	public bool startInitialise = false;
	public bool endInitialise = false;

	public bool gameOverPre = false;
	public bool gameOverPost = false;
	public bool stageClearedPre = false;
	public bool stageClearedPost = false;

	// LoadingState uploading
	private void SendSceneLoaded ()
	{
		if (SwitchBox.isClient)
		{
			GetComponent<NetworkView>().RPC ("RecieveSceneLoaded", RPCMode.Server, ProfileManager.acces.player.username);
		}
		else
		{
			RecieveSceneLoaded (ProfileManager.acces.player.username);
		}
	}
	[RPC] private void RecieveSceneLoaded (string username)
	{
		UserManager.acces.GetUser(username).states.sceneLoaded = true;
	}

	public void SendLevelLoaded ()
	{
		if (SwitchBox.isClient)
		{
			GetComponent<NetworkView>().RPC ("RecieveLevelLoaded", RPCMode.Server, ProfileManager.acces.player.username);
		}
		else
		{
			RecieveLevelLoaded (ProfileManager.acces.player.username);
		}
	}
	[RPC] private void RecieveLevelLoaded (string username)
	{
		UserManager.acces.GetUser(username).states.levelLoaded = true;
	}

	private void SendReadyState (bool state)
	{
		if (SwitchBox.isClient)
		{
			GetComponent<NetworkView>().RPC ("RecieveReadyState", RPCMode.Server, ProfileManager.acces.player.username, state);
		}
		else
		{
			RecieveReadyState (ProfileManager.acces.player.username, state);
		}
	}
	[RPC] private void RecieveReadyState (string username, bool state)
	{
		UserManager.acces.GetUser(username).states.ready = state;
	}
	
	// Unity Functions
	protected void Awake ()
	{
		acces = this;
	}

	protected void OnGUI ()
	{
		if (MultiplayerManager.acces.gameState == GameState.Lobby)
		{
			Rect buttonRect = new Rect ((Screen.width / 2) - 100, (Screen.height / 2) - 100, 200, 200);

			if (!UserManager.acces.thisUser.states.ready && GUI.Button (buttonRect, "Ready"))
			{
				SendReadyState (true);
			}

			if (UserManager.acces.thisUser.states.ready && GUI.Button (buttonRect, "UnReady"))
			{
				SendReadyState (false);
			}
		}
	}

	protected void Update ()
	{
		if (UserManager.acces.thisUser != null)
		{
			// Call SceneStart
			if (ssCall)
			{
				ssCall = false;
				SceneStart ();
			}

			// State reactions
			if (UserManager.acces.AllUsersSceneLoaded() && !creatingLevel && SwitchBox.isHost)
			{
				creatingLevel = true;
				CreateLevel ();
			}
			
			// Initialisation checks
			if (!startInitialise)
			{
				MultiplayerManager.acces.gameState = GameState.Lobby;
				
				foreach (UserInfo user in UserManager.acces.userList)
				{
					ScoreText.acces.EditLineOther (user.username, 0, user.username);
				}
				
				if (SwitchBox.isHost && (UserManager.acces.AllUsersReady() || RunRecorder.acces.level > 0) && UserManager.acces.AllUsersLevelLoaded())
				{
					if (SwitchBox.isServer)
					{
						GameStartingSend (true);
					}
					else if (!SwitchBox.isClient)
					{
						GameStartingRecieve (true);
					}
				}
				
				if (gameStarting)
				{
					startInitialise = true;
					MultiplayerManager.acces.gameState = GameState.Loading;
					newScene = "none";
					Initialise ();
				}
			}
			
			// Game running
			if (endInitialise)
			{
				if (SwitchBox.isHost)
				{
					GameDoneCheck ();
				}
				
				if (gameOverPost)
				{
					GameOver ();
				}
				
				if (stageClearedPost && !gameOverPre)
				{
					StageCleared ();
				}
				
				UpdateScore ();
				ChangeScene ();
			}
		}
	}

	// Initialisation
	protected void SceneStart ()
	{
		if (!SwitchBox.isServerOn)
		{
			SendReadyState (true);
		}
		
		SendSceneLoaded ();
	}

	protected void Initialise ()
	{
		showScore = RunRecorder.acces.runScore;
		ScoreText.acces.Clear ();

		if (RunRecorder.acces.level == 0)
		{
			foreach (UserInfo user in UserManager.acces.userList)
			{
				user.lifes = MultiplayerManager.acces.gameSettings.maxLifes;
				
				if (!SwitchBox.isServerOn)
				{
					user.lifes *= 2;
				}
			}
		}
		
		ChildInitialise ();
		
		endInitialise = true;
		MultiplayerManager.acces.gameState = GameState.Running;
		viewObstructor.SetActive (false);
	}
	
	public virtual void ChildInitialise ()
	{}

	public virtual void CreateLevel ()
	{
		LevelController.acces.SendLevelData ("", true);
	}
	
	// Game done checking
	public virtual void GameDoneCheck ()
	{}

	// Game done initialising
	public void GameOverPreSet ()
	{
		gameOverPre = true;
		MultiplayerManager.acces.gameState = GameState.Results;
		StartCoroutine (GameOverPostSet());
	}
	
	protected IEnumerator GameOverPostSet ()
	{
		yield return new WaitForSeconds (2);
		gameOverPost = true;
	}
	
	protected void StageClearedPreSet ()
	{
		stageClearedPre = true;
		MultiplayerManager.acces.gameState = GameState.Results;
		StartCoroutine (StageClearedPostSet());
	}
	
	protected IEnumerator StageClearedPostSet ()
	{
		yield return new WaitForSeconds (2);
		stageClearedPost = true;
	}
	
	// Game over and stage cleared
	protected void GameOver ()
	{
		if (goCall)
		{
			goCall = false;
			OptionsManager.acces.CloseWindow();
			EscapeMenu.acces.CloseWindow();

			hudItems.SetActive (false);
			scoreText.SetActive (false);
			gameOverItems.SetActive (true);
			MultiplayerManager.acces.gameState = GameState.Results;
			RunRecorder.acces.runTime += Time.time;

			Instantiate (gameoverExplosion, Vector3.zero, Quaternion.identity);

			fontSize = (int) (Screen.height / 40);
			ChildGameOver ();
		}
		
		Cursor.visible = true;
		
		if (Input.GetButtonUp ("Fire1") || Input.GetButtonUp ("Fire2"))
		{
			StateManager.acces.GoToMainMenu ();
		}
	}
	
	protected void StageCleared ()
	{
		if (scCall)
		{
			scCall = false;
			OptionsManager.acces.CloseWindow();
			EscapeMenu.acces.CloseWindow();

			hudItems.SetActive (false);
			scoreText.gameObject.SetActive (false);
			stageCompleteItems.SetActive (true);
			MultiplayerManager.acces.gameState = GameState.Results;
			RunRecorder.acces.runTime += Time.time;
			
			ChildStageCleared ();
		}
		
		Cursor.visible = true;
		
		if (Input.GetButtonUp ("Fire1") || Input.GetButtonUp ("Fire2"))
		{
			if (SwitchBox.isServer)
			{
				NewSceneSend (sceneOnStageComplete);
			}
			else if (!SwitchBox.isClient)
			{
				NewSceneRecieve (sceneOnStageComplete);
			}
		}
	}

	// Game Done virtual functions
	public virtual void ChildGameOver ()
	{}
	
	public virtual void ChildStageCleared ()
	{}

	// Changing scene
	protected void ChangeScene ()
	{
		if (newScene != "none" && !changingScene)
		{
			changingScene = true;
			LoadingScreen.acces.QueLevelChange (newScene);
		}
	}

	// Functions
	public virtual void OnNpcTankDestroy ()
	{
		tanksLeft--;
	}

	// Adding points
	public void AddScore (float score, string username, bool allUsers)
	{
		if (SwitchBox.isServer)
		{
			GetComponent<NetworkView>().RPC("AddScoreRPC", RPCMode.All, score, username, allUsers);
		}
		else if (!SwitchBox.isClient)
		{
			AddScoreRPC (score, username, allUsers);
		}
	}
	
	[RPC] protected void AddScoreRPC (float score, string username, bool allUsers)
	{
		if (username == ProfileManager.acces.player.username || allUsers)
		{
			RunRecorder.acces.runScore += score;
		}
	}

	// Score GUI
	protected void UpdateScore ()
	{
		if (showScore < RunRecorder.acces.runScore)
		{
			showScore += Time.deltaTime * RunRecorder.acces.level * 100;
		}
		else 
		{
			showScore = RunRecorder.acces.runScore;
		}

		ChildUpdateScore ();
	}

	public virtual void ChildUpdateScore ()
	{
	}

	// Syncing values
	protected void GameStartingSend (bool state)
	{
		GetComponent<NetworkView>().RPC ("GameStartingRecieve", RPCMode.AllBuffered, state);
	}
	[RPC] public void GameStartingRecieve (bool state)
	{
		gameStarting = state;
	}
	
	protected void StageClearedSend (bool state)
	{
		GetComponent<NetworkView>().RPC ("StageClearedRecieve", RPCMode.AllBuffered, state);
	}
	[RPC] public void StageClearedRecieve (bool state)
	{
		if (state == true)
		{
			StageClearedPreSet ();
		}
	}
	
	protected void NewSceneSend (string scene)
	{
		GetComponent<NetworkView>().RPC ("NewSceneRecieve", RPCMode.AllBuffered, scene);
	}
	[RPC] public void NewSceneRecieve (string scene)
	{
		newScene = scene;
	}
}
