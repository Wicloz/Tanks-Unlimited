using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : PersistentSingleton<PrefabHolder>
{
	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public Prefab enemyTank;

	public List<Prefab> staticObjects = new List<Prefab>();
	public List<Prefab> dynamicObjects = new List<Prefab>();
}

[System.Serializable]
public class Prefab
{
	public GameObject gameObject;
	public float spawnHeight;
}
