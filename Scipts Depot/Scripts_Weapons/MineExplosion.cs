using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour
{
	public ParticleSystem explosion;

	public float explosionDuration;
	private bool exploding = true;

	private ShellBehaviour shell;

	void Start ()
	{
		CameraShake.acces.SetShake ("MineExplosion", ShakeValues.mine_intensity * (transform.localScale.x + 0.1f), ShakeValues.mine_duration, ShakeValues.mine_fade);

		if (SwitchBox.isClient)
		{
			Destroy (gameObject.GetComponent<MineExplosion>());
		}

		StartCoroutine (StopExploding());
	}

	void OnTriggerStay (Collider other)
	{
		if (exploding)
		{
			if (other.tag == Tags.dynamicObject || other.tag == Tags.playerTank || other.tag == Tags.npcTank || other.tag == Tags.shell || other.tag == Tags.mine || other.tag == Tags.dynamicPart)
			{
				switch (other.gameObject.tag)
				{
				case Tags.dynamicObject:
					Destructor.DynamicObject (other.gameObject);
					break;
				case Tags.dynamicPart:
					Destructor.DynamicPart (other.gameObject);
					break;
				case Tags.playerTank:
					Destructor.PlayerTank (other.gameObject);
					break;
				case Tags.npcTank:
					Destructor.NpcTank (other.gameObject);
					break;
				case Tags.shell:
					other.gameObject.GetComponent <ShellBehaviour>().DestroyShell(null);
					break;
				case Tags.mine:
					StartCoroutine (other.gameObject.GetComponent <MineDestroyer>().Explode("inner"));
					break;
				}
			}
		}
	}

	public void SetProperties (float setRadius)
	{
		explosion.startSize = setRadius / 0.7f;
		transform.localScale = new Vector3 (setRadius, setRadius, setRadius);
	}
	
	IEnumerator StopExploding ()
	{
		yield return new WaitForSeconds (explosionDuration);
		exploding = false;
		yield return new WaitForSeconds (4);
		SwitchBox.Destroy (gameObject);
	}
}
