using UnityEngine;
using System.Collections;

public class GameStates : MonoBehaviour
{
	public static GameStates acces;

	// GameStates
	public bool startInitialised = false;
	public bool endInitialised = false;
	public bool gameStarting = false;
	public bool stageCleared = false;
	public bool gameDone = false;
	private bool gameOver = false;

	public bool GameOver
	{
		get
		{
			return gameOver;
		}
		set
		{
			if (SwitchBox.isServerOn)
			{
				networkView.RPC("SetGameOver", RPCMode.AllBuffered, value);
			}
			else
			{
				SetGameOver (value);
			}
		}
	}

	[RPC] void SetGameOver (bool state)
	{
		gameOver = state;
	}

	public void StartGame ()
	{
		networkView.RPC ("SetGameStarting", RPCMode.AllBuffered, true);
	}

	[RPC] void SetGameStarting (bool state)
	{
		gameStarting = state;
	}

	protected void Awake ()
	{
		acces = this;
	}

	// Syncing values
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		bool tempBool1 = false;
		bool tempBool2 = false;
		
		if (SwitchBox.isServer && stream.isWriting)
		{
			tempBool1 = stageCleared;
			stream.Serialize(ref tempBool1);

			tempBool2 = gameDone;
			stream.Serialize(ref tempBool2);
		}
		else if (SwitchBox.isClient)
		{
			stream.Serialize(ref tempBool1);
			stageCleared = tempBool1;

			stream.Serialize(ref tempBool2);
			gameDone = tempBool2;
		}
	}
}
