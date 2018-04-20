using UnityEngine;
using System.Collections;

public class UpgradeManagerSecond : MonoBehaviour
{
	public Material activated;
	public Material purchased;
	public Material locked;
		
	void Update ()
	{
	}

	void OnMouseDown ()
	{
		switch (transform.name)
		{
		case "null":
			break;
				
		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}
}
