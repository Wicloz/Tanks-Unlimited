using UnityEngine;
using System.Collections;

public class UpgradeManagerRight : MonoBehaviour
{
	public Material activated;
	public Material purchased;
	public Material locked;

	void Update ()
	{
		switch (gameObject.name)
		{
		case "button_standardShell":
			StandardShellGraphics ();
			break;
		case "button_speedShell":
			SpeedShellGraphics ();
			break;

		case "button_proxMine":
			ProxMineGraphics ();
			break;
		case "button_fieldMine":
			FieldMineGraphics ();
			break;

		case "button_aimLaser":
			AimLaserGraphics ();
			break;
		}
	}

	private void StandardShellGraphics ()
	{
		if (ProfileManager.acces.player.selectedPrimary == "standardShell")
		{
			gameObject.GetComponent<MeshRenderer>().material = activated;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equipped";
		}
		else if (ProfileManager.acces.player.purchased_primaryWeapons.Contains ("standardShell"))
		{
			gameObject.GetComponent<MeshRenderer>().material = purchased;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equip";
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = locked;
			transform.GetComponentInChildren<TypogenicText>().Text = "Purchase (0)";
		}
	}

	private void SpeedShellGraphics ()
	{
		if (ProfileManager.acces.player.selectedPrimary == "speedShell")
		{
			gameObject.GetComponent<MeshRenderer>().material = activated;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equipped";
		}
		else if (ProfileManager.acces.player.purchased_primaryWeapons.Contains ("speedShell"))
		{
			gameObject.GetComponent<MeshRenderer>().material = purchased;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equip";
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = locked;
			transform.GetComponentInChildren<TypogenicText>().Text = "Purchase (" + UpgradeManagerValues.speedShell + ")";
		}
	}

	private void ProxMineGraphics ()
	{
		if (ProfileManager.acces.player.selectedSecondary == "proxMine")
		{
			gameObject.GetComponent<MeshRenderer>().material = activated;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equipped";
		}
		else if (ProfileManager.acces.player.purchased_secondaryWeapons.Contains ("proxMine"))
		{
			gameObject.GetComponent<MeshRenderer>().material = purchased;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equip";
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = locked;
			transform.GetComponentInChildren<TypogenicText>().Text = "Purchase (0)";
		}
	}

	private void FieldMineGraphics ()
	{
		if (ProfileManager.acces.player.selectedSecondary == "fieldMine")
		{
			gameObject.GetComponent<MeshRenderer>().material = activated;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equipped";
		}
		else if (ProfileManager.acces.player.purchased_secondaryWeapons.Contains ("fieldMine"))
		{
			gameObject.GetComponent<MeshRenderer>().material = purchased;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equip";
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = locked;
			transform.GetComponentInChildren<TypogenicText>().Text = "Purchase (" + UpgradeManagerValues.fieldMine + ")";
		}
	}

	private void AimLaserGraphics ()
	{
		if (ProfileManager.acces.player.upgrades.applied_AimLaser)
		{
			gameObject.GetComponent<MeshRenderer>().material = activated;
			transform.GetComponentInChildren<TypogenicText>().Text = "Unequip";
		}
		else if (ProfileManager.acces.player.upgrades.purchased_AimLaser)
		{
			gameObject.GetComponent<MeshRenderer>().material = purchased;
			transform.GetComponentInChildren<TypogenicText>().Text = "Equip";
		}
		else
		{
			gameObject.GetComponent<MeshRenderer>().material = locked;
			transform.GetComponentInChildren<TypogenicText>().Text = "Purchase (" + UpgradeManagerValues.aimLaser + ")";
		}
	}

	void OnMouseDown ()
	{
		switch (gameObject.name)
		{
		case "button_standardShell":
			StandardShell ();
			break;
		case "button_speedShell":
			SpeedShell ();
			break;

		case "button_proxMine":
			ProxMine ();
			break;
		case "button_fieldMine":
			FieldMine ();
			break;

		case "button_aimLaser":
			AimLaser ();
			break;
			
		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	private void StandardShell ()
	{
		float cost = 0;
		string name = "standardShell";
		
		if (ProfileManager.acces.player.purchased_primaryWeapons.Contains (name))
		{
			UpgradeManagerScreens.acces.SwitchPrimaryWeapon (name);
		}
		
		else if (UpgradeManagerValues.acces.RemovePoints (cost))
		{
			ProfileManager.acces.player.purchased_primaryWeapons.Add (name);
			UpgradeManagerScreens.acces.SwitchPrimaryWeapon (name);
		}
	}
	
	private void SpeedShell ()
	{
		float cost = UpgradeManagerValues.speedShell;
		string name = "speedShell";
		
		if (ProfileManager.acces.player.purchased_primaryWeapons.Contains (name))
		{
			UpgradeManagerScreens.acces.SwitchPrimaryWeapon (name);
		}
		
		else if (UpgradeManagerValues.acces.RemovePoints (cost))
		{
			ProfileManager.acces.player.purchased_primaryWeapons.Add (name);
			UpgradeManagerScreens.acces.SwitchPrimaryWeapon (name);
		}
	}

	private void ProxMine ()
	{
		float cost = 0;
		string name = "proxMine";

		if (ProfileManager.acces.player.purchased_secondaryWeapons.Contains (name))
		{
			UpgradeManagerScreens.acces.SwitchSecondaryWeapon (name);
		}

		else if (UpgradeManagerValues.acces.RemovePoints (cost))
		{
			ProfileManager.acces.player.purchased_secondaryWeapons.Add (name);
			UpgradeManagerScreens.acces.SwitchSecondaryWeapon (name);
		}
	}

	private void FieldMine ()
	{
		float cost = UpgradeManagerValues.fieldMine;
		string name = "fieldMine";
		
		if (ProfileManager.acces.player.purchased_secondaryWeapons.Contains (name))
		{
			UpgradeManagerScreens.acces.SwitchSecondaryWeapon (name);
		}
		
		else if (UpgradeManagerValues.acces.RemovePoints (cost))
		{
			ProfileManager.acces.player.purchased_secondaryWeapons.Add (name);
			UpgradeManagerScreens.acces.SwitchSecondaryWeapon (name);
		}
	}

	private void AimLaser ()
	{
		float cost = UpgradeManagerValues.aimLaser;
		bool applied = ProfileManager.acces.player.upgrades.applied_AimLaser;
		bool purchased = ProfileManager.acces.player.upgrades.purchased_AimLaser;
		
		if (applied)
		{
			applied = false;
		}

		else if (purchased)
		{
			applied = true;
		}

		else if (UpgradeManagerValues.acces.RemovePoints (cost))
		{
			purchased = true;
			applied = true;
		}

		ProfileManager.acces.player.upgrades.applied_AimLaser = applied;
		ProfileManager.acces.player.upgrades.purchased_AimLaser = purchased;
	}
}
