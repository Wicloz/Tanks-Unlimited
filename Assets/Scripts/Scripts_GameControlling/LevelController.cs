using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
	public static LevelController acces;
	private List<LevelObject> levelObjects = new List<LevelObject>();

	public void SetObjects (List<LevelObject> objects)
	{
		levelObjects = new List<LevelObject> (objects);
	}

	void Awake ()
	{
		acces = this;
	}

	// Generate level presets
	public void GenerateRandomLevel (WorldRect generateRect, WorldRect excludeRect, float density, float dynamicPercentage, bool checkForSpace)
	{
		WorldRect[] excludeRects = {excludeRect};
		GenerateRandomLevel (generateRect, excludeRects, density, dynamicPercentage, checkForSpace);
	}
		
	public void GenerateRandomLevel (WorldRect generateRect, WorldRect[] excludeRects, float density, float dynamicFraction, bool checkForSpace)
	{
		List<LevelObject> localLevelObjects = new List<LevelObject> ();

		//int area = generateRect.GetArea();
		//foreach (WorldRect rect in excludeRects)
		//{
			//area -= rect.GetArea();
		//}

		for (float i = generateRect.left; i <= generateRect.right; i++)
		{
			for (float j = generateRect.bottom; j <= generateRect.top; j++)
			{
				float random = Random.Range (0f, 1f);

				if (random <= density)
				{
					bool goodSpot = false;
					bool inExclusion = false;
					
					if (checkForSpace)
					{
						RaycastHit hit;
						if (Physics.Raycast (new Vector3(i, 4, j), Vector3.down, out hit))
						{
							if (hit.transform.gameObject.name == "Ground")
							{
								goodSpot = true;
							}
						}
					}

					foreach (WorldRect rect in excludeRects)
					{
						if (rect.ContainsValueZ (j) && rect.ContainsValueX (i))
						{
							inExclusion = true;
							break;
						}
					}
					
					if ((goodSpot || !checkForSpace) && !inExclusion)
					{
						float dynRand = Random.Range (0f, 1f);
						
						if (dynRand <= dynamicFraction)
						{
							int randomIndex = Random.Range (0, PrefabHolder.acces.dynamicObjects.Count);
							localLevelObjects.Add (new LevelObject (new Vector2 (i, j), PrefabHolder.acces.dynamicObjects[randomIndex].gameObject.name));
						}
						else
						{
							int randomIndex = Random.Range (0, PrefabHolder.acces.staticObjects.Count);
							localLevelObjects.Add (new LevelObject (new Vector2 (i, j), PrefabHolder.acces.staticObjects[randomIndex].gameObject.name));
						}
					}
				}
			}
		}

		SetObjects (localLevelObjects);
	}

	public void GenerateTemplatedLevel (float dynamicFraction)
	{
		List<LevelObject> localLevelObjects = new List<LevelObject> ();
		GameObject[] templateObjects = GameObject.FindGameObjectsWithTag (Tags.startCubes);

		for (int i = 0; i < templateObjects.Length; i++)
		{
			float dynRand = Random.Range (0f, 1f);
			
			if (dynRand <= dynamicFraction)
			{
				int randomIndex = Random.Range (0, PrefabHolder.acces.dynamicObjects.Count);
				localLevelObjects.Add (new LevelObject (new Vector2 (templateObjects[i].transform.position.x, templateObjects[i].transform.position.z), PrefabHolder.acces.dynamicObjects[randomIndex].gameObject.name));
			}
			else
			{
				int randomIndex = Random.Range (0, PrefabHolder.acces.staticObjects.Count);
				localLevelObjects.Add (new LevelObject (new Vector2 (templateObjects[i].transform.position.x, templateObjects[i].transform.position.z), PrefabHolder.acces.staticObjects[randomIndex].gameObject.name));
			}
		}

		SetObjects (localLevelObjects);
	}
	
	// Create and send level
	public void SendLevelData (string username, bool allUsers)
	{
		Debug.LogWarning ("Sending LevelData to " + username);

		if (SwitchBox.isServer)
		{
			List<string> data = SerializerHelper.SerializeToString (levelObjects, 3);
			GetComponent<NetworkView>().RPC ("RecieveLevelData", RPCMode.All, data[0], data[1], data[2], username, allUsers);
		}

		else if (!SwitchBox.isClient)
		{
			CreateLevel ();
		}
	}

	[RPC] private void RecieveLevelData (string data_1, string data_2, string data_3, string username, bool allUsers)
	{
		levelObjects = SerializerHelper.DeserializeFromString<List<LevelObject>> (data_1 + data_2 + data_3);
		CreateLevel ();
	}

	private void CreateLevel ()
	{
		GameObject[] templateWalls = GameObject.FindGameObjectsWithTag (Tags.startCubes);
		for (int i = 0; i < templateWalls.Length; i++)
		{
			Destroy (templateWalls[i].gameObject);
		}
		
		foreach (LevelObject lo in levelObjects)
		{
			bool prefabFound = false;
			Vector3 position = new Vector3 (lo.positionX, 0, lo.positionZ);

			foreach (Prefab prefab in PrefabHolder.acces.staticObjects)
			{
				if (prefab.gameObject.name == lo.objectName)
				{
					prefabFound = true;
					position.y = prefab.spawnHeight;
					Instantiate (prefab.gameObject, position, prefab.gameObject.transform.rotation);
					break;
				}
			}

			if (!prefabFound && SwitchBox.isHost)
			{
				foreach (Prefab prefab in PrefabHolder.acces.dynamicObjects)
				{
					if (prefab.gameObject.name == lo.objectName)
					{
						prefabFound = true;
						position.y = prefab.spawnHeight;
						SwitchBox.Instantiate (prefab.gameObject, position, prefab.gameObject.transform.rotation);
						break;
					}
				}
			}

			if (!prefabFound)
			{
				Debug.LogError ("Prefab for GameObject titled '" + lo.objectName + "' not found!");
			}
		}

		GameController.acces.SendLevelLoaded ();
	}
}

[System.Serializable]
public class LevelObject
{
	public float positionX;
	public float positionZ;
	public string objectName;

	public LevelObject ()
	{}

	public LevelObject (Vector2 position, string objectName)
	{
		this.positionX = position.x;
		this.positionZ = position.y;
		this.objectName = objectName;
	}
}

[System.Serializable]
public class WorldRect
{
	public float left;
	public float right;
	public float top;
	public float bottom;

	public int GetArea ()
	{
		return (int) ((right - left + 1f) * (top - bottom + 1f));
	}

	public bool ContainsValueX (float x)
	{
		return (x <= right && x >= left);
	}

	public bool ContainsValueZ (float z)
	{
		return (z <= top && z >= bottom);
	}

	public WorldRect ()
	{}

	public WorldRect (float left, float right, float bottom, float top)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}
}
