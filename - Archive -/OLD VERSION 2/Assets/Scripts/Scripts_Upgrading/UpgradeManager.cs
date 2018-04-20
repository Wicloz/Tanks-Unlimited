using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
	public Material activated;
	public Material purchased;
	public Material locked;

	void Update ()
	{
		string upName = gameObject.name.Replace("0","").Replace("1","").Replace("2","").Replace("3","").Replace("4","").Replace("5","").Replace("6","").Replace("7","").Replace("8","").Replace("9","");
		
		if (upName.Contains ("_level"))
		{
			int levelValue = System.Convert.ToInt32 (gameObject.name.Replace (upName, ""));
			
			if (UpgradeManagerValues.nameAppliedMatrix.Acces(upName) >= levelValue)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (UpgradeManagerValues.namePurchasedMatrix.Acces(upName) >= levelValue)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
		}
	}

	void OnMouseDown ()
	{
		string buttonName = gameObject.name.Replace("0","").Replace("1","").Replace("2","").Replace("3","").Replace("4","").Replace("5","").Replace("6","").Replace("7","").Replace("8","").Replace("9","");
		int levelValue = System.Convert.ToInt32 (gameObject.name.Replace (buttonName, ""));

		switch (buttonName)
		{
		case "moveSpeed_level":
			MoveSpeed (levelValue);
			break;
		case "turnSpeed_level":
			TurnSpeed (levelValue);
			break;
		case "turretTurnSpeed_level":
			TurretTurnSpeed (levelValue);
			break;

		case "ammo_level":
			Ammo (levelValue);
			break;
		case "reload_level":
			Reload (levelValue);
			break;
		case "shellSpeed_level":
			ShellSpeed (levelValue);
			break;
		case "bounces_level":
			ShellBounces (levelValue);
			break;

		case "mineAmount_level":
			MineAmount (levelValue);
			break;
		case "mineRadius_level":
			MineRadius (levelValue);
			break;
		case "mineDuration_level":
			MineDuration (levelValue);
			break;

		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	void MoveSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.moveSpeed;
		int purchased = ProfileManager.acces.player.purchased_MoveSpeed;
		int applied = ProfileManager.acces.player.applied_MoveSpeed;

		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}

		if (level <= purchased)
		{
			applied = level;
		}

		ProfileManager.acces.player.purchased_MoveSpeed = purchased;
		ProfileManager.acces.player.applied_MoveSpeed = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void TurnSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.turnSpeed;
		int purchased = ProfileManager.acces.player.purchased_TurnSpeed;
		int applied = ProfileManager.acces.player.applied_TurnSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_TurnSpeed = purchased;
		ProfileManager.acces.player.applied_TurnSpeed = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void TurretTurnSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.turretTurnSpeed;
		int purchased = ProfileManager.acces.player.purchased_TurretTurnSpeed;
		int applied = ProfileManager.acces.player.applied_TurretTurnSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_TurretTurnSpeed = purchased;
		ProfileManager.acces.player.applied_TurretTurnSpeed = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void Ammo (int level)
	{
		float initialCost = UpgradeManagerValues.ammo;
		int purchased = ProfileManager.acces.player.purchased_primaryWeapon.clipSize;
		int applied = ProfileManager.acces.player.applied_primaryWeapon.clipSize;

		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_primaryWeapon.clipSize = purchased;
		ProfileManager.acces.player.applied_primaryWeapon.clipSize = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void Reload (int level)
	{
		float initialCost = UpgradeManagerValues.reload;
		int purchased = (int) ProfileManager.acces.player.purchased_primaryWeapon.reloadTime;
		int applied = (int) ProfileManager.acces.player.applied_primaryWeapon.reloadTime;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_primaryWeapon.reloadTime = purchased;
		ProfileManager.acces.player.applied_primaryWeapon.reloadTime = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void ShellSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.shellSpeed;
		int purchased = (int) ProfileManager.acces.player.purchased_primaryWeapon.shellSpeed;
		int applied = (int) ProfileManager.acces.player.applied_primaryWeapon.shellSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_primaryWeapon.shellSpeed = purchased;
		ProfileManager.acces.player.applied_primaryWeapon.shellSpeed = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void ShellBounces (int level)
	{
		float initialCost = UpgradeManagerValues.shellBounces;
		int purchased = ProfileManager.acces.player.purchased_primaryWeapon.shellBounces;
		int applied = ProfileManager.acces.player.applied_primaryWeapon.shellBounces;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_primaryWeapon.shellBounces = purchased;
		ProfileManager.acces.player.applied_primaryWeapon.shellBounces = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void MineAmount (int level)
	{
		float initialCost = UpgradeManagerValues.mineAmount;
		int purchased = ProfileManager.acces.player.purchased_secondaryWeapon.mineAmount;
		int applied = ProfileManager.acces.player.applied_secondaryWeapon.mineAmount;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_secondaryWeapon.mineAmount = purchased;
		ProfileManager.acces.player.applied_secondaryWeapon.mineAmount = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}
	
	void MineRadius (int level)
	{
		float initialCost = UpgradeManagerValues.mineRadius;
		int purchased = (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineRadius;
		int applied = (int) ProfileManager.acces.player.applied_secondaryWeapon.mineRadius;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_secondaryWeapon.mineRadius = purchased;
		ProfileManager.acces.player.applied_secondaryWeapon.mineRadius = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}

	void MineDuration (int level)
	{
		float initialCost = UpgradeManagerValues.mineDuration;
		int purchased = (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineDuration;
		int applied = (int) ProfileManager.acces.player.applied_secondaryWeapon.mineDuration;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.acces.RemovePoints (UpgradeManagerValues.acces.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		ProfileManager.acces.player.purchased_secondaryWeapon.mineDuration = purchased;
		ProfileManager.acces.player.applied_secondaryWeapon.mineDuration = applied;
		ProfileManager.acces.CalculatePlayerTankProperties ();
	}
}
