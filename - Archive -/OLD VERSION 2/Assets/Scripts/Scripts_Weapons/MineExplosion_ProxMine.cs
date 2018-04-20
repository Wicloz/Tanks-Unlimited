using UnityEngine;
using System.Collections;

public class MineExplosion_ProxMine : MineExplosion
{
	public override void ClientEffects ()
	{
		CameraShake.acces.SetShake ("MineExplosion", ShakeValues.mine_intensity * (transform.localScale.x + 0.1f), ShakeValues.mine_duration, ShakeValues.mine_fade);
	}

	public override void ReactToTrigger (GameObject other)
	{
		if (other.tag == Tags.shell)
		{
			other.GetComponent<ShellBehaviour>().DestroyShell (true);
		}
		
		else if (other.name == "mine_destroyCollider")
		{
			other.transform.parent.GetComponent<MineBehaviour>().Explode ();
		}
		
		else if (other.tag == Tags.playerTank || other.tag == Tags.npcTank || other.tag == Tags.dynamicObject || other.tag == Tags.dynamicPart)
		{
			other.GetComponent<IDestroyable>().Destroy (shooter);
		}
		
		else if (other.tag != Tags.staticObject && other.tag != Tags.ignoredByWeapons && other.tag != Tags.mine)
		{
			Debug.LogWarning ("Unknown object " + other.gameObject.name + " hit by MineExplosion");
		}
	}
}
