using UnityEngine;
using System.Collections;

public class ShotSpawnProber : MonoBehaviour
{
	public bool isInObject = false;

	void OnTriggerStay (Collider collision)
	{
		if (collision.gameObject.tag == Tags.staticObject || collision.gameObject.tag == Tags.dynamicObject)
		{
			isInObject = true;
		}
		else
		{
			isInObject = false;
		}
	}

	void OnTriggerExit ()
	{
		isInObject = false;
	}
}
