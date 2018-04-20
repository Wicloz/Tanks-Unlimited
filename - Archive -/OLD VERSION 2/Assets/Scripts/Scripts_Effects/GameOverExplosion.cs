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
	
	private IEnumerator Expand ()
	{
		yield return new WaitForSeconds (waitTime);
		while (true)
		{
			transform.localScale = transform.localScale + Vector3.one * Time.deltaTime * 20.0f;
			yield return null;
		}
	}

	private IEnumerator Shake ()
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
		if (other.tag == Tags.shell || other.tag == Tags.mine)
		{
			SwitchBox.Destroy (other.gameObject);
		}
		
		else if (other.tag == Tags.playerTank || other.tag == Tags.npcTank || other.tag == Tags.dynamicObject || other.tag == Tags.dynamicPart)
		{
			other.GetComponent<IDestroyable>().Destroy ("GOE");
		}
	}
}
