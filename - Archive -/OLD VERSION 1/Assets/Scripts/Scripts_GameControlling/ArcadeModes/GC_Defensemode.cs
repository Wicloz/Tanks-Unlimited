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
			npcSpawnPoints = GameObject.FindGameObjectsWithTag (Tags.spawnPoint);
			
			RandomDefenseLevel ();
			StartCoroutine (WaveController ());
		}
		else
		{
			GameObject[] defenseWalls = GameObject.FindGameObjectsWithTag (Tags.startCubes);
			for (int i = 0; i < defenseWalls.Length; i++)
			{
				Destroy (defenseWalls[i].gameObject);
			}
		}
	}

	void RandomDefenseLevel ()
	{
		GameObject[] defenseWalls = GameObject.FindGameObjectsWithTag (Tags.startCubes);
		
		SwitchBox.Instantiate (flagPole, flagPole.transform.position, flagPole.transform.rotation);
		
		for (int i = 0; i < defenseWalls.Length; i++)
		{
			float random = Random.Range (0, 12);
			Destroy (defenseWalls[i].gameObject);
			
			if (random <= 2)
			{
				SwitchBox.Instantiate (destObjects[0], new Vector3(defenseWalls[i].transform.position.x, 0.75f, defenseWalls[i].transform.position.z), defenseWalls[i].transform.rotation);
			}
			else if (random <= 7)
			{
				SwitchBox.Instantiate (largeObjects[0], new Vector3(defenseWalls[i].transform.position.x, 0.5f, defenseWalls[i].transform.position.z), defenseWalls[i].transform.rotation);
			}
			else if (random <= 12)
			{
				SwitchBox.Instantiate (largeObjects[1], new Vector3(defenseWalls[i].transform.position.x, 1.0f, defenseWalls[i].transform.position.z), defenseWalls[i].transform.rotation);
			}
		}
	}
	
	// Game Controlling
	IEnumerator WaveController ()
	{
		RunRecorder.acces.level = 1;
		
		while (!GameStates.acces.gameDone)
		{
			enemyTanks = RunRecorder.acces.level;
			tanksLeft = RunRecorder.acces.level;
			
			for (int i = 0; i < RunRecorder.acces.level; i++)
			{
				int spawnPoint = (int) Random.Range (-0.5f, 3.5f);
				
				SwitchBox.Instantiate (enemyTank, npcSpawnPoints[spawnPoint].transform.position, npcSpawnPoints[spawnPoint].transform.rotation);
				
				yield return new WaitForSeconds (Random.Range (2, 6));
			}
			
			while (!waveCleared)
			{
				yield return new WaitForSeconds (0.5f);
			}
			
			waveCleared = false;
			
			while (GameStates.acces.gameDone)
			{
				yield return new WaitForSeconds (0.5f);
			}
			
			AddScore (pointsPerLevel);
			
			yield return new WaitForSeconds (6);
			RunRecorder.acces.level++;
		}
	}

	void GameDoneCheck ()
	{
		if (pointCaptured && !GameStates.acces.gameDone)
		{
			StartCoroutine (GameOverSet());
		}
	}

	// Game Done
	public override void ChildGameOver ()
	{
		GameOverText.acces.RequestTextLine ("Waves Survived: " + RunRecorder.acces.level, fontSize);
		GameOverText.acces.RequestTextLine ("Time Survived: " + (int) RunRecorder.acces.runTime + "s", fontSize);

		GameOverText.acces.RequestSeperatorLine ();

		GameOverText.acces.RequestTextLine ("Total Score: " + RunRecorder.acces.runScore, fontSize);

		GameOverText.acces.done = true;
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
		ScoreText.acces.EditLine (0, (enemyTanks - tanksLeft) + " of " + enemyTanks + " tanks destroyed");
		ScoreText.acces.EditLine (1, "Points: " + (int) showScore);
		ScoreText.acces.EditLine (2, "Wave " + RunRecorder.acces.level);
	}
}
