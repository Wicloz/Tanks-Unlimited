using UnityEngine;
using System.Collections;

public class PlayerController : TankController
{
	public static PlayerController thisTank;

	private bool upgrade_aimLine;
	
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
		if (SwitchBox.isServerOn && !GetComponent<NetworkView>().isMine)
		{
			Destroy (gameObject.GetComponent <PlayerController>());
		}

		aimLineSpawn = transform.Find ("TurretPivoter/Gturret/part_aimLaser/part_laserSpawn").gameObject;
		aimLine = aimLineSpawn.GetComponent <LineRenderer>();
		upgradeObject_aimLine = aimLineSpawn.transform.parent.gameObject;

		FirstPerson.acces.CameraRespawned ();
		SetTankProperties ();
		Initialise ();

		upgradeObject_aimLine.SetActive (upgrade_aimLine);

		shells = burstAmount;
		lastFiredTime = Time.time;
	}

	void OnDestroy ()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			FirstPerson.acces.CameraDestroyed ();
		}
	}

	private void SetTankProperties ()
	{
		ProfileManager.acces.CalculateTankProperties ();
		type = ProfileManager.acces.player.username;

		mSpeed = ProfileManager.acces.playerProperties.moveSpeed;
		tSpeed = ProfileManager.acces.playerProperties.turnSpeed;
		turretSpeed = ProfileManager.acces.playerProperties.turretTurnSpeed;
		burstAmount = ProfileManager.acces.playerProperties.ammo;
		reloadTime = ProfileManager.acces.playerProperties.reload;
		shellSpeed = ProfileManager.acces.playerProperties.shellSpeed;
		shellMaxBounces = ProfileManager.acces.playerProperties.shellBounces;
		mineAmount = ProfileManager.acces.playerProperties.mineAmount;
		mineRadius = ProfileManager.acces.playerProperties.mineRadius;

		upgrade_aimLine = ProfileManager.acces.player.applied_AimLaser;
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

		if (Input.GetButton ("Fire1"))
		{
			FireShell (Tags.playerTank);
		}

		if (Input.GetButtonDown ("Fire2"))
		{
			FireMine (Tags.playerTank, 0.2f);
		}

		Recoil ();
	}

	private void MoveTank ()
	{
		float turnIn = Input.GetAxis ("Horizontal");
		float moveIn = Input.GetAxis ("Vertical");
		
		GetComponent<Rigidbody>().AddTorque (transform.up * turnIn * Time.deltaTime * tSpeed);
		GetComponent<Rigidbody>().AddRelativeForce (new Vector3 (0.0f, 0.0f, moveIn * Time.deltaTime * mSpeed));
	}

	private void TurnTurret ()
	{
		if (!ProfileManager.acces.player.firstPerson)
		{
			Quaternion turretRotation = Quaternion.LookRotation (RespawnManager.acces.thisUser.reticule.transform.position - turretRotator.transform.position);

			turretRotator.transform.rotation = Quaternion.Slerp
			(
				turretRotator.transform.rotation,
				Quaternion.Euler(0, turretRotation.eulerAngles.y, 0),
				turretSpeed * Time.deltaTime
			);
		}
		else if (ProfileManager.acces.player.firstPerson)
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

	private void AimLaser ()
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
}
