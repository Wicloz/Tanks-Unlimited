using UnityEngine;
using System.Collections;

public class GlobalObjectManager : MonoBehaviour
{
	public static GlobalObjectManager acces;
	public bool isEnabled = false;

	void Awake ()
	{
		if (isEnabled)
		{
			if (acces == null)
			{
				acces = this;
				DontDestroyOnLoad (gameObject);
			}
			else
			{
				Destroy (gameObject);
			}
		}
	}
}
