using UnityEngine;
using System.Collections;
using Wicloz.Collections;

public class RunRecorder : PersistentSingleton<RunRecorder>
{
	// Basic Run Data
	private float _runScore = 0;
	private int _kills = 0;
	private int _deaths = 0;

	public float runScore
	{
		get
		{
			return _runScore;
		}
		set
		{
			_runScore = value;
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetRunScore", RPCMode.All, ProfileManager.acces.player.username, value);
			}
			else
			{
				SetRunScore (ProfileManager.acces.player.username, value);
			}
		}
	}
	[RPC] private void SetRunScore (string username, float value)
	{
		if (UserManager.acces.GetUser(username) != null)
		{
			UserManager.acces.GetUser(username).points = value;
		}
	}

	public int kills
	{
		get
		{
			return _kills;
		}
		set
		{
			_kills = value;
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetKills", RPCMode.All, ProfileManager.acces.player.username, value);
			}
			else
			{
				SetKills (ProfileManager.acces.player.username, value);
			}
		}
	}
	[RPC] private void SetKills (string username, int value)
	{
		if (UserManager.acces.GetUser(username) != null)
		{
			UserManager.acces.GetUser(username).kills = value;
		}
	}

	public int deaths
	{
		get
		{
			return _deaths;
		}
		set
		{
			_deaths = value;
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetDeaths", RPCMode.All, ProfileManager.acces.player.username, value);
			}
			else
			{
				SetDeaths (ProfileManager.acces.player.username, value);
			}
		}
	}
	[RPC] private void SetDeaths (string username, int value)
	{
		if (UserManager.acces.GetUser(username) != null)
		{
			UserManager.acces.GetUser(username).deaths = value;
		}
	}

	public int level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC ("SetLevel", RPCMode.OthersBuffered, value);
			}
			else
			{
				SetLevel (value);
			}
		}
	}
	[RPC] private void SetLevel (int value)
	{
		_level = value;
	}

	private int _level = 0;
	public float runTime = 0;

	// Tank Destruction Log
	public SimpleDict <string, int> killLog = new SimpleDict <string, int> ();

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public override void _OnDestroy ()
	{
		Reset ();
		ProfileManager.acces.SaveProfile ();
	}

	public void Reset ()
	{
		ProfileManager.acces.player.points += runScore;

		runScore = 0;
		kills = 0;
		deaths = 0;
		runTime = 0;
		level = 0;
	}

	public void OnPlayerDestroyed (string playerName, float playerPoints, string destroyer)
	{
		if (MultiplayerManager.acces.gameState == GameState.Running)
		{
			if (SwitchBox.isServer)
			{
				GetComponent<NetworkView>().RPC("OnPlayerDestroyedRPC", RPCMode.All, playerName, destroyer, GameController.acces.playerPoints);
			}
			else if (!SwitchBox.isClient)
			{
				OnPlayerDestroyedRPC (playerName, destroyer, GameController.acces.playerPoints);
			}

			if (GameController.acces.playerPoints && playerName != destroyer)
			{
				GameController.acces.AddScore (playerPoints, destroyer, false);
			}
		}
	}

	[RPC] void OnPlayerDestroyedRPC (string playerName, string username, bool givePoints)
	{
		if (playerName == ProfileManager.acces.player.username)
		{
			deaths ++;
		}
		
		if (ProfileManager.acces.player.username == username && playerName != username && givePoints)
		{
			kills ++;

			if (killLog.ContainsKey (playerName))
			{
				killLog.Edit (playerName, killLog.Acces(playerName) + 1);
			}
			else
			{
				killLog.Add (playerName, 1);
			}
		}
	}

	public void OnEnemyDestroyed (string tankName, float tankScore, string destroyer)
	{
		if (MultiplayerManager.acces.gameState == GameState.Running)
		{
			if (SwitchBox.isServer)
			{
				GetComponent<NetworkView>().RPC("OnEnemyDestroyedRPC", RPCMode.All, tankName, destroyer, GameController.acces.coop);
			}
			else if (!SwitchBox.isClient)
			{
				OnEnemyDestroyedRPC (tankName, destroyer, GameController.acces.coop);
			}

			GameController.acces.AddScore (tankScore, destroyer, GameController.acces.coop);
		}
	}

	[RPC] void OnEnemyDestroyedRPC (string tankType, string username, bool allUsers)
	{
		if (username == ProfileManager.acces.player.username)
		{
			kills ++;
		}
		
		if (ProfileManager.acces.player.username == username || allUsers)
		{
			if (killLog.ContainsKey (tankType))
			{
				killLog.Edit (tankType, killLog.Acces(tankType) + 1);
			}
			else
			{
				killLog.Add (tankType, 1);
			}
		}
	}
}
