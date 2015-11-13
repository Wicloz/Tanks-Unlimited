using UnityEngine;
using System.Collections;

public class RpcSoundController : MonoBehaviour
{
	public void PlaySound ()
	{
		if (SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC("StartPlaying", RPCMode.All);
		}
		else
		{
			StartPlaying ();
		}
	}

	[RPC] void StartPlaying ()
	{
		GetComponent<AudioSource>().Play ();
	}
}
