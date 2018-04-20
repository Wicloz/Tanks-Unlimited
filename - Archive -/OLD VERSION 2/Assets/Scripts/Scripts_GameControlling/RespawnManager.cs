using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
	public static RespawnManager acces;
	public List<UserSpawnInfo> spawnList = new List<UserSpawnInfo>();

	public List<GameObject> playerTanks = new List<GameObject>();
	public List<GameObject> playerReticules = new List<GameObject>();

	public List<GameObject> spawnPoints = new List<GameObject>();

	public UserSpawnInfo thisUser
	{
		get
		{
			foreach (UserSpawnInfo user in spawnList)
			{
				if (user.username == ProfileManager.acces.player.username)
				{
					return user;
				}
			}
			return null;
		}
	}

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		spawnPoints.Add (GameObject.Find ("spawnPoint_player_1"));
		spawnPoints.Add (GameObject.Find ("spawnPoint_player_2"));
		spawnPoints.Add (GameObject.Find ("spawnPoint_player_3"));
		spawnPoints.Add (GameObject.Find ("spawnPoint_player_4"));

		spawnList.Add (new UserSpawnInfo (ProfileManager.acces.player.username, 0));
	}
	
	void Update ()
	{
		if (SwitchBox.isHost && MultiplayerManager.acces.gameState == GameState.Running)
		{
			foreach (UserSpawnInfo user in spawnList)
			{
				user.tank = GameObject.Find ("PlayerTank_" + user.spawnNumber + "_" + user.username);
				user.reticule = GameObject.Find ("Reticule_" + user.spawnNumber + "_" + user.username);

				if (user.respawnTimer > 0)
				{
					user.respawnTimer -= Time.deltaTime;
				}
				
				if (user.respawnTimer <= 0 && user.isDead)
				{
					if (user.GetUserInfo().lifes >= GameController.acces.respawnCost)
					{
						user.isDead = false;

						if (SwitchBox.isServer)
						{
							GetComponent<NetworkView>().RPC ("SpawnSelf", RPCMode.All, user.username, user.spawnNumber);
						}
						else if (!SwitchBox.isClient)
						{
							SpawnSelf (user.username, user.spawnNumber);
						}
					}
				}
			}
		}
	}

	[RPC] private void SpawnSelf (string username, int spawnNumber)
	{
		if (username == ProfileManager.acces.player.username)
		{
			if (thisUser.reticule == null)
			{
				thisUser.reticule = (GameObject) SwitchBox.Instantiate (playerReticules[spawnNumber], new Vector3 (0, -22, 0), playerReticules[spawnNumber].transform.rotation);
				thisUser.reticule.name = "Reticule_" + spawnNumber + "_" + username;
			}

			thisUser.tank = (GameObject) SwitchBox.Instantiate (playerTanks[spawnNumber], new Vector3 (spawnPoints[spawnNumber].transform.position.x, 0.01f, spawnPoints[spawnNumber].transform.position.z), spawnPoints[spawnNumber].transform.rotation);
			thisUser.tank.name = "PlayerTank_" + spawnNumber + "_" + username;
		}
	}
	
	public void OnPlayerDestroyed (string playerName)
	{
		if (SwitchBox.isHost && MultiplayerManager.acces.gameState == GameState.Running)
		{
			foreach (UserSpawnInfo user in spawnList)
			{
				if (playerName == user.username)
				{
					user.GetUserInfo().lifes -= GameController.acces.respawnCost;
					user.respawnTimer = MultiplayerManager.acces.gameSettings.respawnTime;
					user.isDead = true;
				}
			}
		}
	}

	// Update userlist
	public void UpdateList ()
	{
		foreach (UserInfo user in UserManager.acces.userList)
		{
			bool userExists = false;
			List<int> numbersTaken = new List<int>();

			foreach (UserSpawnInfo spawnUser in spawnList)
			{
				numbersTaken.Add (spawnUser.spawnNumber);

				if (spawnUser.username == user.username)
				{
					userExists = true;
				}
			}

			if (!userExists && user.isOnline)
			{
				numbersTaken.Sort ();
				int lastNumber = 0;

				foreach (int number in numbersTaken)
				{
					if (number == lastNumber || number == lastNumber + 1)
					{
						lastNumber = number;
					}
					else
					{
						break;
					}
				}

				spawnList.Add (new UserSpawnInfo (user.username, lastNumber + 1));
			}
		}
	}

	public void RemoveUser (string username)
	{
		for (int i = 0; i < spawnList.Count; i++)
		{
			if (spawnList[i].username == username)
			{
				SwitchBox.Destroy (spawnList[i].tank);
				SwitchBox.Destroy (spawnList[i].reticule);
				spawnList.RemoveAt (i);
				break;
			}
		}
	}
	
	// GameControlling
	public bool AllGameOver ()
	{
		foreach (UserSpawnInfo user in spawnList)
		{
			if (!user.gameOver)
			{
				return false;
			}
		}
		return true;
	}
}

[System.Serializable]
public class UserSpawnInfo
{
	public string username = "ERROR";
	public GameObject tank;
	public GameObject reticule;

	public int spawnNumber;

	public float respawnTimer = 0;
	public bool isDead = true;

	public bool gameOver
	{
		get
		{
			return (isDead && (GetUserInfo() == null || GetUserInfo().lifes == 0 || !GetUserInfo().isOnline));
		}
	}

	public UserInfo GetUserInfo ()
	{
		return UserManager.acces.GetUser (username);
	}

	public UserSpawnInfo (string username, int spawnNumber)
	{
		this.username = username;
		this.spawnNumber = spawnNumber;
	}
}
