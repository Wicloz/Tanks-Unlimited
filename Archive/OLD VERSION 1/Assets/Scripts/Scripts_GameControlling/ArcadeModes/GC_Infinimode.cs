using UnityEngine;
using System.Collections;

public class GC_Infinimode : GameController
{
	public GameObject gameCompleteItems;

	new void Update ()
	{
		base.Update ();

		if (GameStates.acces.endInitialised && !SwitchBox.isClient)
		{
			GameDoneCheck ();
		}
	}

	// Initialisation
	public override void ChildInitialise ()
	{
		if (!SwitchBox.isClient)
		{
			RandomInfiniLevel ();
		}
	}

	void RandomInfiniLevel ()
	{
		enemyTanks = GlobalValues.acces.enemyTanks;
		tanksLeft = GlobalValues.acces.enemyTanks;
		
		for (int t = 0; t < GlobalValues.acces.enemyTanks; t++)
		{
			float X = Random.Range(-12, 17);
			float Z = Random.Range(-13, 13);
			SwitchBox.Instantiate (enemyTank, new Vector3(X, 0.3f, Z), Quaternion.Euler(enemyTank.transform.rotation.x, Random.Range(0,360), enemyTank.transform.rotation.z));
		}
		
		for (float i = -17.5f; i <= 17.5f; i++)
		{
			for (float j = -13.5f; j <= 13.5f; j++)
			{
				float random = Random.value;
				float section = GlobalValues.acces.chance / (largeObjects.Length + smallObjects.Length + destObjects.Length);
				
				if (random > GlobalValues.acces.chance)
				{
				}
				else if (random <= (section * 1))
				{
					RaycastHit hit;
					if (Physics.Raycast (new Vector3(i, 4, j), Vector3.down, out hit))
					{
						if (hit.transform.gameObject.name == "Ground")
						{
							SwitchBox.Instantiate (largeObjects[0], new Vector3(i, 0.5f, j), largeObjects[0].transform.rotation);
						}
					}
				}
				else if (random <= (section * 2))
				{
					RaycastHit hit;
					if (Physics.Raycast (new Vector3(i, 4, j), Vector3.down, out hit))
					{
						if (hit.transform.gameObject.name == "Ground")
						{
							SwitchBox.Instantiate (largeObjects[1], new Vector3(i, 1.0f, j), largeObjects[1].transform.rotation);
						}
					}
				}
				else if (random <= (section * 3))
				{
					RaycastHit hit;
					if (Physics.Raycast (new Vector3(i, 4, j), Vector3.down, out hit))
					{
						if (hit.transform.gameObject.name == "Ground")
						{
							SwitchBox.Instantiate (destObjects[0], new Vector3(i, 0.75f, j), destObjects[0].transform.rotation);
						}
					}
				}
			}
		}
	}

	// Game Controlling
	void GameDoneCheck ()
	{
		if (PlayerManager.acces.AreAllDead() && GlobalValues.acces.lives == 0 && !GameStates.acces.gameDone && PlayerManager.acces.hasSpawned)
		{
			StartCoroutine (GameOverSet());
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

		GameOverText.acces.done = true;
	}
	
	public override void ChildStageCleared ()
	{
		sceneOnStageComplete = "Play_InfiniMode";

		gameCompleteItems.SetActive (true);
		
		AddScore (pointsPerLevel);
		RunRecorder.acces.level++;
		RunRecorder.acces.runTime += Time.time;
	}

	// Functions
	public override void OnNpcTankDestroy ()
	{
		base.OnNpcTankDestroy ();
		if (tanksLeft == 0)
		{
			GameStates.acces.stageCleared = true;
		}
	}

	// Score GUI
	public override void ChildUpdateScore ()
	{
		ScoreText.acces.EditLine (0, (enemyTanks - tanksLeft) + " of " + enemyTanks + " tanks destroyed");
		ScoreText.acces.EditLine (1, "Points: " + (int) showScore);
		ScoreText.acces.EditLine (2, "Stage " + RunRecorder.acces.level);
	}
}
