using UnityEngine;
using System.Collections;

public class DynamicObjectBehaviour : MonoBehaviour, IDestroyable
{
	public GameObject effects;
	public float explosionForceHorizontal;
	public float explosionForceVertical;

	public void Destroy (string destroyer)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Destroy (gameObject);

			GameObject effectsClone = SwitchBox.Instantiate (effects, transform.position, transform.rotation);
			PartsHolder partsHolder = effectsClone.GetComponent<PartsHolder>();

			if (partsHolder != null)
			{
				for (int i = 0; i < partsHolder.explosion_parts.Length; i++)
				{
					partsHolder.explosion_parts[i].GetComponent<Rigidbody>().AddForce (new Vector3 (Random.Range(-explosionForceHorizontal,explosionForceHorizontal), Random.Range(0,explosionForceVertical), Random.Range(-explosionForceHorizontal,explosionForceHorizontal)), ForceMode.VelocityChange);
				}
			}
		}
	}
}
