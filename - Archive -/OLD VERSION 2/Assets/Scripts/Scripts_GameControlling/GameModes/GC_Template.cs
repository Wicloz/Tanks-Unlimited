using UnityEngine;
using System.Collections;

public class GC_Template : GameController
{
	new void Update ()
	{
		base.Update ();
	}
	
	// Initialisation
	public override void ChildInitialise ()
	{
		coop = true;
		playerPoints = false;
		respawnCost = 1;
		pointsPerLevel = 0 * ((float) MultiplayerManager.acces.gameSettings.difficulty / 2);

		if (SwitchBox.isHost)
		{
			RunRecorder.acces.level += 1;
		}
	}

	public override void CreateLevel ()
	{
		base.CreateLevel ();
	}
	
	// Game Controlling
	public override void GameDoneCheck ()
	{
		if (true)
		{
			StateManager.acces.EndGame("Game Over");
		}

		if (true)
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
		GameOverText.acces.requestDone = true;
	}
	
	public override void ChildStageCleared ()
	{
		sceneOnStageComplete = "Play_";
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
			if (user.username != ProfileManager.acces.player.username)
			{
				ScoreText.acces.EditLineOther (user.username, 0, user.username);
				ScoreText.acces.EditLineOther (user.username, 1, "Points: " + user.points);
				ScoreText.acces.EditLineOther (user.username, 2, "Lifes: " + user.showLifes);
			}
		}
	}
}
