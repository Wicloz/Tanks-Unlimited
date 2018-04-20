using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager acces;

	// Data
	private int playerAmount;
	private bool canSpawn = true;

	// Slot data
	public int thisSlot;

	// GameObjects
	public GameObject[] playerTanks = {null, null, null, null};
	private GameObject[] spawnPoints = {null, null, null, null};
	public GameObject playerTankPrefab;

	// Reticules
	public GameObject reticule1;
	public GameObject reticule2;
	
	// Variables for respawning
	public float respawnWait;
	private bool respawning;
	public bool hasSpawned = false;

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		int tempPlayerAmount = 0;

		if (SwitchBox.isServer && stream.isWriting)
		{
			tempPlayerAmount = playerAmount;
			stream.Serialize(ref tempPlayerAmount);
		}
		else if (SwitchBox.isClient)
		{
			stream.Serialize(ref tempPlayerAmount);
			playerAmount = tempPlayerAmount;
		}
	}

	void Awake ()
	{
		acces = this;
	}
	
	void Start ()
	{
		playerAmount = GlobalValues.acces.playerAmount;
		
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i] = GameObject.Find ("spawnPoint_player_" + (i + 1));
		}

		if (!SwitchBox.isClient)
		{
			thisSlot = 0;
		}
		else
		{
			thisSlot = 1;
		}
	}

	public void SpawnReticule ()
	{
		if (thisSlot == 0)
		{
			SwitchBox.Instantiate (reticule1, new Vector3 (0, 0, 0), reticule1.transform.rotation);
		}
		else if (thisSlot == 1)
		{
			SwitchBox.Instantiate (reticule2, new Vector3 (0, 0, 0), reticule2.transform.rotation);
		}
	}

	void Update ()
	{
		playerTanks[thisSlot] = GameObject.Find ("ThisPlayerTank");

		for (int i = 0; i < playerTanks.Length; i ++)
		{
			if (i != thisSlot)
			{
				GameObject playerTank = GameObject.Find ("PlayerTank(Clone)");
				if (playerTank != null)
				{
					playerTank.name = "PlayerTank" + i;
				}

				playerTanks[i] = GameObject.Find ("PlayerTank" + i);
			}
		}

		if (canSpawn && !GameStates.acces.gameDone && GameStates.acces.endInitialised)
		{
			if (playerTanks[thisSlot] == null)
			{
				StartRespawn (respawnWait, false);
			}
		}
	}

	// Respawning
	public void StartRespawn (float wait, bool firstspawn)
	{
		if (firstspawn)
		{
			GlobalValues.acces.lives ++;
		}

		if (!respawning)
		{
			respawning = true;
			Invoke ("RespawnPlayer", wait);
		}
	}
	
	void RespawnPlayer ()
	{
		GlobalValues.acces.lives --;
		playerTanks[thisSlot] = SwitchBox.Instantiate (playerTankPrefab, spawnPoints[thisSlot].transform.position, spawnPoints[thisSlot].transform.rotation);
		playerTanks[thisSlot].name = "ThisPlayerTank";

		CheckCanRespawn ();
		respawning = false;
		hasSpawned = true;
		FirstPerson.acces.CameraRespawn ();
	}

	void CheckCanRespawn ()
	{
		if (GlobalValues.acces.lives != 0)
		{
			canSpawn = true;
		}
		else
		{
			canSpawn = false;
		}
	}

	public void OnPlayerDestroy ()
	{
		FirstPerson.acces.OnPlayerCameraDestroy ();
	}

	// Managing
	void OnPlayerConnected ()
	{
		playerAmount ++;
	}
	
	void OnPlayerDisconnected ()
	{
		playerAmount --;
	}

	public bool AreAllDead ()
	{
		bool areDead = true;

		for (int i = 0; i < playerTanks.Length; i++)
		{
			if (playerTanks[i] != null)
			{
				areDead = false;
			}
		}

		return areDead;
	}

	public GameObject GetTank (int tank)
	{
		if (playerTanks[tank] != null)
		{
			return playerTanks [tank];
		}
		else
		{
			return null;
		}
	}
}
