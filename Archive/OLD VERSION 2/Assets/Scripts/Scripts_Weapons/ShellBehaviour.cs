using UnityEngine;
using System.Collections;

public class ShellBehaviour : MonoBehaviour
{
	private int maxBounces;
	private float shellVelocity;
	
	public GameObject shellExplosion;
	public GameObject shellBounce;
	
	private int bounces = 0;
	private float heightValue;
	private float nextBounceCheck = 0;
	
	private float shotTime;
	public string shooter;
	
	private Vector3 currentAngle;
	private bool doRaycast = true;
	private Vector3 leaveAngle;

	void Start ()
	{
		if (SwitchBox.isClient)
		{
			Destroy (GetComponent<ShellBehaviour>());
		}
	}

	public void Initialise (int setMaxBounces, float setSpeed, string setShooter)
	{
		maxBounces = setMaxBounces;
		shellVelocity = setSpeed;
		shooter = setShooter;

		GetComponent<Rigidbody>().velocity = transform.forward * shellVelocity;
		shotTime = Time.time;
		heightValue = transform.position.y;
		currentAngle = transform.rotation.eulerAngles;
	}
	
	void FixedUpdate ()
	{
		transform.rotation = Quaternion.Euler (currentAngle);
		GetComponent<Rigidbody>().velocity = transform.forward * shellVelocity;
		transform.position = new Vector3 (transform.position.x, heightValue, transform.position.z);
	}
	
	void Update ()
	{
		RaycastHit hit;
		Ray forwardRay = new Ray (transform.position, transform.forward);
		
		Debug.DrawRay (forwardRay.origin, forwardRay.direction * 1, Color.blue);
		if (doRaycast && Physics.Raycast (forwardRay, out hit, 1))
		{
			if (hit.transform.gameObject.tag == Tags.staticObject)
			{
				doRaycast = false;
				leaveAngle = BounceAngleCalculations.AngleCalculation (transform);
			}
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == Tags.staticObject)
		{
			if (bounces < maxBounces)
			{
				if (Time.time >= nextBounceCheck)
				{
					nextBounceCheck = Time.time + 0.1f;
					
					SwitchBox.Instantiate (shellBounce, transform.position, transform.rotation);
					currentAngle = leaveAngle;
					bounces++;
					
					Invoke ("DoRaycast", 0.04f);
				}
			}
			else
			{
				DestroyShell (true);
			}
		}
		
		else if (other.tag == Tags.shell)
		{
			DestroyShell (true);
		}
		
		else if (other.tag == Tags.dynamicObject || other.tag == Tags.dynamicPart)
		{
			DestroyShell (true);
			other.GetComponent<IDestroyable>().Destroy (shooter);
		}
		
		else if ((other.tag == Tags.playerTank || other.tag == Tags.npcTank) && (Time.time >= (shotTime + 0.1f) || shooter != other.GetComponent<TankController>().GetName()))
		{
			DestroyShell (false);
			other.GetComponent<IDestroyable>().Destroy (shooter);
		}
		
		else if (other.gameObject.name == "mine_destroyCollider")
		{
			DestroyShell (false);
			other.transform.parent.GetComponent<MineBehaviour>().Explode ();
		}
		
		else if (other.tag != Tags.ignoredByWeapons && other.tag != Tags.mine && other.tag != Tags.playerTank && other.tag != Tags.npcTank)
		{
			Debug.LogWarning ("Unknown object " + other.gameObject.name + " hit by Shell");
		}
	}
	
	private void DoRaycast ()
	{
		doRaycast = true;
	}
	
	public void DestroyShell (bool makeSound)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Destroy (gameObject);
			GameObject shellExplosionClone = SwitchBox.Instantiate (shellExplosion, transform.position, transform.rotation);
			
			if (makeSound)
			{
				shellExplosionClone.GetComponent <RpcSoundController>().PlaySound();
			}
		}
	}
	
	public string GetShooter ()
	{
		return shooter;
	}
}
