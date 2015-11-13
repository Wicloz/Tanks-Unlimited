using UnityEngine;
using System.Collections;

public class GC_Infinimode : GameController
{
	new void Update ()
	{
		base.Update ();
	}

	// Initialisation
	public override void ChildInitialise ()
	{
		respawnCost = 1;
		pointsPerLevel = MultiplayerManager.acces.gameSettings.enemyTanks * 100 * ((float) MultiplayerManager.acces.gameSettings.difficulty / 2);

		if (SwitchBox.isHost)
		{
			RunRecorder.acces.level += 1;
		}
	}

	public override void CreateLevel ()
	{
		enemyTanks = MultiplayerManager.acces.gameSettings.enemyTanks;
		tanksLeft = MultiplayerManager.acces.gameSettings.enemyTanks;
		
		for (int t = 0; t < enemyTanks; t++)
		{
			float X = Random.Range(-11, 18);
			float Z = Random.Range(-13, 14);
			SwitchBox.Instantiate (PrefabHolder.acces.enemyTank.gameObject, new Vector3(X, PrefabHolder.acces.enemyTank.spawnHeight, Z), Quaternion.Euler(PrefabHolder.acces.enemyTank.gameObject.transform.rotation.x, Random.Range(0f,360f), PrefabHolder.acces.enemyTank.gameObject.transform.rotation.z));
		}

		LevelController.acces.GenerateRandomLevel (new WorldRect (-17.5f, 17.5f, -13.5f, 13.5f), new WorldRect (-17.5f, -13.5f, -3.5f, 3.5f), (float)MultiplayerManager.acces.gameSettings.objectDensity / 1000, 0.3f, true);

		base.CreateLevel ();
	}

	// Game Controlling
	public override void GameDoneCheck ()
	{
		if (RespawnManager.acces.AllGameOver())
		{
			StateManager.acces.EndGame("Game Over");
		}

		if (tanksLeft == 0 && !stageClearedPre)
		{
			if (SwitchBox.isServer)
			{
				StageClearedSend (true);
			}
			else if (!SwitchBox.isClient)
			{
				StageClearedRecieve (true);
			}
		}
	}

	// Game Done
	public override void ChildGameOver ()
	{
		GameOverText.acces.RequestTextLine ("Stages Played: " + RunRecorder.acces.level, fontSize);
		GameOverText.acces.RequestTextLine ("Time Played: " + (int) RunRecorder.acces.runTime + "s", fontSize);

		GameOverText.acces.RequestSeperatorLine ();

		GameOverText.acces.RequestTextLine ("Enemy Tanks Destroyed:", fontSize);
		for (int i = 0; i < RunRecorder.acces.killLog.Count; i++)
		{
			GameOverText.acces.RequestTextLine (RunRecorder.acces.killLog.GetKey(i) + ": " + RunRecorder.acces.killLog.Acces(i), fontSize);
		}
		if (RunRecorder.acces.killLog.Count == 0)
		{
			GameOverText.acces.RequestTextLine ("No Tanks Destroyed", fontSize);
		}

		GameOverText.acces.RequestSeperatorLine ();

		GameOverText.acces.RequestTextLine ("Total Score: " + RunRecorder.acces.runScore, fontSize);

		GameOverText.acces.requestDone = true;
	}
	
	public override void ChildStageCleared ()
	{
		sceneOnStageComplete = "Play_InfiniMode";
		AddScore (pointsPerLevel, "", true);
	}

	// Functions
	public override void OnNpcTankDestroy ()
	{
		base.OnNpcTankDestroy ();
	}

	// Score GUI
	public override void ChildUpdateScore ()
	{
		ScoreText.acces.EditLineSelf (0, (enemyTanks - tanksLeft) + " of " + enemyTanks + " tanks destroyed");
		ScoreText.acces.EditLineSelf (1, "Points: " + (int) showScore);
		ScoreText.acces.EditLineSelf (2, "Stage " + RunRecorder.acces.level);

		foreach (UserInfo user in UserManager.acces.userList)
		{
			ScoreText.acces.EditLineOther (user.username, 0, "- " + user.username + " -");
			ScoreText.acces.EditLineOther (user.username, 1, "Kills: " + user.kills);
			ScoreText.acces.EditLineOther (user.username, 2, "Lifes: " + user.showLifes);
		}
	}
}
