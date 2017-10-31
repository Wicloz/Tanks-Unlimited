using UnityEngine;
using System.Collections;

public class PlayerController : TankController
{
	private float newYRotation = 0;

	void Start ()
	{
		if (SwitchBox.isServerOn && !GetComponent<NetworkView>().isMine)
		{
			Destroy (GetComponent <PlayerController>());
		}

		else
		{
			SetTankValues (ProfileManager.acces.player.CreatePlayerConfig());
			tankName = ProfileManager.acces.player.username;
			tankType = "PlayerTank";
			Initialise ();
			FirstPerson.acces.CameraRespawned ();
		}
	}

	void OnDestroy ()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			FirstPerson.acces.CameraDestroyed ();
			CameraShake.acces.SetShake ("PlayerTank", ShakeValues.playerTank_intensity, ShakeValues.playerTank_duration, ShakeValues.playerTank_fade);
		}
	}

	new void FixedUpdate ()
	{
		if (!EscapeMenu.blockInput)
		{
			MoveTank ();
		}

		TurnTurret ();
		base.FixedUpdate ();
	}
	
	new void Update ()
	{
		base.Update ();

		if (!EscapeMenu.blockInput)
		{
			if (Input.GetButton ("Fire1"))
			{
				FireShell ();
			}
			
			if (Input.GetButtonDown ("Fire2"))
			{
				FireMine ();
			}
		}
	}

	// Tank movement
	private void MoveTank ()
	{
		float turnIn = Input.GetAxis ("Horizontal");
		float moveIn = Input.GetAxis ("Vertical");
		
		GetComponent<Rigidbody>().AddTorque (transform.up * turnIn * Time.deltaTime * properties.turnSpeed);
		GetComponent<Rigidbody>().AddRelativeForce (new Vector3 (0.0f, 0.0f, moveIn * Time.deltaTime * properties.moveSpeed));
	}
	
	private void TurnTurret ()
	{
		if (!ProfileManager.acces.player.firstPerson)
		{
			Quaternion turretRotation = Quaternion.LookRotation (RespawnManager.acces.thisUser.reticule.transform.position - turretRotator.transform.position);
			
			turretRotator.transform.localRotation = Quaternion.Slerp (turretRotator.transform.localRotation, Quaternion.Euler(0, turretRotation.eulerAngles.y - transform.rotation.eulerAngles.y, 0), properties.turretTurnSpeed * Time.deltaTime);
		}

		else if (ProfileManager.acces.player.firstPerson)
		{
			float turretRotation = (Input.mousePosition.x - (Screen.width / 2)) / (400 / properties.turretTurnSpeed);
			newYRotation += turretRotation;
			
			turretRotator.transform.localRotation = Quaternion.Euler (0, newYRotation - transform.rotation.eulerAngles.y, 0);
			
			//			turretRotator.transform.localRotation = Quaternion.Slerp
			//			(
			//				turretRotator.transform.localRotation,
			//				Quaternion.Euler(0, newYRotation, 0),
			//				turretSpeed * Time.deltaTime
			//			);
		}
	}
}
