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
		StartCoroutine (CreateHud ());
	}

	private IEnumerator CreateHud ()
	{
		yield return null;

		for (int i = 0; i < ProfileManager.acces.playerProperties.primaryWeapon.clipSize; i++)
		{
			GameObject newIcon = (GameObject) Instantiate (shellIconBottom, new Vector3 (-8.3f + (i * 0.7f), 15.7f, -150), Quaternion.Euler (0, 180, 0));
			newIcon.transform.SetParent (gameObject.transform, true);
			bottomIconList.Add (newIcon);
		}

		for (int i = 0; i < ProfileManager.acces.playerProperties.primaryWeapon.clipSize; i++)
		{
			GameObject newIcon = (GameObject) Instantiate (shellIconTop, new Vector3 (-8.3f + (i * 0.7f), 15.7f, -149), Quaternion.Euler (0, 180, 0));
			newIcon.transform.SetParent (gameObject.transform, true);
			topIconList.Add (newIcon);
		}
	}

	void Update ()
	{
		PlayerController playerTank = null;

		if (RespawnManager.acces.thisUser.tank != null)
		{
			playerTank = RespawnManager.acces.thisUser.tank.GetComponent<PlayerController> ();
		}

		if (playerTank != null)
		{
//			for (int i = GlobalValues.properties.ammo; i > PlayerController.thisTank.shells; i--)
			for (int i = 0; i < ProfileManager.acces.playerProperties.primaryWeapon.clipSize; i++)
			{
				float reloadness = 0;

				if (i > playerTank.shellsLeft)
				{
					reloadness = 0;
				}

				else if (i == playerTank.shellsLeft)
				{
					reloadness = playerTank.reloadness;
				}

				else if (i < playerTank.shellsLeft)
				{
					reloadness = 1;
				}

				topIconList[i].transform.position = new Vector3 (topIconList[i].transform.position.x, 14.7f + reloadness, topIconList[i].transform.position.z);
			}
		}
	}
}
