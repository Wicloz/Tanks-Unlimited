using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankController : MonoBehaviour
{
	protected string tankType;
	protected string tankName;
	protected float scoreValue;

	protected TankProperties properties;

	public GameObject upgradeObject_aimLine;
	public GameObject aimLineSpawn;
	public LineRenderer aimLine;

	public GameObject turretRotator;
	public GameObject shotSpawn;

	public float reloadness;
	public int shellsLeft;
	protected float lastFiredTime = 0;
	protected float nextFirePrimary = 0;
	protected float nextFireSecondary = 0;

	private Quaternion rotateTo;

	protected void Initialise ()
	{
		if (SwitchBox.isClient)
		{
			GetComponent<NetworkView>().RPC("SetMineList", RPCMode.Server, properties.secondaryWeapon.mineAmount);
			GetComponent<NetworkView>().RPC ("RecieveValues", RPCMode.Server, tankType, GetName(), GetScore());
		}
		else
		{
			GetComponent<MultiplayerWeaponUse>().SetMineList (properties.secondaryWeapon.mineAmount);
			GetComponent<MultiplayerTankDestruction>().RecieveValues (tankType, GetName(), GetScore());
		}

		upgradeObject_aimLine.SetActive (properties.upgrade_aimLine);
		
		shellsLeft = properties.primaryWeapon.clipSize;
		lastFiredTime = Time.time;
	}

	protected void SetTankValues (TankConfigValues config)
	{
		properties = config.ToTankProperties ();
		scoreValue = config.TotalValues () * 10;
	}

	public void FixedUpdate ()
	{
		FixPosition ();
	}

	public void Update ()
	{
		if (properties.upgrade_aimLine)
		{
			AimLaser ();
		}
	}

	// Shell firing
	protected void FireShell ()
	{
		if (Time.time > nextFirePrimary)
		{
			if (shellsLeft > 0 && !shotSpawn.GetComponent <ShotSpawnProber>().isInObject)
			{
				shellsLeft--;
				lastFiredTime = Time.time;
				nextFirePrimary = Time.time + properties.primaryWeapon.fireDelay;

				switch (properties.primaryWeapon.weaponName)
				{
				case "standardShell":

					if (SwitchBox.isClient)
					{
						GetComponent<NetworkView>().RPC("FirePrimaryA", RPCMode.Server, shotSpawn.transform.position, shotSpawn.transform.rotation, tankName, properties.primaryWeapon.shellBounces, properties.primaryWeapon.shellSpeed);
					}
					else
					{
						GetComponent <MultiplayerWeaponUse>().FirePrimaryA (shotSpawn.transform.position, shotSpawn.transform.rotation, tankName, properties.primaryWeapon.shellBounces, properties.primaryWeapon.shellSpeed);
					}
					break;

				case "speedShell":

					if (SwitchBox.isClient)
					{
						GetComponent<NetworkView>().RPC("FirePrimaryB", RPCMode.Server, shotSpawn.transform.position, shotSpawn.transform.rotation, tankName, properties.primaryWeapon.shellBounces);
					}
					else
					{
						GetComponent <MultiplayerWeaponUse>().FirePrimaryB (shotSpawn.transform.position, shotSpawn.transform.rotation, tankName, properties.primaryWeapon.shellBounces);
					}
					break;
				}

				StartCoroutine (SetRecoil ());
				
				StopCoroutine ("ReloadShell");
				StartCoroutine ("ReloadShell");
			}
		}
	}

	protected IEnumerator ReloadShell ()
	{
		float startTime = Time.time;
		
		while (true)
		{
			reloadness = (Time.time - startTime) / properties.primaryWeapon.reloadTime;
			yield return null;
			
			if (shellsLeft == properties.primaryWeapon.clipSize)
			{
				StopCoroutine ("ReloadShell");
			}
			
			if (Time.time >= startTime + properties.primaryWeapon.reloadTime)
			{
				shellsLeft ++;
				startTime = Time.time;
			}
		}
	}

	// Mine firing
	protected void FireMine ()
	{
		if (Time.time > nextFireSecondary)
		{
			nextFireSecondary = Time.time + properties.secondaryWeapon.fireDelay;

			string teamName = this.gameObject.tag;
			if (teamName == Tags.playerTank && !GameController.acces.coop)
			{
				teamName = tankName;
			}

			switch (properties.secondaryWeapon.weaponName)
			{
			case "proxMine":

				if (SwitchBox.isClient)
				{
					GetComponent<NetworkView>().RPC("FireSecondaryA", RPCMode.Server, transform.position, transform.rotation, tankName, teamName, properties.secondaryWeapon.mineRadius);
				}
				else
				{
					GetComponent <MultiplayerWeaponUse>().FireSecondaryA (transform.position, transform.rotation, tankName, teamName, properties.secondaryWeapon.mineRadius);
				}
				break;

			case "fieldMine":

				if (SwitchBox.isClient)
				{
					GetComponent<NetworkView>().RPC("FireSecondaryB", RPCMode.Server, transform.position, transform.rotation, tankName, properties.secondaryWeapon.mineRadius, properties.secondaryWeapon.mineDuration);
				}
				else
				{
					GetComponent <MultiplayerWeaponUse>().FireSecondaryB (transform.position, transform.rotation, tankName, properties.secondaryWeapon.mineRadius, properties.secondaryWeapon.mineDuration);
				}
				break;
			}
		}
	}

	// Upgrades
	protected void AimLaser ()
	{
		Ray ray = new Ray (aimLineSpawn.transform.position, aimLineSpawn.transform.forward);
		RaycastHit hit;
		int layerMask = ~(1 << 11);
		
		aimLine.SetPosition (0, ray.origin);
		aimLine.SetPosition (1, ray.GetPoint (100));
		
		if (Physics.Raycast(ray, out hit, 100, layerMask))
		{
			aimLine.SetPosition (1, hit.point);
		}
	}

	// Recoil handling
	protected IEnumerator SetRecoil ()
	{
		float angle1 = Mathf.PingPong (turretRotator.transform.localRotation.eulerAngles.y, 180);
		float recoilAngle1 = (angle1 - 90) / 90;
		
		float angle2 = Mathf.PingPong (turretRotator.transform.localRotation.eulerAngles.y - 90, 180);
		float recoilAngle2 = (angle2 - 90) / -90;
		
		rotateTo = Quaternion.Euler (6 * recoilAngle1, transform.rotation.eulerAngles.y, 6 * recoilAngle2);

		yield return new WaitForSeconds (0.1f);
		rotateTo = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
	}

	// Movement
	protected void FixPosition ()
	{
		rotateTo = Quaternion.Euler (rotateTo.eulerAngles.x, transform.rotation.eulerAngles.y, rotateTo.eulerAngles.z);
		transform.position = new Vector3 (transform.position.x, 0.01f, transform.position.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotateTo, Time.deltaTime * 40);
	}

	// Data functions
	public float GetScore ()
	{
		return scoreValue;
	}

	public string GetName ()
	{
		return tankName;
	}
}

public class TankConfigValues
{
	public int moveSpeed = 0;
	public int turnSpeed = 0;
	public int turretTurnSpeed = 0;

	public PrimaryWeaponValues primaryWeapon = new PrimaryWeaponValues ();
	public SecondaryWeaponValues secondaryWeapon = new SecondaryWeaponValues ();
	
	public bool upgrade_aimLaser = false;

	public TankProperties ToTankProperties ()
	{
		TankProperties properties = new TankProperties ();
		
		properties.moveSpeed = 10000 + (this.moveSpeed * 800);
		properties.turnSpeed = 1500 + (this.turnSpeed * 100);
		properties.turretTurnSpeed = 2 + this.turretTurnSpeed;
		
		properties.primaryWeapon = this.primaryWeapon.ToProperties ();
		properties.secondaryWeapon = this.secondaryWeapon.ToProperties ();

		properties.upgrade_aimLine = this.upgrade_aimLaser;
		
		return properties;
	}

	public int TotalValues ()
	{
		return moveSpeed + turnSpeed + turretTurnSpeed + primaryWeapon.TotalValues() + secondaryWeapon.TotalValues();
	}

	public TankConfigValues ()
	{}
}

[System.Serializable]
public class TankProperties
{
	public float moveSpeed;
	public float turnSpeed;
	public float turretTurnSpeed;
	
	public PrimaryWeaponValues primaryWeapon;
	public SecondaryWeaponValues secondaryWeapon;
	
	public bool upgrade_aimLine;
	
	public TankProperties ()
	{}
}
