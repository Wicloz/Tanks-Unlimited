using UnityEngine;
using System.Collections;

public class PlayerController : TankController
{
	public static PlayerController thisTank;

	private bool upgrade_aimLine;

	private GameObject targetReticule;
	private LineRenderer aimLine;
	private GameObject aimLineSpawn;

	private GameObject upgradeObject_aimLine;

	private float newYRotation = 0;

	void Awake ()
	{
		thisTank = this;
	}

	void Start ()
	{
		if (GlobalValues.acces.multiplayer && !networkView.isMine)
		{
			Destroy (gameObject.GetComponent <PlayerController>());
		}

		if (!SwitchBox.isClient)
		{
			targetReticule = GameObject.Find ("reticule_1(Clone)");
		}
		else
		{
			targetReticule = GameObject.Find ("reticule_2(Clone)");
		}

		aimLineSpawn = transform.Find ("TurretPivoter/Gturret/part_aimLaser/part_laserSpawn").gameObject;
		aimLine = aimLineSpawn.GetComponent <LineRenderer>();
		upgradeObject_aimLine = aimLineSpawn.transform.parent.gameObject;

		SetTankProperties ();
		Initialise ();

		upgradeObject_aimLine.SetActive (upgrade_aimLine);

		shells = burstAmount;
		lastFiredTime = Time.time;
	}

	void SetTankProperties ()
	{
		GlobalValues.acces.SetTankProperties ();
		type = GlobalValues.acces.userName;

		mSpeed = GlobalValues.properties.moveSpeed;
		tSpeed = GlobalValues.properties.turnSpeed;
		turretSpeed = GlobalValues.properties.turretTurnSpeed;
		burstAmount = GlobalValues.properties.ammo;
		reloadTime = GlobalValues.properties.reload;
		shellSpeed = GlobalValues.properties.shellSpeed;
		shellMaxBounces = GlobalValues.properties.shellBounces;
		mineAmount = GlobalValues.properties.mineAmount;
		mineRadius = GlobalValues.properties.mineRadius;

		upgrade_aimLine = GlobalValues.playerData.applied_AimLaser;
	}

	void OnDestroy ()
	{
		PlayerManager.acces.OnPlayerDestroy ();
	}

	void FixedUpdate ()
	{
		MoveTank ();
		TurnTurret ();
		FixPosition ();
	}

	void Update ()
	{
		if (upgrade_aimLine)
		{
			AimLaser ();
		}

		if (CrossPlatformInput.GetButton ("Fire1"))
		{
			FireShell (Tags.playerTank);
		}

		if (CrossPlatformInput.GetButtonDown ("Fire2"))
		{
			FireMine (Tags.playerTank, 0.2f);
		}

		Recoil ();
	}

	void MoveTank ()
	{
		float turnIn = CrossPlatformInput.GetAxis ("Horizontal");
		float moveIn = CrossPlatformInput.GetAxis ("Vertical");
		
		rigidbody.AddTorque (transform.up * turnIn * Time.deltaTime * tSpeed);
		rigidbody.AddRelativeForce (new Vector3 (0.0f, 0.0f, moveIn * Time.deltaTime * mSpeed));
	}

	void TurnTurret ()
	{
		if (!GlobalValues.acces.firstPerson)
		{
			Quaternion turretRotation = Quaternion.LookRotation (targetReticule.transform.position - turretRotator.transform.position);

			turretRotator.transform.rotation = Quaternion.Slerp
			(
				turretRotator.transform.rotation,
				Quaternion.Euler(0, turretRotation.eulerAngles.y, 0),
				turretSpeed * Time.deltaTime
			);
		}
		else if (GlobalValues.acces.firstPerson)
		{
			float turretRotation = (Input.mousePosition.x - (Screen.width / 2)) / (400 / turretSpeed);
			newYRotation += turretRotation;

			turretRotator.transform.rotation = Quaternion.Euler (0, newYRotation, 0);

//			turretRotator.transform.localRotation = Quaternion.Slerp
//			(
//				turretRotator.transform.localRotation,
//				Quaternion.Euler(0, newYRotation, 0),
//				turretSpeed * Time.deltaTime
//			);
		}
	}

	void AimLaser ()
	{
		Ray ray = new Ray (aimLineSpawn.transform.position, aimLineSpawn.transform.forward);
		RaycastHit hit;

		aimLine.SetPosition (0, ray.origin);

		if (Physics.Raycast(ray, out hit, 100))
		{
			aimLine.SetPosition (1, hit.point);
		}
		else
		{
			aimLine.SetPosition (1, ray.GetPoint (100));
		}
	}

	public GameObject GetReticule ()
	{
		return targetReticule;
	}
}
