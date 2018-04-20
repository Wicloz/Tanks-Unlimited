using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController acces;
	public static ScreenFader screenFader;

	protected GameObject EXPLOSION;
	
	// GUI references
	protected GameObject hudItems;
	protected GameObject gameOverItems;
	protected GameObject gameOverText;
	protected GameObject viewObstructor;
	protected GameObject scoreText;

	protected int fontSize;

	// GameObjects for generating the playfield
	protected GameObject[] largeObjects;
	protected GameObject[] smallObjects;
	protected GameObject[] destObjects;
	protected GameObject enemyTank;
	
	public void SetReferences (GameObject[] setLargeObjects, GameObject[] setSmallObjects, GameObject[] setDestObjects, GameObject setEnemyTank, GameObject setHudItems, GameObject setGameOverItems, GameObject setGameOverText, GameObject setViewObstructor, GameObject setScoreText, GameObject setExplosion)
	{
		largeObjects = setLargeObjects;
		smallObjects = setSmallObjects;
		destObjects = setDestObjects;
		enemyTank = setEnemyTank;

		hudItems = setHudItems;
		gameOverItems = setGameOverItems;
		gameOverText = setGameOverText;
		viewObstructor = setViewObstructor;
		scoreText = setScoreText;

		EXPLOSION = setExplosion;
	}
	
	// Switching scenes
	public string newScene = "none";
	
	// Bools for one-shot functions
	protected bool goCall = true;
	protected bool scCall = true;
	protected bool changingScene = false;
	
	// Level data
	public int enemyTanks;
	public int tanksLeft;
	public int difficulty;

	// Values for score handeling
	protected float showScore;
	protected float pointsPerLevel = 400;

	// Misc
	public string sceneOnStageComplete = "none";

	// Awake, start, update
	protected void Awake ()
	{
		acces = this;
		screenFader = GameObject.FindWithTag (Tags.fader).GetComponent <ScreenFader>();
	}
	
	protected void Start ()
	{
		if (!GlobalValues.acces.multiplayer)
		{
			viewObstructor.SetActive (false);
		}
	}
	
	protected void Update ()
	{
		if (!GameStates.acces.startInitialised)
		{
			if (!GlobalValues.acces.multiplayer || RunRecorder.acces.level >= 2)
			{
				GameStates.acces.gameStarting = true;
			}
			
			if (GameStates.acces.gameStarting)
			{
				GameStates.acces.startInitialised = true;
				newScene = "none";
				difficulty = GlobalValues.acces.difficulty;
				
				if (SwitchBox.isHost)
				{
					StartCoroutine (Initialise (0));
				}
				else
				{
					StartCoroutine (Initialise (0.1f));
				}
			}
		}
		
		if (GameStates.acces.endInitialised)
		{
			if (GameStates.acces.GameOver)
			{
				GameOver ();
			}
			
			if (GameStates.acces.stageCleared && !GameStates.acces.gameDone)
			{
				StageCleared ();
			}
			
			UpdateScore ();
			ChangeScene ();
		}
	}

	// Initialisation
	public IEnumerator Initialise (float wait)
	{
		yield return new WaitForSeconds (wait);
		PauseMenu.acces.TimeScale (0);
		
		showScore = RunRecorder.acces.runScore;
		
		PlayerManager.acces.SpawnReticule ();
		PlayerManager.acces.StartRespawn (0, true);
		
		ChildInitialise ();
		
		GameStates.acces.endInitialised = true;
		viewObstructor.SetActive (false);
		
		PauseMenu.acces.TimeScale (1);
	}
	
	public virtual void ChildInitialise ()
	{
	}

	// Game over initialising
	protected void OnDisconnectedFromServer ()
	{
		if (SwitchBox.isClient)
		{
			StartCoroutine (GameOverSet());
		}
	}
	
	public IEnumerator GameOverSet ()
	{
		if (!GameStates.acces.stageCleared)
		{
			GameStates.acces.gameDone = true;
			yield return new WaitForSeconds (4);
			GameStates.acces.GameOver = true;
			
			if (SwitchBox.isClient)
			{
				DisconnectServer ();
			}
			else if (SwitchBox.isServer)
			{
				Invoke ("DisconnectServer", 0.1f);
			}
		}
	}
	
	public void DisconnectServer ()
	{
		Network.Disconnect ();
		MasterServer.UnregisterHost ();
		Debug.LogError ("Server Stopped");
	}
	
	// Game over and stage cleared
	protected void GameOver ()
	{
		if (goCall)
		{
			goCall = false;
			PauseMenu.acces.paused = false;

			hudItems.SetActive (false);
			scoreText.SetActive (false);
			gameOverItems.SetActive (true);
			
			GlobalValues.acces.totalScore += RunRecorder.acces.runScore;
			RunRecorder.acces.runTime += Time.time;

			Instantiate (EXPLOSION, Vector3.zero, Quaternion.identity);
			GlobalValues.acces.SaveGame ();

			fontSize = (int) (Screen.height / 40);
			ChildGameOver ();
		}
		
		Screen.showCursor = true;
		
		if (CrossPlatformInput.GetButtonUp ("Fire1") || CrossPlatformInput.GetButtonUp ("Fire2"))
		{
			screenFader.ChangeScene ("MainMenu");
		}
	}
	
	protected void StageCleared ()
	{
		if (scCall)
		{
			scCall = false;
			PauseMenu.acces.paused = false;

			hudItems.SetActive (false);
			scoreText.gameObject.SetActive (false);
			
			ChildStageCleared ();
		}
		
		Screen.showCursor = true;
		
		if (CrossPlatformInput.GetButtonUp ("Fire1") || CrossPlatformInput.GetButtonUp ("Fire2"))
		{
			if (!SwitchBox.isClient)
			{
				newScene = sceneOnStageComplete;
			}
		}
	}

	// Game Done virtual functions
	public virtual void ChildGameOver ()
	{
	}
	
	public virtual void ChildStageCleared ()
	{
	}
	
	// Changing scene
	protected void ChangeScene ()
	{
		if (newScene != "none" && !changingScene)
		{
			changingScene = true;
			screenFader.ChangeScene (newScene);
		}
	}
	
	// Syncing values
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		int tempEnemyTanks = 0;
		int tempTanksLeft = 0;
		int tempDifficulty = 0;

		networkView.RPC("BitStreamStrings", RPCMode.OthersBuffered, newScene);

		if (SwitchBox.isServer && stream.isWriting)
		{
			tempEnemyTanks = enemyTanks;
			stream.Serialize(ref tempEnemyTanks);

			tempTanksLeft = tanksLeft;
			stream.Serialize(ref tempTanksLeft);

			tempDifficulty = difficulty;
			stream.Serialize(ref tempDifficulty);
		}
		else if (SwitchBox.isClient)
		{
			stream.Serialize(ref tempEnemyTanks);
			enemyTanks = tempEnemyTanks;

			stream.Serialize(ref tempTanksLeft);
			tanksLeft = tempTanksLeft;

			stream.Serialize(ref tempDifficulty);
			difficulty = tempDifficulty;
		}
	}

	[RPC] public void BitStreamStrings (string tempNewScene)
	{
		newScene = tempNewScene;
	}

	// Functions
	public virtual void OnNpcTankDestroy ()
	{
		tanksLeft--;
	}

	// Adding points
	public void AddScore (float score)
	{
		if (SwitchBox.isServer)
		{
			networkView.RPC("AddScoreRPC", RPCMode.AllBuffered, score);
		}
		else if (!SwitchBox.isClient)
		{
			AddScoreRPC (score);
		}
	}
	
	[RPC] protected void AddScoreRPC (float score)
	{
		if (!GameStates.acces.gameDone)
		{
			RunRecorder.acces.runScore += score * RunRecorder.acces.level * ((float) difficulty / 2);
		}
	}

	// Score GUI
	public void UpdateScore ()
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
}
