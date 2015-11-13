using UnityEngine;
using System.Collections;

public class NpcController : TankController
{
	public float scoreValue;
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
			Destroy (gameObject.GetComponent<NpcController>());
			Destroy (gameObject.GetComponent<NavMeshAgent>());
		}

		if (Application.loadedLevelName == "Play_InfiniMode")
		{
			StartCoroutine (RandomRotation ());
		}
		
		agent = GetComponent<NavMeshAgent>();

		SetTankProperties ();
		Initialise ();

		shells = burstAmount;
		lastFiredTime = Time.time;

		nextFire = Time.time + 2.0f;
		nextLay = Time.time + 10f;
	}

	private void SetTankProperties ()
	{
		type = "DefaultEnemy";

		mSpeed = 9000 + 1000 * MultiplayerManager.acces.gameSettings.difficulty;
		tSpeed = 1500 + 100 * MultiplayerManager.acces.gameSettings.difficulty;

		turretSpeed = 4 * MultiplayerManager.acces.gameSettings.difficulty;
		burstAmount = 1 + MultiplayerManager.acces.gameSettings.difficulty;
		reloadTime = 4 - MultiplayerManager.acces.gameSettings.difficulty;

		shellSpeed = 4 + MultiplayerManager.acces.gameSettings.difficulty;
		shellMaxBounces = -1 + MultiplayerManager.acces.gameSettings.difficulty;

		mineAmount = MultiplayerManager.acces.gameSettings.difficulty;
		mineRadius = 0.8f + ((float) MultiplayerManager.acces.gameSettings.difficulty / 10);

		scoreValue = 100 * MultiplayerManager.acces.gameSettings.difficulty;
	}

	void FixedUpdate ()
	{
		MoveTank ();
		AddRandomRotation ();
		TurnTurret ();
		FixPosition ();
	}

	void Update ()
	{
		targetPlayer = RespawnManager.acces.spawnList[target].tank;
		if (targetPlayer == null)
		{
			target = Random.Range (0, RespawnManager.acces.spawnList.Count);
		}

		if (Time.timeScale != 0)
		{
			CheckLOS ();
		}

		if (targetPlayer != null && targetFound)
		{
			FireShell (Tags.npcTank);
		}

		if (Application.loadedLevelName == "Play_InfiniMode")
		{
			FireMine (Tags.npcTank, 20);
		}

		Recoil ();
	}

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
			GetComponent<Rigidbody>().AddTorque (transform.up * randomDirection * Time.deltaTime * tSpeed);
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
					GetComponent<Rigidbody>().AddTorque (transform.up * -direction * Time.deltaTime * tSpeed);
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
			GetComponent<Rigidbody>().AddRelativeForce (new Vector3 (0.0f, 0.0f, Time.deltaTime * mSpeed));
		}
	}

	private void TurnTurret ()
	{
		if (targetPlayer != null)
		{
			Quaternion playerLocation = Quaternion.LookRotation (targetPlayer.transform.position - turretRotator.transform.position);

			float pingPonged = (Mathf.PingPong (Time.time * 5 * turretSpeed, 180) - 90);
			turretRotation = Quaternion.Euler(0, playerLocation.eulerAngles.y + pingPonged, 0.0f);

			turretRotator.transform.rotation = Quaternion.Slerp (turretRotator.transform.rotation, turretRotation, Time.deltaTime * turretSpeed);
		}
	}

	private void CheckLOS ()
	{
		RaycastHit hit;

		if (Physics.Raycast (shotSpawn.position, shotSpawn.forward, out hit) && !shotSpawn.gameObject.GetComponent <ShotSpawnProber>().isInObject)
		{
			Debug.DrawRay(shotSpawn.position, shotSpawn.forward * 20, Color.blue);
			if (hit.transform.gameObject.tag == Tags.playerTank)
			{
				targetFound = true;
			}

			else if (shellMaxBounces >= 1 && hit.transform.gameObject.tag == Tags.staticObject)
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

					else if (shellMaxBounces >= 2 && hit.transform.gameObject.tag == Tags.staticObject)
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

							else if (shellMaxBounces >= 3 && hit.transform.gameObject.tag == Tags.staticObject)
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

									else if (shellMaxBounces >= 4 && hit.transform.gameObject.tag == Tags.staticObject)
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

	public float GetPoints ()
	{
		return scoreValue;
	}
}
