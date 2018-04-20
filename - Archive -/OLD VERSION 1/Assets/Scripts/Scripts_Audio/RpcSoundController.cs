using UnityEngine;
using System.Collections;

public class RpcSoundController : MonoBehaviour
{
	public void PlaySound ()
	{
		if (SwitchBox.isClient || SwitchBox.isServer)
		{
			networkView.RPC("StartPlaying", RPCMode.AllBuffered);
		}
		else
		{
			StartPlaying ();
		}
	}

	[RPC] void StartPlaying ()
	{
		audio.Play ();
	}
}
