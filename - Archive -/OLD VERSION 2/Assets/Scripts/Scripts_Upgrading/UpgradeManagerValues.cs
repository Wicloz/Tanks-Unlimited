using UnityEngine;
using System.Collections;
using Wicloz.Collections;
using System.Collections.Generic;

public class UpgradeManagerValues : MonoBehaviour
{
	public static UpgradeManagerValues acces;

	public static SimpleDict<string, int> nameAppliedMatrix = new SimpleDict<string, int>();
	public static SimpleDict<string, int> namePurchasedMatrix = new SimpleDict<string, int>();

	public static float fieldMine = 10000;
	public static float speedShell = 12000;
	public static float aimLaser = 6000;

	public static float moveSpeed = 600;
	public static float turnSpeed = 500;
	public static float turretTurnSpeed = 400;

	public static float ammo = 2000;
	public static float reload = 1000;
	public static float shellSpeed = 2000;
	public static float shellBounces = 4000;

	public static float mineAmount = 4000;
	public static float mineRadius = 1000;
	public static float mineDuration = 1000;
	
	void Awake ()
	{
		acces = this;
	}

	void Update ()
	{
		// Applied
		nameAppliedMatrix.Edit ("moveSpeed_level", ProfileManager.acces.player.applied_MoveSpeed);
		nameAppliedMatrix.Edit ("turnSpeed_level", ProfileManager.acces.player.applied_TurnSpeed);
		nameAppliedMatrix.Edit ("turretTurnSpeed_level", ProfileManager.acces.player.applied_TurretTurnSpeed);
		
		nameAppliedMatrix.Edit ("ammo_level", ProfileManager.acces.player.applied_primaryWeapon.clipSize);
		nameAppliedMatrix.Edit ("reload_level", (int) ProfileManager.acces.player.applied_primaryWeapon.reloadTime);
		nameAppliedMatrix.Edit ("shellSpeed_level", (int) ProfileManager.acces.player.applied_primaryWeapon.shellSpeed);
		nameAppliedMatrix.Edit ("bounces_level", ProfileManager.acces.player.applied_primaryWeapon.shellBounces);
		
		nameAppliedMatrix.Edit ("mineAmount_level", ProfileManager.acces.player.applied_secondaryWeapon.mineAmount);
		nameAppliedMatrix.Edit ("mineRadius_level", (int) ProfileManager.acces.player.applied_secondaryWeapon.mineRadius);
		nameAppliedMatrix.Edit ("mineDuration_level", (int) ProfileManager.acces.player.applied_secondaryWeapon.mineDuration);
		
		//Purchased
		namePurchasedMatrix.Edit ("moveSpeed_level", ProfileManager.acces.player.purchased_MoveSpeed);
		namePurchasedMatrix.Edit ("turnSpeed_level", ProfileManager.acces.player.purchased_TurnSpeed);
		namePurchasedMatrix.Edit ("turretTurnSpeed_level", ProfileManager.acces.player.purchased_TurretTurnSpeed);
		
		namePurchasedMatrix.Edit ("ammo_level", ProfileManager.acces.player.purchased_primaryWeapon.clipSize);
		namePurchasedMatrix.Edit ("reload_level", (int) ProfileManager.acces.player.purchased_primaryWeapon.reloadTime);
		namePurchasedMatrix.Edit ("shellSpeed_level", (int) ProfileManager.acces.player.purchased_primaryWeapon.shellSpeed);
		namePurchasedMatrix.Edit ("bounces_level", ProfileManager.acces.player.purchased_primaryWeapon.shellBounces);
		
		namePurchasedMatrix.Edit ("mineAmount_level", ProfileManager.acces.player.purchased_secondaryWeapon.mineAmount);
		namePurchasedMatrix.Edit ("mineRadius_level", (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineRadius);
		namePurchasedMatrix.Edit ("mineDuration_level", (int) ProfileManager.acces.player.purchased_secondaryWeapon.mineDuration);
	}

	public bool RemovePoints (float cost)
	{
		if (ProfileManager.acces.player.points - cost < 0)
		{
			Debug.Log ("Not enough points");
			return false;
		}
		else
		{
			ProfileManager.acces.player.points -= cost;
			return true;
		}
	}

	public float CalculateCost (float initialCost, int currentLevel)
	{
		float cost = initialCost * Mathf.Pow(2, currentLevel);
		return cost;
	}
}
