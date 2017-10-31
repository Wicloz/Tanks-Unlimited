using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellHUD : MonoBehaviour
{
	public GameObject shellIconBottom;
	public GameObject shellIconTop;

	List <GameObject> topIconList = new List <GameObject> ();
	List <GameObject> bottomIconList = new List <GameObject> ();

	void Start ()
	{
		for (int i = 0; i < GlobalValues.properties.ammo; i++)
		{
			GameObject newIcon = (GameObject) Instantiate (shellIconBottom, new Vector3 (-8.3f + (i * 0.7f), 15.7f, -150), Quaternion.Euler (0, 180, 0));
			
			bottomIconList.Add (newIcon);
		}

		for (int i = 0; i < GlobalValues.properties.ammo; i++)
		{
			GameObject newIcon = (GameObject) Instantiate (shellIconTop, new Vector3 (-8.3f + (i * 0.7f), 15.7f, -149), Quaternion.Euler (0, 180, 0));
			
			topIconList.Add (newIcon);
		}
	}

	void Update ()
	{
		if (PlayerController.thisTank != null)
		{
//			for (int i = GlobalValues.properties.ammo; i > PlayerController.thisTank.shells; i--)
			for (int i = 0; i < GlobalValues.properties.ammo; i++)
			{
				float reloadness = 0;

				if (i > PlayerController.thisTank.shells)
				{
					reloadness = 0;
				}

				else if (i == PlayerController.thisTank.shells)
				{
					reloadness = PlayerController.thisTank.reloadness;
				}

				else if (i < PlayerController.thisTank.shells)
				{
					reloadness = 1;
				}

				topIconList[i].transform.position = new Vector3 (topIconList[i].transform.position.x, 14.7f + reloadness, topIconList[i].transform.position.z);
			}
		}
	}
}
