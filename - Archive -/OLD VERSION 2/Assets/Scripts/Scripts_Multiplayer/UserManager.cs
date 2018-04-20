using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class States
{
	public bool sceneLoaded = false;
	public bool ready = false;
	public bool levelLoaded = false;

	public void Reset ()
	{
		sceneLoaded = false;
		ready = false;
		levelLoaded = false;
	}

	public States ()
	{}
}

[System.Serializable]
public class UserInfo
{
	public string username;

	public bool isOnline = true;
	public bool isWelcomed = false;

	public bool checkPing = false;
	public bool isHost = false;

	public States states = new States ();

	public int kills = 0;
	public int deaths = 0;
	public float points = 0;
	public int lifes = 1;

	public int showLifes
	{
		get
		{
			return Mathf.Max (lifes - 1, 0);
		}
	}

	public UserInfo ()
	{
		username = "ERROR";
	}
	
	public UserInfo (string Username)
	{
		username = Username;
	}

	public UserInfo Clone ()
	{
		return SerializerHelper.DeserializeFromString<UserInfo> (SerializerHelper.SerializeToString (this));
	}

	public void CleanClientData ()
	{}

	public void ResetGameInfo ()
	{
		kills = 0;
		deaths = 0;
		points = 0;
		ResetSceneInfo ();
	}

	public void ResetSceneInfo ()
	{
		states.Reset();
	}
}

public class UserManager : PersistentSingleton<UserManager>
{
	public List<UserInfo> userList = new List<UserInfo>();
	private List<string> lastUserList = new List<string>();
	public List<bool> pingList = new List<bool>();

	// Retrieve information
	public List<string> GetSimpleUserList ()
	{
		List<string> returnList = new List<string> ();
		
		for (int i = 0; i < userList.Count; i++)
		{
			if (userList[i].isOnline)
			{
				returnList.Add(userList[i].username);
			}
		}
		
		return returnList;
	}
	
	public bool AllUsersReady ()
	{
		foreach (UserInfo user in userList)
		{
			if (!user.states.ready && user.isOnline)
			{
				return false;
			}
		}
		return true;
	}
	
	public bool AllUsersSceneLoaded ()
	{
		foreach (UserInfo user in userList)
		{
			if (!user.states.sceneLoaded && user.isOnline)
			{
				return false;
			}
		}
		return true;
	}
	
	public bool AllUsersLevelLoaded ()
	{
		foreach (UserInfo user in userList)
		{
			if (!user.states.levelLoaded && user.isOnline)
			{
				return false;
			}
		}
		return true;
	}

	public int onlinePlayers
	{
		get
		{
			int i = 0;
			foreach (UserInfo user in userList)
			{
				if (user.isOnline)
				{
					i++;
				}
			}
			return i;
		}
	}

	public UserInfo thisUser
	{
		get
		{
			foreach (UserInfo user in userList)
			{
				if (user.username == ProfileManager.acces.player.username)
				{
					return user;
				}
			}
			return null;
		}
	}

	public UserInfo GetUser (string username)
	{
		foreach (UserInfo user in userList)
		{
			if (user.username == username)
			{
				return user;
			}
		}
		
		return null;
	}

	// Basic Functions
	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public override void _Start ()
	{
		SetLastUserList ();
		StartCoroutine (CheckForTimeouts ());
	}

	public override void _OnDestroy ()
	{
		if (SwitchBox.isServer && SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC("KickClient", RPCMode.Others, "", true, "Server Closed");
		}
	}

	public void Reset ()
	{
		userList = new List<UserInfo>();
		pingList = new List<bool>();
	}

	void Update ()
	{
		foreach (UserInfo user in userList)
		{
			if (!lastUserList.Contains(SerializerHelper.SerializeToString (user)))
			{
				SendUser (user.username);
			}
		}

		SetLastUserList ();
	}

	private void SetLastUserList ()
	{
		lastUserList = new List<string> ();

		foreach (UserInfo user in userList)
		{
			lastUserList.Add (SerializerHelper.SerializeToString (user));
		}
	}

	// Manipulate user data
	public void ResetPlayerGameInfo ()
	{
		for (int i = 0; i < userList.Count; i++)
		{
			userList[i].ResetGameInfo ();
			SendUser (userList[i].username);
		}
	}
	
	public void ResetPlayerLevelInfo ()
	{
		for (int i = 0; i < userList.Count; i++)
		{
			userList[i].ResetSceneInfo ();
			SendUser (userList[i].username);
		}
	}
	
	// Manage users
	public void KickUser (string username)
	{
		GetComponent<NetworkView>().RPC("KickClient", RPCMode.Others, username, false, "Kicked by Host");
		DisconnectUser (username, " was kicked.");
	}

	public void RemoveUser (string username)
	{
		for (int i = 0; i < userList.Count; i++)
		{
			if (userList[i].username == username)
			{
				RemoveUserAt(i);
				break;
			}
		}
	}

	public void RemoveOfflineUsers ()
	{
		for (int i = 0; i < userList.Count; i++)
		{
			if (!userList[i].isOnline)
			{
				RemoveUserAt(i);
				i--;
			}
		}
	}

	public void RemoveUserAt (int index)
	{
		RespawnManager.acces.RemoveUser (userList[index].username);
		userList.RemoveAt(index);
	}

	// Managing player connections
	[RPC] public void AddUser (string username)
	{
		bool userExists = false;
		UserInfo joinUser = new UserInfo (username);
		
		for (int i = 0; i < userList.Count; i++)
		{
			if (userList[i].username == username)
			{
				userExists = true;
				userList[i].isOnline = true;
				joinUser = userList[i];
				break;
			}
		}
		
		if (!userExists)
		{
			if (username == ProfileManager.acces.player.username)
			{
				joinUser.isHost = true;
			}
			
			userList.Add (joinUser);
		}

		if (!joinUser.isWelcomed)
		{
			// Code when a player joins the game
			if (SwitchBox.isServerOn)
			{
				MultiplayerManager.acces.SendSettings ();
			}
			
			if (MultiplayerManager.acces.gameState == GameState.Running || MultiplayerManager.acces.gameState == GameState.Results || MultiplayerManager.acces.gameState == GameState.Lobby)
			{
				LevelController.acces.SendLevelData (joinUser.username, false);
			}
			// End join code
			joinUser.isWelcomed = true;
		}

		if (RespawnManager.acces != null)
		{
			RespawnManager.acces.UpdateList ();
		}
		
		foreach (UserInfo user in userList)
		{
			SendUser (user.username);
		}
		
		MultiplayerManager.acces.RegisterHost ();
	}

	[RPC] public void DisconnectUser (string username, string reason)
	{
		foreach (UserInfo user in userList)
		{
			if (user.username == username)
			{
				user.isOnline = false;
				user.isWelcomed = false;
				user.ResetGameInfo ();
				RespawnManager.acces.RemoveUser (user.username);
				ChatManager.acces.SendServerMessage(user.username + reason);
				SendUser (username);
				break;
			}
		}
	}
	
	void OnPlayerConnected ()
	{}

	void OnPlayerDisconnected ()
	{
		CheckConnection (" left the game.");
	}

	// Ping system
	private IEnumerator CheckForTimeouts ()
	{
		while (true)
		{
			yield return new WaitForSeconds (10);

			if (SwitchBox.isServerOn && SwitchBox.isServer)
			{
				CheckConnection (" timed out.");
			}

			yield return new WaitForSeconds (10);
		}
	}

	public void CheckConnection (string message)
	{
		pingList = new List<bool>();
		
		foreach (UserInfo user in userList)
		{
			pingList.Add(false);
			
			if (user.username != ProfileManager.acces.player.username && user.isOnline)
			{
				user.checkPing = true;
				GetComponent<NetworkView>().RPC("Marco", RPCMode.All, user.username);
			}
			else
			{
				user.checkPing = false;
			}
		}

		StopCoroutine ("DelayedConnectionCheck");
		StartCoroutine ("DelayedConnectionCheck", message);
	}

	[RPC] public void Polo (string username)
	{
		if (SwitchBox.isHost)
		{
			for (int i = 0; i < userList.Count; i++)
			{
				if (userList[i].username == username)
				{
					pingList[i] = true;
					break;
				}
			}
		}
	}

	private IEnumerator DelayedConnectionCheck (string message)
	{
		bool done = false;
		float starttime = Time.time;

		while (!done)
		{
			if (Time.time - starttime > 1)
			{
				for (int i = 0; i < userList.Count; i++)
				{
					if (userList[i].checkPing && !pingList[i])
					{
						DisconnectUser (userList[i].username, message);
					}
				}

				done = true;
			}

			yield return null;
		}

		MultiplayerManager.acces.RegisterHost ();
	}

	// Client synchronisation
	private void SendUser (string username)
	{
		if (SwitchBox.isServer)
		{
			foreach (UserInfo user in userList)
			{
				if (user.username == username)
				{
					UserInfo parsedUser = user.Clone ();
					parsedUser.CleanClientData ();

					Debug.Log ("Sending user " + user.username);

					List<string> stringSections = SerializerHelper.SerializeToString (parsedUser, 4);
					GetComponent<NetworkView>().RPC("RecieveUser", RPCMode.Others, stringSections[0], stringSections[1], stringSections[2], stringSections[3]);
					break;
				}
			}
		}
	}
	
	[RPC] public void RecieveUser (string string_1, string string_2, string string_3, string string_4)
	{
		if (!SwitchBox.isServer)
		{
			string userString = string_1 + string_2 + string_3 + string_4;
			
			UserInfo user = SerializerHelper.DeserializeFromString<UserInfo> (userString);
			bool userExists = false;
			
			for (int i = 0; i < userList.Count; i++)
			{
				if (userList[i].username == user.username)
				{
					userList[i] = user;
					userExists = true;
					break;
				}
			}
			
			if (!userExists)
			{
				userList.Add (user);
			}
		}
	}
}
