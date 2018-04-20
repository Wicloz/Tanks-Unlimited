using UnityEngine;
using System.Collections;

public class UpgradeManagerScreens : MonoBehaviour
{
	public static UpgradeManagerScreens acces;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		SwitchPrimaryWeapon (ProfileManager.acces.player.selectedPrimary);
		SwitchSecondaryWeapon (ProfileManager.acces.player.selectedSecondary);
	}

	// Left tabs
	public GameObject body;
	public GameObject primary1;
	public GameObject primary2;
	public GameObject secondary;

	// Child tabs
	public GameObject primary1A;
	public GameObject primary2A;
	public GameObject primary1B;
	public GameObject primary2B;

	public GameObject secondaryA;
	public GameObject secondaryB;

	// Right tabs
	public GameObject generalUpgrades;
	public GameObject primaryUpgrades;
	public GameObject secondaryUpgrades;

	public void SwitchPrimaryWeapon (string newName)
	{
		ProfileManager.acces.player.selectedPrimary = newName;

		primary1A.SetActive (false);
		primary2A.SetActive (false);
		primary1B.SetActive (false);
		primary2B.SetActive (false);

		if (newName == WeaponUpgradeList.PrimaryWeaponNames()[0])
		{
			primary1A.SetActive (true);
			primary2A.SetActive (true);
		}

		else if (newName == WeaponUpgradeList.PrimaryWeaponNames()[1])
		{
			primary1B.SetActive (true);
			primary2B.SetActive (true);
		}

		ProfileManager.acces.CalculatePlayerTankProperties ();
	}
	
	public void SwitchSecondaryWeapon (string newName)
	{
		ProfileManager.acces.player.selectedSecondary = newName;

		secondaryA.SetActive (false);
		secondaryB.SetActive (false);

		if (newName == WeaponUpgradeList.SecondaryWeaponNames()[0])
		{
			secondaryA.SetActive (true);
		}

		else if (newName == WeaponUpgradeList.SecondaryWeaponNames()[1])
		{
			secondaryB.SetActive (true);
		}

		ProfileManager.acces.CalculatePlayerTankProperties ();
	}
}
