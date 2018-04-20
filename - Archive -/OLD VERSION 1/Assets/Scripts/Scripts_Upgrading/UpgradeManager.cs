using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
	public static UpgradeManager acces;

	public Material activated;
	public Material purchased;
	public Material locked;

	void Awake ()
	{
		acces = this;
	}

	void Update ()
	{
		MoveSpeedGraphics ();
		TurnSpeedGraphics ();
		TurretTurnSpeedGraphics ();
		AmmoGraphics ();
		ReloadGraphics ();
		ShellSpeedGraphics ();
		BouncesGraphics ();
		MineAmountGraphics ();
		MineRadiusGraphics ();
	}

	void MoveSpeedGraphics ()
	{
		switch (transform.name)
		{
		case "moveSpeed_level1":
			if (GlobalValues.playerData.applied_MoveSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level2":
			if (GlobalValues.playerData.applied_MoveSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level3":
			if (GlobalValues.playerData.applied_MoveSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level4":
			if (GlobalValues.playerData.applied_MoveSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level5":
			if (GlobalValues.playerData.applied_MoveSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level6":
			if (GlobalValues.playerData.applied_MoveSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level7":
			if (GlobalValues.playerData.applied_MoveSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level8":
			if (GlobalValues.playerData.applied_MoveSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level9":
			if (GlobalValues.playerData.applied_MoveSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "moveSpeed_level10":
			if (GlobalValues.playerData.applied_MoveSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MoveSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void TurnSpeedGraphics ()
	{
		switch (transform.name)
		{
		case "turnSpeed_level1":
			if (GlobalValues.playerData.applied_TurnSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level2":
			if (GlobalValues.playerData.applied_TurnSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level3":
			if (GlobalValues.playerData.applied_TurnSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level4":
			if (GlobalValues.playerData.applied_TurnSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level5":
			if (GlobalValues.playerData.applied_TurnSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level6":
			if (GlobalValues.playerData.applied_TurnSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level7":
			if (GlobalValues.playerData.applied_TurnSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level8":
			if (GlobalValues.playerData.applied_TurnSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level9":
			if (GlobalValues.playerData.applied_TurnSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turnSpeed_level10":
			if (GlobalValues.playerData.applied_TurnSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurnSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void TurretTurnSpeedGraphics ()
	{
		switch (transform.name)
		{
		case "turretTurnSpeed_level1":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level2":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level3":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level4":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level5":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level6":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 6)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level7":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 7)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level8":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 8)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level9":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 9)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "turretTurnSpeed_level10":
			if (GlobalValues.playerData.applied_TurretTurnSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_TurretTurnSpeed >= 10)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void AmmoGraphics ()
	{
		switch (transform.name)
		{
		case "ammo_level1":
			if (GlobalValues.playerData.applied_Ammo >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Ammo >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "ammo_level2":
			if (GlobalValues.playerData.applied_Ammo >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Ammo >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "ammo_level3":
			if (GlobalValues.playerData.applied_Ammo >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Ammo >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "ammo_level4":
			if (GlobalValues.playerData.applied_Ammo >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Ammo >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "ammo_level5":
			if (GlobalValues.playerData.applied_Ammo >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Ammo >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void ReloadGraphics ()
	{
		switch (transform.name)
		{
		case "reload_level1":
			if (GlobalValues.playerData.applied_Reload >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Reload >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "reload_level2":
			if (GlobalValues.playerData.applied_Reload >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Reload >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "reload_level3":
			if (GlobalValues.playerData.applied_Reload >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Reload >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "reload_level4":
			if (GlobalValues.playerData.applied_Reload >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Reload >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "reload_level5":
			if (GlobalValues.playerData.applied_Reload >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Reload >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void ShellSpeedGraphics ()
	{
		switch (transform.name)
		{
		case "shellSpeed_level1":
			if (GlobalValues.playerData.applied_ShellSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_ShellSpeed >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "shellSpeed_level2":
			if (GlobalValues.playerData.applied_ShellSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_ShellSpeed >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "shellSpeed_level3":
			if (GlobalValues.playerData.applied_ShellSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_ShellSpeed >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "shellSpeed_level4":
			if (GlobalValues.playerData.applied_ShellSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_ShellSpeed >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "shellSpeed_level5":
			if (GlobalValues.playerData.applied_ShellSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_ShellSpeed >= 5)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void BouncesGraphics ()
	{
		switch (transform.name)
		{
		case "bounces_level1":
			if (GlobalValues.playerData.applied_Bounces >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Bounces >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "bounces_level2":
			if (GlobalValues.playerData.applied_Bounces >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Bounces >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "bounces_level3":
			if (GlobalValues.playerData.applied_Bounces >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_Bounces >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void MineAmountGraphics ()
	{
		switch (transform.name)
		{
		case "mineAmount_level1":
			if (GlobalValues.playerData.applied_MineAmount >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineAmount >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineAmount_level2":
			if (GlobalValues.playerData.applied_MineAmount >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineAmount >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineAmount_level3":
			if (GlobalValues.playerData.applied_MineAmount >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineAmount >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineAmount_level4":
			if (GlobalValues.playerData.applied_MineAmount >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineAmount >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}
	
	void MineRadiusGraphics ()
	{
		switch (transform.name)
		{
		case "mineRadius_level1":
			if (GlobalValues.playerData.applied_MineRadius >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineRadius >= 1)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineRadius_level2":
			if (GlobalValues.playerData.applied_MineRadius >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineRadius >= 2)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineRadius_level3":
			if (GlobalValues.playerData.applied_MineRadius >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineRadius >= 3)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		case "mineRadius_level4":
			if (GlobalValues.playerData.applied_MineRadius >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else if (GlobalValues.playerData.purchased_MineRadius >= 4)
			{
				gameObject.GetComponent <MeshRenderer>().material = purchased;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = locked;
			}
			break;
		}
	}

	void OnMouseDown ()
	{
		switch (transform.name)
		{
		case "moveSpeed_level0":
			MoveSpeed (0);
			break;
		case "moveSpeed_level1":
			MoveSpeed (1);
			break;
		case "moveSpeed_level2":
			MoveSpeed (2);
			break;
		case "moveSpeed_level3":
			MoveSpeed (3);
			break;
		case "moveSpeed_level4":
			MoveSpeed (4);
			break;
		case "moveSpeed_level5":
			MoveSpeed (5);
			break;
		case "moveSpeed_level6":
			MoveSpeed (6);
			break;
		case "moveSpeed_level7":
			MoveSpeed (7);
			break;
		case "moveSpeed_level8":
			MoveSpeed (8);
			break;
		case "moveSpeed_level9":
			MoveSpeed (9);
			break;
		case "moveSpeed_level10":
			MoveSpeed (10);
			break;

		case "turnSpeed_level0":
			TurnSpeed (0);
			break;
		case "turnSpeed_level1":
			TurnSpeed (1);
			break;
		case "turnSpeed_level2":
			TurnSpeed (2);
			break;
		case "turnSpeed_level3":
			TurnSpeed (3);
			break;
		case "turnSpeed_level4":
			TurnSpeed (4);
			break;
		case "turnSpeed_level5":
			TurnSpeed (5);
			break;
		case "turnSpeed_level6":
			TurnSpeed (6);
			break;
		case "turnSpeed_level7":
			TurnSpeed (7);
			break;
		case "turnSpeed_level8":
			TurnSpeed (8);
			break;
		case "turnSpeed_level9":
			TurnSpeed (9);
			break;
		case "turnSpeed_level10":
			TurnSpeed (10);
			break;

		case "turretTurnSpeed_level0":
			TurretTurnSpeed (0);
			break;
		case "turretTurnSpeed_level1":
			TurretTurnSpeed (1);
			break;
		case "turretTurnSpeed_level2":
			TurretTurnSpeed (2);
			break;
		case "turretTurnSpeed_level3":
			TurretTurnSpeed (3);
			break;
		case "turretTurnSpeed_level4":
			TurretTurnSpeed (4);
			break;
		case "turretTurnSpeed_level5":
			TurretTurnSpeed (5);
			break;
		case "turretTurnSpeed_level6":
			TurretTurnSpeed (6);
			break;
		case "turretTurnSpeed_level7":
			TurretTurnSpeed (7);
			break;
		case "turretTurnSpeed_level8":
			TurretTurnSpeed (8);
			break;
		case "turretTurnSpeed_level9":
			TurretTurnSpeed (9);
			break;
		case "turretTurnSpeed_level10":
			TurretTurnSpeed (10);
			break;

		case "ammo_level0":
			Ammo (0);
			break;
		case "ammo_level1":
			Ammo (1);
			break;
		case "ammo_level2":
			Ammo (2);
			break;
		case "ammo_level3":
			Ammo (3);
			break;
		case "ammo_level4":
			Ammo (4);
			break;
		case "ammo_level5":
			Ammo (5);
			break;

		case "reload_level0":
			Reload (0);
			break;
		case "reload_level1":
			Reload (1);
			break;
		case "reload_level2":
			Reload (2);
			break;
		case "reload_level3":
			Reload (3);
			break;
		case "reload_level4":
			Reload (4);
			break;
		case "reload_level5":
			Reload (5);
			break;

		case "shellSpeed_level0":
			ShellSpeed (0);
			break;
		case "shellSpeed_level1":
			ShellSpeed (1);
			break;
		case "shellSpeed_level2":
			ShellSpeed (2);
			break;
		case "shellSpeed_level3":
			ShellSpeed (3);
			break;
		case "shellSpeed_level4":
			ShellSpeed (4);
			break;
		case "shellSpeed_level5":
			ShellSpeed (5);
			break;

		case "bounces_level0":
			Bounces (0);
			break;
		case "bounces_level1":
			Bounces (1);
			break;
		case "bounces_level2":
			Bounces (2);
			break;
		case "bounces_level3":
			Bounces (3);
			break;

		case "mineAmount_level0":
			MineAmount (0);
			break;
		case "mineAmount_level1":
			MineAmount (1);
			break;
		case "mineAmount_level2":
			MineAmount (2);
			break;
		case "mineAmount_level3":
			MineAmount (3);
			break;
		case "mineAmount_level4":
			MineAmount (4);
			break;
			
		case "mineRadius_level0":
			MineRadius (0);
			break;
		case "mineRadius_level1":
			MineRadius (1);
			break;
		case "mineRadius_level2":
			MineRadius (2);
			break;
		case "mineRadius_level3":
			MineRadius (3);
			break;
		case "mineRadius_level4":
			MineRadius (4);
			break;

		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	void MoveSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.cost.moveSpeed;
		int purchased = GlobalValues.playerData.purchased_MoveSpeed;
		int applied = GlobalValues.playerData.applied_MoveSpeed;

		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}

		if (level <= purchased)
		{
			applied = level;
		}

		GlobalValues.playerData.purchased_MoveSpeed = purchased;
		GlobalValues.playerData.applied_MoveSpeed = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void TurnSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.cost.turnSpeed;
		int purchased = GlobalValues.playerData.purchased_TurnSpeed;
		int applied = GlobalValues.playerData.applied_TurnSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_TurnSpeed = purchased;
		GlobalValues.playerData.applied_TurnSpeed = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void TurretTurnSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.cost.turretTurnSpeed;
		int purchased = GlobalValues.playerData.purchased_TurretTurnSpeed;
		int applied = GlobalValues.playerData.applied_TurretTurnSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_TurretTurnSpeed = purchased;
		GlobalValues.playerData.applied_TurretTurnSpeed = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void Ammo (int level)
	{
		float initialCost = UpgradeManagerValues.cost.ammo;
		int purchased = GlobalValues.playerData.purchased_Ammo;
		int applied = GlobalValues.playerData.applied_Ammo;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_Ammo = purchased;
		GlobalValues.playerData.applied_Ammo = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void Reload (int level)
	{
		float initialCost = UpgradeManagerValues.cost.reload;
		int purchased = GlobalValues.playerData.purchased_Reload;
		int applied = GlobalValues.playerData.applied_Reload;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_Reload = purchased;
		GlobalValues.playerData.applied_Reload = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void ShellSpeed (int level)
	{
		float initialCost = UpgradeManagerValues.cost.shellSpeed;
		int purchased = GlobalValues.playerData.purchased_ShellSpeed;
		int applied = GlobalValues.playerData.applied_ShellSpeed;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_ShellSpeed = purchased;
		GlobalValues.playerData.applied_ShellSpeed = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void Bounces (int level)
	{
		float initialCost = UpgradeManagerValues.cost.shellBounces;
		int purchased = GlobalValues.playerData.purchased_Bounces;
		int applied = GlobalValues.playerData.applied_Bounces;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_Bounces = purchased;
		GlobalValues.playerData.applied_Bounces = applied;
		GlobalValues.acces.SetTankProperties ();
	}

	void MineAmount (int level)
	{
		float initialCost = UpgradeManagerValues.cost.mineAmount;
		int purchased = GlobalValues.playerData.purchased_MineAmount;
		int applied = GlobalValues.playerData.applied_MineAmount;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_MineAmount = purchased;
		GlobalValues.playerData.applied_MineAmount = applied;
		GlobalValues.acces.SetTankProperties ();
	}
	
	void MineRadius (int level)
	{
		float initialCost = UpgradeManagerValues.cost.mineRadius;
		int purchased = GlobalValues.playerData.purchased_MineRadius;
		int applied = GlobalValues.playerData.applied_MineRadius;
		
		if (level > purchased)
		{
			if (UpgradeManagerValues.cost.RemovePoints (UpgradeManagerValues.cost.CalculateCost(initialCost, purchased)))
			{
				purchased++;
				applied = purchased;
			}
		}
		
		if (level <= purchased)
		{
			applied = level;
		}
		
		GlobalValues.playerData.purchased_MineRadius = purchased;
		GlobalValues.playerData.applied_MineRadius = applied;
		GlobalValues.acces.SetTankProperties ();
	}
}
