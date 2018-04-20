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
		}
	}

	void MoveSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.moveSpeed, GlobalValues.playerData.purchased_MoveSpeed) + "\n" + "Current Movement Speed: " + "\n" + GlobalValues.properties.moveSpeed;
	}

	void TurnSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.turnSpeed, GlobalValues.playerData.purchased_TurnSpeed) + "\n" + "Current Turning Speed: " + "\n" + GlobalValues.properties.turnSpeed;
	}

	void TurretTurnSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.turretTurnSpeed, GlobalValues.playerData.purchased_TurretTurnSpeed) + "\n" + "Current Turning Speed: " + "\n" + GlobalValues.properties.turretTurnSpeed;
	}

	void AmmoDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.ammo, GlobalValues.playerData.purchased_Ammo) + "\n" + "Current Clip Size: " + "\n" + GlobalValues.properties.ammo;
	}

	void ReloadDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.reload, GlobalValues.playerData.purchased_Reload) + "\n" + "Current Reload Time: " + "\n" + GlobalValues.properties.reload;
	}

	void ShellSpeedDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.shellSpeed, GlobalValues.playerData.purchased_ShellSpeed) + "\n" + "Flux Thruster Power: " + "\n" + GlobalValues.properties.shellSpeed;
	}

	void BouncesDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.shellBounces, GlobalValues.playerData.purchased_Bounces) + "\n" + "Quantum Repulsor Grade: " + "\n" + GlobalValues.properties.shellBounces;
	}

	void MineAmountDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.mineAmount, GlobalValues.playerData.purchased_MineAmount) + "\n" + "Maximum Active Mines: " + "\n" + GlobalValues.properties.mineAmount;
	}
	
	void MineRadiusDescription ()
	{
		gameObject.GetComponent<TypogenicText>().Text = "Next Level Cost: " + UpgradeManagerValues.cost.CalculateCost(UpgradeManagerValues.cost.mineRadius, GlobalValues.playerData.purchased_MineRadius) + "\n" + "Mine Radius: " + "\n" + GlobalValues.properties.mineRadius;
	}
}
