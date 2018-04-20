using UnityEngine;
using System.Collections;
using Wicloz.Collections;

public class RunRecorder : MonoBehaviour
{
	public static RunRecorder acces;

	// Basic Run Data
	public float runScore = 0;
	public float runTime = 0;
	public int level;

	// Tank Destruction Log
	public SimpleDict <string, int> killLog = new SimpleDict <string, int> ();

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		int tempLevel = 0;
		
		if (SwitchBox.isServer && stream.isWriting)
		{
			tempLevel = level;
			stream.Serialize(ref tempLevel);
		}
		else if (SwitchBox.isClient)
		{
			stream.Serialize(ref tempLevel);
			level = tempLevel;
		}
	}

	void Awake ()
	{
		if (acces == null)
		{
			DontDestroyOnLoad(gameObject);
			acces = this;
		}
		else if (acces != this)
		{
			Destroy (gameObject);
		}
	}

	public void OnPlayerDestroyed (GameObject player)
	{
		if (!GameStates.acces.gameDone)
		{
			PlayerController playerScript = player.GetComponent <PlayerController>();
			string playerName = playerScript.type;

			if (SwitchBox.isServer)
			{
				networkView.RPC("OnPlayerDestroyedRPC", RPCMode.AllBuffered, playerName);
			}
			else if (!SwitchBox.isClient)
			{
				OnPlayerDestroyedRPC (playerName);
			}
		}
	}

	[RPC] void OnPlayerDestroyedRPC (string playerName)
	{
		if (killLog.ContainsKey (playerName))
		{
			killLog.Edit (playerName, killLog.Acces(playerName) + 1);
		}
		else
		{
			killLog.Add (playerName, 1);
		}
	}

	public void OnEnemyDestroyed (GameObject enemy)
	{
		if (!GameStates.acces.gameDone)
		{
			NpcController enemyScript = enemy.GetComponent <NpcController>();
			string tankType = enemyScript.type;

			if (SwitchBox.isServer)
			{
				networkView.RPC("OnEnemyDestroyedRPC", RPCMode.AllBuffered, tankType);
			}
			else if (!SwitchBox.isClient)
			{
				OnEnemyDestroyedRPC (tankType);
			}
		}
	}

	[RPC] void OnEnemyDestroyedRPC (string tankType)
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
