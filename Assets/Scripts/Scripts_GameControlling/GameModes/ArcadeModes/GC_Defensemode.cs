using UnityEngine;
using System.Collections;

public class GC_Defensemode : GameController
{
	public GameObject flagPole;
	private GameObject[] npcSpawnPoints = new GameObject[4];

	private bool waveCleared = false;
	private bool pointCaptured = false;
	
	new void Update ()
	{
		base.Update ();
	}

	// Initialisation
	public override void ChildInitialise ()
	{
		respawnCost = 0;
		pointsPerLevel = 100 * MultiplayerManager.acces.gameSettings.difficulty;

		if (SwitchBox.isHost)
		{
			npcSpawnPoints = GameObject.FindGameObjectsWithTag (Tags.spawnPoint);
			StartCoroutine (WaveController ());
		}
	}

	public override void CreateLevel ()
	{
		SwitchBox.Instantiate (flagPole, flagPole.transform.position, flagPole.transform.rotation);
		LevelController.acces.GenerateTemplatedLevel (0.2f);

		base.CreateLevel ();
	}
	
	// Game Controlling
	private IEnumerator WaveController ()
	{
		RunRecorder.acces.level = 1;
		
		while (!gameOverPre)
		{
			enemyTanks = RunRecorder.acces.level;
			tanksLeft = RunRecorder.acces.level;
			
			for (int i = 0; i < RunRecorder.acces.level; i++)
			{
				int spawnPoint = Random.Range (0, 4);
				
				SwitchBox.Instantiate (PrefabHolder.acces.enemyTank.gameObject, npcSpawnPoints[spawnPoint].transform.position, npcSpawnPoints[spawnPoint].transform.rotation);
				
				yield return new WaitForSeconds (Random.Range (2f, 6f));
			}
			
			while (!waveCleared)
			{
				yield return new WaitForSeconds (0.5f);
			}
			
			waveCleared = false;
			
			while (gameOverPre)
			{
				yield return new WaitForSeconds (0.5f);
			}
			
			AddScore (pointsPerLevel, "", true);
			
			yield return new WaitForSeconds (6);
			RunRecorder.acces.level++;
		}
	}

	public override void GameDoneCheck ()
	{
		if (pointCaptured && !gameOverPre)
		{
			StateManager.acces.EndGame("Game Over");
		}
	}

	// Game Done
	public override void ChildGameOver ()
	{
		GameOverText.acces.RequestTextLine ("Waves Survived: " + RunRecorder.acces.level, fontSize);
		GameOverText.acces.RequestTextLine ("Time Survived: " + (int) RunRecorder.acces.runTime + "s", fontSize);

		GameOverText.acces.RequestSeperatorLine ();

		GameOverText.acces.RequestTextLine ("Total Score: " + RunRecorder.acces.runScore, fontSize);

		GameOverText.acces.requestDone = true;
	}

	// Functions
	public override void OnNpcTankDestroy ()
	{
		base.OnNpcTankDestroy ();
		if (tanksLeft == 0)
		{
			waveCleared = true;
		}
	}
	
	public void SetPointCapture ()
	{
		pointCaptured = true;
	}

	// Score GUI
	public override void ChildUpdateScore ()
	{
		ScoreText.acces.EditLineSelf (0, (enemyTanks - tanksLeft) + " of " + enemyTanks + " tanks destroyed");
		ScoreText.acces.EditLineSelf (1, "Points: " + (int) showScore);
		ScoreText.acces.EditLineSelf (2, "Wave " + RunRecorder.acces.level);

		foreach (UserInfo user in UserManager.acces.userList)
		{
			ScoreText.acces.EditLineOther (user.username, 0, "- " + user.username + " -");
			ScoreText.acces.EditLineOther (user.username, 1, "Kills: " + user.kills);
			ScoreText.acces.EditLineOther (user.username, 2, "Deaths: " + user.deaths);
		}
	}
}
