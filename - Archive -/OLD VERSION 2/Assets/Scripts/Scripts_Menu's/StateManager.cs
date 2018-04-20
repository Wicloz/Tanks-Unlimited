using UnityEngine;
using System.Collections;

public class StateManager : PersistentSingleton<StateManager>
{
	private HostData serverData = null;
	private bool isHost = true;
	private bool multiplayer = false;

	public bool serverOn = false;

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	void Update ()
	{
		if (!serverOn && Application.loadedLevel != 0)
		{
			serverOn = true;
			LevelLoaded ();
		}

		if (Application.loadedLevel == 0)
		{
			serverOn = false;
		}
	}

	// Joining/Starting Game
	private void LevelLoaded ()
	{
		MultiplayerManager.acces.gameState = GameState.Loading;

		if (isHost)
		{
			StartServer (multiplayer);
		}
		else
		{
			ConnectToServer (serverData);
		}
	}

	private void StartServer (bool multiplayer)
	{
		if (multiplayer)
		{
			MultiplayerManager.acces.StartOnlineServer ();
		}
		else
		{
			MultiplayerManager.acces.StartOfflineServer ();
			JoinGame (true);
		}
	}

	private void ConnectToServer (HostData hostData)
	{
		Network.Connect (hostData);
	}

	void OnConnectedToServer ()
	{
		StateManager.acces.JoinGame (false);
	}

	public void JoinGame (bool isHost)
	{
		if (isHost)
		{
			UserManager.acces.AddUser(ProfileManager.acces.player.username);
		}
		else
		{
			GetComponent<NetworkView>().RPC ("AddUser", RPCMode.Server, ProfileManager.acces.player.username);
		}

		Debug.LogError ("Server Joined");
		ChatManager.acces.SendServerMessage (ProfileManager.acces.player.username + " joined the server.");
	}

	public void StartLevel (string gamemode, bool multiplayer, bool isHost, HostData serverData)
	{
		MultiplayerManager.acces.gameSettings.gamemode = gamemode;
		this.multiplayer = multiplayer;
		this.isHost = isHost;
		this.serverData = serverData;

		switch (gamemode)
		{
		case "Play_InfiniMode":
			InfiniMode ();
			break;
		case "Play_DefenseMode":
			DefenseMode ();
			break;
		default:
			Debug.LogError ("Scene '" + gamemode + "' not found");
			break;
		}
	}

	// Leaving Game
	public void EndGame (string reason)
	{
		if (SwitchBox.isClient)
		{
			Network.Disconnect();
		}
		else if (SwitchBox.isServer)
		{
			GetComponent<NetworkView>().RPC("KickClient", RPCMode.Others, "", true, "Server Closed");
			MultiplayerManager.acces.StopServer ();
		}

		GameController.acces.GameOverPreSet ();
	}
	
	[RPC] public void KickClient (string username, bool allUsers, string reason)
	{
		if (username == ProfileManager.acces.player.username || allUsers)
		{
			EndGame (reason);
		}
	}

	public void GoToMainMenu ()
	{
		UserManager.acces.Reset ();
		MultiplayerManager.acces.Reset ();
		RunRecorder.acces.Reset ();
		LoadingScreen.acces.QueLevelChange ("MainMenu");
		MultiplayerManager.acces.gameState = GameState.Stopped;
	}

	// Gamemodes
	private void InfiniMode ()
	{
		LoadingScreen.acces.QueLevelChange ("Play_InfiniMode");
	}
	
	private void DefenseMode ()
	{
		LoadingScreen.acces.QueLevelChange ("Play_DefenseMode");
	}
}
