using UnityEngine;
using System.Collections;

public enum GameState {Stopped, Loading, Lobby, Running, Results};

public class MultiplayerManager : PersistentSingleton<MultiplayerManager>
{
	public GameState gameState = GameState.Stopped;

	public string passWord = "";
	public int maxUsers = 4;
	public const string gameName = "WiclozTanks";
	private string roomName = "null";
	public string protocolVersion = "A-0.1";

	public bool multiplayer = false;
	public GameSettings gameSettings = new GameSettings();
	
	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public override void _Start ()
	{
		StartCoroutine ("SynchroniseSettings");
	}

	public void Reset ()
	{
		roomName = "null";
		gameSettings.gamemode = "null";
		multiplayer = false;
	}

	// Hosting a server
	public void StartOnlineServer ()
	{
		StartOfflineServer ();
		Network.InitializeServer(maxUsers, 25000, !Network.HavePublicAddress());
		RegisterHost ();
	}

	public void StartOfflineServer ()
	{
		UserManager.acces.Reset ();
	}
	
	public void StopServer ()
	{
		MasterServer.UnregisterHost();
		Network.Disconnect();
		Debug.LogError ("Server Stopped");
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		// On Server Started !!
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
		{
			if (gameState == GameState.Loading)
			{
				Debug.LogError  ("Server Registration Succeeded");
				ChatManager.acces.SendServerMessage ("Server Started!");
				StateManager.acces.JoinGame (true);
			}
		}
		
		else if (msEvent == MasterServerEvent.RegistrationFailedNoServer)
		{
			StateManager.acces.EndGame ("Failed to start server");
		}
	}
	
	public void RegisterHost()
	{
		if (SwitchBox.isServer)
		{
			roomName = ProfileManager.acces.player.username + "'s Room";
			
			MasterServer.UnregisterHost ();
			MasterServer.RegisterHost (gameName, roomName, CommentManager.SerialiseComment(passWord, ProfileManager.acces.player.username, UserManager.acces.GetSimpleUserList(), gameSettings.gamemode, protocolVersion));
			Debug.LogError ("Server Registration Sent");
		}
	}

	// Streaming settings
	private IEnumerator SynchroniseSettings ()
	{
		while (true)
		{
			if (gameState == GameState.Lobby && SwitchBox.isServer)
			{
				SendSettings ();
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void SendSettings ()
	{
		Debug.Log ("Sending Settings ...");
		string settings = SerializerHelper.SerializeToString (gameSettings);
		GetComponent<NetworkView>().RPC ("RecieveSettings", RPCMode.Others, settings);
	}
	
	[RPC] public void RecieveSettings (string settings)
	{
		gameSettings = SerializerHelper.DeserializeFromString<GameSettings> (settings);
	}
}

[System.Serializable]
public class GameSettings
{
	public int objectDensity = 60;
	public int enemyTanks = 3;
	public int difficulty = 2;
	public string gamemode = "null";
	public int maxLifes = 1;
	public float respawnTime = 4;
	
	public GameSettings ()
	{}
}
