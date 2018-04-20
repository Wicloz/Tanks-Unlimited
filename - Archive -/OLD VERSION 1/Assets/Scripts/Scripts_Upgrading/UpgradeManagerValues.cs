using UnityEngine;
using System.Collections;

public class UpgradeManagerValues : MonoBehaviour
{
	public static UpgradeManagerValues cost;
	
	public float moveSpeed;
	public float turnSpeed;
	public float turretTurnSpeed;
	public float ammo;
	public float reload;
	public float shellSpeed;
	public float shellBounces;
	public float mineAmount;
	public float mineRadius;
	
	void Awake ()
	{
		cost = this;
	}
	
	void Start ()
	{
		moveSpeed = 600;
		turnSpeed = 500;
		
		turretTurnSpeed = 400;
		ammo = 2000;
		reload = 1000;
		
		shellSpeed = 2000;
		shellBounces = 4000;
		
		mineAmount = 4000;
		mineRadius = 1000;
	}

	public bool RemovePoints (float cost)
	{
		if (GlobalValues.acces.totalScore - cost < 0)
		{
			Debug.Log ("Not enough points");
			return false;
		}
		else
		{
			GlobalValues.acces.totalScore -= cost;
			return true;
		}
	}

	public float CalculateCost (float initialCost, int currentLevel)
	{
		float cost = initialCost * Mathf.Pow(2, currentLevel);
		return cost;
	}
}
