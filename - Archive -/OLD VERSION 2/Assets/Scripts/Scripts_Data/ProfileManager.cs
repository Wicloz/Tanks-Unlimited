using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ProfileManager : PersistentSingleton<ProfileManager>
{
	public ProfileInfo player = new ProfileInfo();
	public TankProperties playerProperties = new TankProperties();

	public string fileFolder
	{
		get
		{
			if (OptionsManager.acces.saveProfileLocal)
			{
				return Application.dataPath + "\\..\\Saves\\";
			}
			else
			{
				return Application.persistentDataPath + "\\";
			}
		}
	}

	// Camera position saving
	public Vector3 firstPersonCameraPosition;
	public Quaternion firstPersonCameraRotation;

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public void CalculatePlayerTankProperties ()
	{
		if (player.applied_secondaryWeapon.weaponName == "fieldMine")
		{
			player.applied_secondaryWeapon.fireDelay = 4 + player.applied_secondaryWeapon.mineDuration;
		}
		
		playerProperties = player.CreatePlayerConfig().ToTankProperties();
	}

	public void LoadProfile (string username)
	{
		string fileLocation = fileFolder + username.Replace(" ", "") + ".dat";

		if (File.Exists(fileLocation))
		{
			player = SerializerHelper.LoadFileBf<ProfileInfo> (fileLocation);
		}
		else
		{
			player = new ProfileInfo (username);
		}

		if (player == null || player.username != username.Replace(" ", ""))
		{
			player = new ProfileInfo (username);
		}

		player.Initialise ();
		SaveProfile ();

		UpgradeManagerScreens.acces.SwitchPrimaryWeapon (player.selectedPrimary);
		UpgradeManagerScreens.acces.SwitchSecondaryWeapon (player.selectedSecondary);
		CalculatePlayerTankProperties ();
	}

	public void SaveProfile ()
	{
		string fileLocation = fileFolder + player.username.Replace(" ", "") + ".dat";
		SerializerHelper.SaveFileBf (player, fileLocation);
		Debug.Log ("Profile Saved");
	}
}

[System.Serializable]
public class ProfileInfo
{
	public string username = "";
	public float points = 0;
	public bool firstPerson = false;

	public int applied_MoveSpeed = 0;
	public int purchased_MoveSpeed = 0;
	public int applied_TurnSpeed = 0;
	public int purchased_TurnSpeed = 0;
	public int applied_TurretTurnSpeed = 0;
	public int purchased_TurretTurnSpeed = 0;

	public List<PrimaryWeaponValues> applied_PrimaryValues = new List<PrimaryWeaponValues> ();
	public List<PrimaryWeaponValues> purchased_PrimaryValues = new List<PrimaryWeaponValues> ();
	public List<SecondaryWeaponValues> applied_SecondaryValues = new List<SecondaryWeaponValues> ();
	public List<SecondaryWeaponValues> purchased_SecondaryValues = new List<SecondaryWeaponValues> ();

	public string selectedPrimary = "";
	public string selectedSecondary = "";
	public List<string> purchased_primaryWeapons = new List<string> ();
	public List<string> purchased_secondaryWeapons = new List<string> ();

	public UpgradeValues upgrades = new UpgradeValues();
	
	public PrimaryWeaponValues applied_primaryWeapon
	{
		get
		{
			foreach (PrimaryWeaponValues values in applied_PrimaryValues)
			{
				if (values.weaponName == this.selectedPrimary)
				{
					return values;
				}
			}
			return null;
		}
	}
	public PrimaryWeaponValues purchased_primaryWeapon
	{
		get
		{
			foreach (PrimaryWeaponValues values in purchased_PrimaryValues)
			{
				if (values.weaponName == this.selectedPrimary)
				{
					return values;
				}
			}
			return null;
		}
	}
	public SecondaryWeaponValues applied_secondaryWeapon
	{
		get
		{
			foreach (SecondaryWeaponValues values in applied_SecondaryValues)
			{
				if (values.weaponName == this.selectedSecondary)
				{
					return values;
				}
			}
			return null;
		}
	}
	public SecondaryWeaponValues purchased_secondaryWeapon
	{
		get
		{
			foreach (SecondaryWeaponValues values in purchased_SecondaryValues)
			{
				if (values.weaponName == this.selectedSecondary)
				{
					return values;
				}
			}
			return null;
		}
	}
	
	public TankConfigValues CreatePlayerConfig ()
	{
		TankConfigValues playerConfig = new TankConfigValues ();
		
		playerConfig.moveSpeed = applied_MoveSpeed;
		playerConfig.turnSpeed = applied_TurnSpeed;
		playerConfig.turretTurnSpeed = applied_TurretTurnSpeed;

		playerConfig.primaryWeapon = applied_primaryWeapon;
		playerConfig.secondaryWeapon = applied_secondaryWeapon;

		playerConfig.upgrade_aimLaser = upgrades.applied_AimLaser;
		
		return playerConfig;
	}

	public void Initialise ()
	{
		for (int i = 0; i < applied_PrimaryValues.Count; i++)
		{
			if (!WeaponUpgradeList.PrimaryWeaponNames().Contains (applied_PrimaryValues[i].weaponName))
			{
				applied_PrimaryValues.RemoveAt (i);
				i--;
			}
		}
		for (int i = 0; i < purchased_PrimaryValues.Count; i++)
		{
			if (!WeaponUpgradeList.PrimaryWeaponNames().Contains (purchased_PrimaryValues[i].weaponName))
			{
				purchased_PrimaryValues.RemoveAt (i);
				i--;
			}
		}
		
		for (int i = 0; i < applied_SecondaryValues.Count; i++)
		{
			if (!WeaponUpgradeList.SecondaryWeaponNames().Contains (applied_SecondaryValues[i].weaponName))
			{
				applied_SecondaryValues.RemoveAt (i);
				i--;
			}
		}
		for (int i = 0; i < purchased_SecondaryValues.Count; i++)
		{
			if (!WeaponUpgradeList.SecondaryWeaponNames().Contains (purchased_SecondaryValues[i].weaponName))
			{
				purchased_SecondaryValues.RemoveAt (i);
				i--;
			}
		}

		foreach (PrimaryWeaponValues baseValues in WeaponUpgradeList.basePrimaryWeaponsList)
		{
			bool exists = false;
			foreach (PrimaryWeaponValues values in applied_PrimaryValues)
			{
				if (values.weaponName == baseValues.weaponName)
				{
					exists = true;
					break;
				}
			}
			
			if (!exists)
			{
				applied_PrimaryValues.Add (baseValues);
				purchased_PrimaryValues.Add (baseValues);
			}
		}

		foreach (SecondaryWeaponValues baseValues in WeaponUpgradeList.baseSecondaryWeaponsList)
		{
			bool exists = false;
			foreach (SecondaryWeaponValues values in applied_SecondaryValues)
			{
				if (values.weaponName == baseValues.weaponName)
				{
					exists = true;
					break;
				}
			}
			
			if (!exists)
			{
				applied_SecondaryValues.Add (baseValues);
				purchased_SecondaryValues.Add (baseValues);
			}
		}

		if (!WeaponUpgradeList.PrimaryWeaponNames().Contains (selectedPrimary))
		{
			selectedPrimary = WeaponUpgradeList.PrimaryWeaponNames()[0];
		}
		if (!WeaponUpgradeList.SecondaryWeaponNames().Contains (selectedSecondary))
		{
			selectedSecondary = WeaponUpgradeList.SecondaryWeaponNames()[0];
		}

		if (!purchased_primaryWeapons.Contains (WeaponUpgradeList.PrimaryWeaponNames()[0]))
		{
			purchased_primaryWeapons.Add (WeaponUpgradeList.PrimaryWeaponNames()[0]);
		}
		if (!purchased_secondaryWeapons.Contains (WeaponUpgradeList.SecondaryWeaponNames()[0]))
		{
			purchased_secondaryWeapons.Add (WeaponUpgradeList.SecondaryWeaponNames()[0]);
		}
	}

	public ProfileInfo ()
	{}

	public ProfileInfo (string username)
	{
		this.username = username;
	}
}
