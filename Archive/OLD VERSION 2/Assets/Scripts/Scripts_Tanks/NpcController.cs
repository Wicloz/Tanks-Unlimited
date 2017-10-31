using UnityEngine;
using System.Collections;

public class NpcController : TankController
{
	public GameObject tankExplosion;

	private GameObject targetPlayer = null;
	private bool targetFound;
	private int target;
	
	public GameObject castProbe;
	private Quaternion turretRotation = Quaternion.Euler(270, 0, 0);
	private NavMeshAgent agent;
	private GameObject defenseTarget;
	
	public GameObject rayCastProber;
	public GameObject[] rayCastProbers;
	private int direction = 0;
	private int randomDirection = 0;

	void Start ()
	{
		if (SwitchBox.isClient)
		{
			Destroy (GetComponent<NpcController>());
			Destroy (GetComponent<NavMeshAgent>());
		}

		else
		{
			TankConfigValues tankValues = new TankConfigValues ();
			tankValues.secondaryWeapon.fireDelay = 10;
			
			tankValues.moveSpeed = MultiplayerManager.acces.gameSettings.difficulty * 2;
			tankValues.turnSpeed = MultiplayerManager.acces.gameSettings.difficulty * 2;
			tankValues.turretTurnSpeed = MultiplayerManager.acces.gameSettings.difficulty * 2;

			tankValues.primaryWeapon.weaponName = "standardShell";
			tankValues.primaryWeapon.clipSize = MultiplayerManager.acces.gameSettings.difficulty;
			tankValues.primaryWeapon.reloadTime = MultiplayerManager.acces.gameSettings.difficulty;
			tankValues.primaryWeapon.shellSpeed = MultiplayerManager.acces.gameSettings.difficulty ;
			tankValues.primaryWeapon.shellBounces = MultiplayerManager.acces.gameSettings.difficulty - 1;

			tankValues.secondaryWeapon.weaponName = "proxMine";
			tankValues.secondaryWeapon.mineAmount = MultiplayerManager.acces.gameSettings.difficulty;
			tankValues.secondaryWeapon.mineRadius = MultiplayerManager.acces.gameSettings.difficulty;

			SetTankValues (tankValues);
			tankName = "Generic Tank";
			tankType = "NpcTank";
			Initialise ();
			
			if (Application.loadedLevelName == "Play_InfiniMode")
			{
				Destroy (GetComponent<NavMeshAgent>());
				StartCoroutine (RandomRotation ());
			}
			
			agent = GetComponent<NavMeshAgent>();
			
			nextFirePrimary = Time.time + 2.0f;
			nextFireSecondary = Time.time + 10f;
		}
	}
	
	new void FixedUpdate ()
	{
		if (MultiplayerManager.acces.gameState == GameState.Running)
		{
			MoveTank ();
			AddRandomRotation ();
			TurnTurret ();
		}

		base.FixedUpdate ();
	}
	
	new void Update ()
	{
		base.Update ();

		if (MultiplayerManager.acces.gameState == GameState.Running)
		{
			targetPlayer = RespawnManager.acces.spawnList[target].tank;
			if (targetPlayer == null)
			{
				target = Random.Range (0, RespawnManager.acces.spawnList.Count);
			}
			
			CheckLOS ();
			
			if (targetPlayer != null && targetFound)
			{
				FireShell ();
			}
			
			if (Application.loadedLevelName == "Play_InfiniMode")
			{
				FireMine ();
			}
		}
	}

	// Tank movement
	private void MoveTank ()
	{
		if (Application.loadedLevelName == "Play_InfiniMode")
		{
			RandomMovement ();
		}
		
		else if (Application.loadedLevelName == "Play_DefenseMode")
		{
			defenseTarget = GameObject.Find ("part_triggerZone_navMeshTarget");
			agent.SetDestination(defenseTarget.transform.position);
		}
	}
	
	private IEnumerator RandomRotation ()
	{
		while (true)
		{
			yield return new WaitForSeconds (Random.Range (1f, 2f));

			if (Random.value <= 0.5f)
			{
				randomDirection = 1;
			}
			else
			{
				randomDirection = -1;
			}
			
			yield return new WaitForSeconds (Random.Range (1f, 2f));
			randomDirection = 0;
		}
	}
	
	private void AddRandomRotation ()
	{
		if (Application.loadedLevelName == "Play_InfiniMode" && direction == 0)
		{
			GetComponent<Rigidbody>().AddTorque (transform.up * randomDirection * Time.deltaTime * properties.turnSpeed);
		}
	}
	
	private void RandomMovement()
	{
		RaycastHit hit;
		float length = 4;
		
		if (direction == 0)
		{
			foreach (GameObject prober in rayCastProbers)
			{
				Debug.DrawRay (prober.transform.position, prober.transform.forward * length, Color.green);
				if (Physics.Raycast (prober.transform.position, prober.transform.forward, out hit, length))
				{
					if (hit.transform.gameObject.tag == Tags.dynamicObject || hit.transform.gameObject.tag == Tags.staticObject || hit.transform.gameObject.tag == Tags.npcTank || hit.transform.gameObject.tag == Tags.playerTank)
					{
						Debug.DrawRay (prober.transform.position, prober.transform.forward * length, Color.red);
						if (prober.name == "raycastStaticProbe1")
						{
							direction = 1;
							rayCastProber.transform.localPosition = new Vector3 (1.2f, 0.8f, 2f);
							break;
						}
						else if (prober.name == "raycastStaticProbe2")
						{
							direction = -1;
							rayCastProber.transform.localPosition = new Vector3 (-1.2f, 0.8f, 2f);
							break;
						}
					}
				}
			}
		}
		else
		{
			Debug.DrawRay (rayCastProber.transform.position, rayCastProber.transform.forward * (length + 1), Color.red);
			if (Physics.Raycast (rayCastProber.transform.position, rayCastProber.transform.forward, out hit, length + 1))
			{
				if (hit.transform.gameObject.tag == Tags.dynamicObject || hit.transform.gameObject.tag == Tags.staticObject || hit.transform.gameObject.tag == Tags.npcTank || hit.transform.gameObject.tag == Tags.playerTank)
				{
					GetComponent<Rigidbody>().AddTorque (transform.up * -direction * Time.deltaTime * properties.turnSpeed);
				}
				else 
				{
					direction = 0;
				}
			}
			else 
			{
				direction = 0;
			}
		}
		
		if (GetComponent<Rigidbody>().velocity.x >= 0.5f || GetComponent<Rigidbody>().velocity.x <= -0.5f || GetComponent<Rigidbody>().velocity.z >= 0.5f || GetComponent<Rigidbody>().velocity.z <= -0.5f || direction == 0)
		{
			GetComponent<Rigidbody>().AddRelativeForce (new Vector3 (0.0f, 0.0f, Time.deltaTime * properties.moveSpeed));
		}
	}
	
	private void TurnTurret ()
	{
		if (targetPlayer != null)
		{
			Quaternion playerLocation = Quaternion.LookRotation (targetPlayer.transform.position - turretRotator.transform.position);
			
			float pingPonged = (Mathf.PingPong (Time.time * 5 * properties.turretTurnSpeed, 180) - 90);
			turretRotation = Quaternion.Euler(0, playerLocation.eulerAngles.y + pingPonged - transform.rotation.eulerAngles.y, 0.0f);
			
			turretRotator.transform.localRotation = Quaternion.Slerp (turretRotator.transform.localRotation, turretRotation, Time.deltaTime * properties.turretTurnSpeed);
		}
	}

	// Tank firing
	private void CheckLOS ()
	{
		RaycastHit hit;
		
		if (Physics.Raycast (shotSpawn.transform.position, shotSpawn.transform.forward, out hit) && !shotSpawn.gameObject.GetComponent <ShotSpawnProber>().isInObject)
		{
			Debug.DrawRay(shotSpawn.transform.position, shotSpawn.transform.forward * 20, Color.blue);
			if (hit.transform.gameObject.tag == Tags.playerTank)
			{
				targetFound = true;
			}
			
			else if (properties.primaryWeapon.shellBounces >= 1 && hit.transform.gameObject.tag == Tags.staticObject)
			{
				Vector3 leaveAngle = BounceAngleCalculations.AngleCalculation (shotSpawn.transform);
				GameObject castProbeClone = (GameObject) Instantiate (castProbe, hit.point, Quaternion.Euler(leaveAngle));
				if (Physics.Raycast (castProbeClone.transform.position, castProbeClone.transform.forward, out hit))
				{
					Debug.DrawRay (castProbeClone.transform.position, castProbeClone.transform.forward * 20, Color.yellow);
					if (hit.transform.gameObject.tag == Tags.playerTank)
					{
						targetFound = true;
					}
					
					else if (properties.primaryWeapon.shellBounces >= 2 && hit.transform.gameObject.tag == Tags.staticObject)
					{
						leaveAngle = BounceAngleCalculations.AngleCalculation (castProbeClone.transform);
						castProbeClone = (GameObject) Instantiate (castProbe, hit.point, Quaternion.Euler(leaveAngle));
						if (Physics.Raycast (castProbeClone.transform.position, castProbeClone.transform.forward, out hit))
						{
							Debug.DrawRay (castProbeClone.transform.position, castProbeClone.transform.forward * 20, Color.yellow);
							if (hit.transform.gameObject.tag == Tags.playerTank)
							{
								targetFound = true;
							}
							
							else if (properties.primaryWeapon.shellBounces >= 3 && hit.transform.gameObject.tag == Tags.staticObject)
							{
								leaveAngle = BounceAngleCalculations.AngleCalculation (castProbeClone.transform);
								castProbeClone = (GameObject) Instantiate (castProbe, hit.point, Quaternion.Euler(leaveAngle));
								if (Physics.Raycast (castProbeClone.transform.position, castProbeClone.transform.forward, out hit))
								{
									Debug.DrawRay (castProbeClone.transform.position, castProbeClone.transform.forward * 20, Color.yellow);
									if (hit.transform.gameObject.tag == Tags.playerTank)
									{
										targetFound = true;
									}
									
									else if (properties.primaryWeapon.shellBounces >= 4 && hit.transform.gameObject.tag == Tags.staticObject)
									{
										targetFound = false;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				targetFound = false;
			}
		}
	}
}
