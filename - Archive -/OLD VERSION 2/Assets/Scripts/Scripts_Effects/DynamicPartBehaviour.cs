using UnityEngine;
using System.Collections;

public class DynamicPartBehaviour : MonoBehaviour, IDestroyable
{
	public GameObject effects;
	
	public void Destroy (string destroyer)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Destroy (gameObject);
			SwitchBox.Instantiate (effects, transform.position, transform.rotation);
		}
	}
}
