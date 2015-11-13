using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponUpgradeList : MonoBehaviour
{
	public static List<PrimaryWeaponValues> basePrimaryWeaponsList = new List<PrimaryWeaponValues> ();
	public static List<SecondaryWeaponValues> baseSecondaryWeaponsList = new List<SecondaryWeaponValues> ();

	static WeaponUpgradeList()
	{
		basePrimaryWeaponsList.Add (new PrimaryWeaponValues("standardShell"));

		PrimaryWeaponValues speedShell = new PrimaryWeaponValues("speedShell");
		speedShell.fireDelay = 0.5f;
		basePrimaryWeaponsList.Add (speedShell);

		baseSecondaryWeaponsList.Add (new SecondaryWeaponValues ("proxMine"));
		baseSecondaryWeaponsList.Add (new SecondaryWeaponValues ("fieldMine"));
	}

	public static PrimaryWeaponValues GetPrimaryWeapon (string weaponName)
	{
		foreach (PrimaryWeaponValues weapon in basePrimaryWeaponsList)
		{
			if (weapon.weaponName == weaponName)
			{
				return SerializerHelper.DeserializeFromString<PrimaryWeaponValues>(SerializerHelper.SerializeToString(weapon));
			}
		}
		return null;
	}

	public static SecondaryWeaponValues GetSecondaryWeapon (string weaponName)
	{
		foreach (SecondaryWeaponValues weapon in baseSecondaryWeaponsList)
		{
			if (weapon.weaponName == weaponName)
			{
				return SerializerHelper.DeserializeFromString<SecondaryWeaponValues>(SerializerHelper.SerializeToString(weapon));
			}
		}
		return null;
	}

	public static List<string> PrimaryWeaponNames ()
	{
		List<string> retList = new List<string> ();
		foreach (PrimaryWeaponValues weapon in basePrimaryWeaponsList)
		{
			retList.Add(weapon.weaponName);
		}
		return retList;
	}

	public static List<string> SecondaryWeaponNames ()
	{
		List<string> retList = new List<string> ();
		foreach (SecondaryWeaponValues weapon in baseSecondaryWeaponsList)
		{
			retList.Add(weapon.weaponName);
		}
		return retList;
	}
}

[System.Serializable]
public class UpgradeValues
{
	public bool applied_AimLaser = false;
	public bool purchased_AimLaser = false;
}

[System.Serializable]
public class PrimaryWeaponValues
{
	public string weaponName = "";
	public float fireDelay = 0.2f;
	
	public int clipSize = 0;
	public float reloadTime = 0;
	
	public float shellSpeed = 0;
	public int shellBounces = 0;
	
	public PrimaryWeaponValues ToProperties ()
	{
		PrimaryWeaponValues properties = new PrimaryWeaponValues ();
		
		properties.weaponName = this.weaponName;
		properties.fireDelay = this.fireDelay;
		
		properties.clipSize = 1 + this.clipSize;
		properties.reloadTime = 2 - (this.reloadTime / 5);
		properties.shellSpeed = 5 + this.shellSpeed;
		properties.shellBounces = this.shellBounces;
		
		return properties;
	}
	
	public int TotalValues ()
	{
		return (int) (clipSize + reloadTime + shellSpeed + shellBounces);
	}
	
	public PrimaryWeaponValues ()
	{}
	
	public PrimaryWeaponValues (string name)
	{
		weaponName = name;
	}
}

[System.Serializable]
public class SecondaryWeaponValues
{
	public string weaponName = "";
	public float fireDelay = 0.6f;
	
	public int mineAmount = 0;
	public float mineRadius = 0;
	public float mineDuration = 0;
	
	public SecondaryWeaponValues ToProperties ()
	{
		SecondaryWeaponValues properties = new SecondaryWeaponValues ();
		
		properties.weaponName = this.weaponName;
		properties.fireDelay = this.fireDelay;
		
		properties.mineAmount = 1 + this.mineAmount;
		properties.mineRadius = 0.8f + (this.mineRadius / 10);
		properties.mineDuration = 2 + (this.mineDuration / 2);
		
		return properties;
	}
	
	public int TotalValues ()
	{
		return (int) (mineAmount + mineRadius + mineDuration);
	}
	
	public SecondaryWeaponValues ()
	{}
	
	public SecondaryWeaponValues (string name)
	{
		weaponName = name;
	}
}
