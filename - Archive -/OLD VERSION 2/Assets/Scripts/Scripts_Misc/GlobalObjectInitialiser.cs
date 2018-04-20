using UnityEngine;
using System.Collections;

public class GlobalObjectInitialiser : MonoBehaviour
{
	public GameObject globalPrefab;

	void Awake ()
	{
		if (PrefabHolder.acces == null)
		{
			GameObject scriptHolder = (GameObject) Object.Instantiate (globalPrefab, globalPrefab.transform.position, globalPrefab.transform.rotation);
			scriptHolder.name = "GlobalScripts";
			DontDestroyOnLoad (scriptHolder);
		}
		
		Destroy (gameObject);
	}
}
