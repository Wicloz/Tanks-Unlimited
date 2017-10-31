using UnityEngine;
using System.Collections;

public class UpgradeManagerDescriptions : MonoBehaviour
{
	void Update ()
	{
		switch (transform.parent.gameObject.name)
		{
		case "upgade_moveSpeed":
			MoveSpeedDescription ();
			break;
		case "upgade_turnSpeed":
			TurnSpeedDescription ();
			break;

		case "upgade_turretTurnSpeed":
			TurretTurnSpeedDescription ();
			break;
		case "upgade_ammo":
			AmmoDescription ();
			break;
		case "upgade_reloadSpeed":
			ReloadDescription ();
			break;

		case "upgade_shellSpeed":
			ShellSpeedDescription ();
			break;
		case "upgade_bounces":
			BouncesDescription ();
			break;

		case "upgade_mineAmount":
			MineAmountDescription ();
			break;
		case "upgade_mineRadius":
			MineRadiusDescription ();
			break;
		case "upgade_mineDuration":
			MineDurationDescription ();
			break;
		}
	}

	void MoveSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.moveSpeed, ProfileManager.acces.player.purchased_MoveSpeed) + "\n" + "Current Movement Speed: " + "\n" + ProfileManager.acces.playerProperties.moveSpeed + "";
	}

	void TurnSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.turnSpeed, ProfileManager.acces.player.purchased_TurnSpeed) + "\n" + "Current Turning Speed: " + "\n" + ProfileManager.acces.playerProperties.turnSpeed + "";
	}

	void TurretTurnSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.turretTurnSpeed, ProfileManager.acces.player.purchased_TurretTurnSpeed) + "\n" + "Current Rotation Speed: " + "\n" + ProfileManager.acces.playerProperties.turretTurnSpeed + "";
	}

	void AmmoDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.ammo, ProfileManager.acces.player.purchased_primaryWeapon.clipSize) + "\n" + "Current Clip Size: " + "\n" + ProfileManager.acces.playerProperties.primaryWeapon.clipSize + " shells";
	}

	void ReloadDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.reload, (int) ProfileManager.acces.player.purchased_primaryWeapon.reloadTime) + "\n" + "Current Reload Time: " + "\n" + ProfileManager.acces.playerProperties.primaryWeapon.reloadTime + " seconds";
	}

	void ShellSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.shellSpeed, (int) ProfileManager.acces.player.purchased_primaryWeapon.shellSpeed) + "\n" + "Flux Thruster Power: " + "\n" + ProfileManager.acces.playerProperties.primaryWeapon.shellSpeed + "";
	}

	void BouncesDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.shellBounces, ProfileManager.acces.player.purchased_primaryWeapon.shellBounces) + "\n" + "Quantum Repulsor Grade: " + "\n" + ProfileManager.acces.playerProperties.primaryWeapon.shellBounces + " bounces";
	}

	void MineAmountDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.mineAmount, ProfileManager.acces.player.purchased_secondaryWeapon.mineAmount) + "\n" + "Maximum Active Mines: " + "\n" + ProfileManager.acces.playerProperties.secondaryWeapon.mineAmount + " mines";
	}
	
	void MineRadiusDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.mineRadius, (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineRadius) + "\n" + "Mine Radius: " + "\n" + ProfileManager.acces.playerProperties.secondaryWeapon.mineRadius + " units";
	}

	void MineDurationDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.acces.CalculateCost(UpgradeManagerValues.mineDuration, (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineDuration) + "\n" + "Mine Duration: " + "\n" + ProfileManager.acces.playerProperties.secondaryWeapon.mineDuration + " seconds";
	}
}
