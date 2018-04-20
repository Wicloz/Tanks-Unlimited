using UnityEngine;
using System.Collections;

public class MultiplayerTankDestruction : MonoBehaviour, IDestroyable
{
	public GameObject tankExplosion;
	
	public string tankType;
	
	public string tankName;
	public float tankScore;
	
	public void Destroy (string destroyer)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Destroy (gameObject);

			switch (tankType)
			{
			case "PlayerTank":
				RunRecorder.acces.OnPlayerDestroyed (tankName, tankScore, destroyer);
				RespawnManager.acces.OnPlayerDestroyed (tankName);
				SwitchBox.Instantiate (tankExplosion, transform.position, transform.rotation);
				break;

			case "NpcTank":
				RunRecorder.acces.OnEnemyDestroyed (tankName, tankScore, destroyer);
				GameController.acces.OnNpcTankDestroy ();
				SwitchBox.Instantiate (tankExplosion, transform.position, transform.rotation);
				break;
			}
		}
	}

	void Destroy ()
	{
		CameraShake.acces.SetShake ("OtherTank", ShakeValues.otherTank_intensity, ShakeValues.otherTank_duration, ShakeValues.otherTank_fade);
	}

	[RPC] public void RecieveValues (string tankType, string tankName, float tankScore)
	{
		this.tankType = tankType;
		this.tankName = tankName;
		this.tankScore = tankScore;
	}
}
