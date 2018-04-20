using UnityEngine;
using System.Collections;

public class GameOverExplosion : MonoBehaviour
{
	public float waitTime;
	private float shake;
	
	void Start ()
	{
		StartCoroutine (Expand ());
		StartCoroutine (Shake ());
	}
	
	IEnumerator Expand ()
	{
		yield return new WaitForSeconds (waitTime);
		while (true)
		{
			transform.localScale = transform.localScale + Vector3.one * Time.deltaTime * 20.0f;
			yield return null;
		}
	}

	IEnumerator Shake ()
	{
		while (shake < 0.8f)
		{
			shake += 0.5f * Time.deltaTime;
			CameraShake.acces.SetAmplitude ("GameOverExplosion", shake);
			yield return null;
		}

		yield return new WaitForSeconds (2.8f);

		while (shake > 0f)
		{
			shake -= 0.5f * Time.deltaTime;
			CameraShake.acces.SetAmplitude ("GameOverExplosion", shake);
			yield return null;
		}

		CameraShake.acces.Unsubscribe ("GameOverExplosion");
	}

	void OnTriggerStay (Collider other)
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
