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
	private string shooter;

	private Vector3 currentAngle;
	private bool doRaycast = true;
	private Vector3 leaveAngle;

	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * shellVelocity;

		if (SwitchBox.isClient)
		{
			Destroy (gameObject.GetComponent<ShellBehaviour>());
		}

		shotTime = Time.time;
		heightValue = transform.position.y;
		currentAngle = transform.rotation.eulerAngles;
	}

	public void SetProperties (int setMaxBounces, float setSpeed, string setShooter)
	{
		maxBounces = setMaxBounces;
		shellVelocity = setSpeed;
		shooter = setShooter;
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

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
		case Tags.staticObject:
			StaticObject (other);
			break;
		case Tags.dynamicObject:
			DynamicObject (other);
			break;
		case Tags.dynamicPart:
			DynamicPart (other);
			break;
		case Tags.playerTank:
			PlayerTank (other);
			break;
		case Tags.npcTank:
			NpcTank (other);
			break;
		case Tags.shell:
			DestroyShell (other);
			break;
		case Tags.mine:
			break;
		case Tags.ignoredByShell:
			break;
		default:
			Debug.LogWarning ("Unknown object " + other.gameObject.name + " hit by Shell");
			break;
		}
	}

	private void StaticObject (Collider collision)
	{
		if (bounces < maxBounces)
		{
			if (Time.time >= nextBounceCheck)
			{
				nextBounceCheck = Time.time + 0.1f;
				ShellBounce ();
				Invoke ("DoRaycast", 0.04f);
			}
		}
		else
		{
			DestroyShell(collision);
		}
	}
	
	private void DynamicObject (Collider collision)
	{
		DestroyShell(collision);
		Destructor.DynamicObject (collision.gameObject);
	}

	private void DynamicPart (Collider collision)
	{
		DestroyShell(collision);
		Destructor.DynamicPart (collision.gameObject);
	}

	private void PlayerTank (Collider collision)
	{
		if (Time.time >= (shotTime + 0.1f) || shooter == Tags.npcTank)
		{
			DestroyShell(collision);
			Destructor.PlayerTank (collision.gameObject);
		}
	}

	private void NpcTank (Collider collision)
	{
		if (Time.time >= (shotTime + 0.1f) || shooter == Tags.playerTank)
		{
			DestroyShell(collision);
			Destructor.NpcTank (collision.gameObject);
		}
	}
	
	private void ShellBounce ()
	{
		SwitchBox.Instantiate (shellBounce, transform.position, transform.rotation);
		currentAngle = leaveAngle;
		bounces++;
	}

	private void DoRaycast ()
	{
		doRaycast = true;
	}

	public void DestroyShell (Collider collision)
	{
		SwitchBox.Destroy (gameObject);
		GameObject shellExplosionClone = SwitchBox.Instantiate (shellExplosion, transform.position, transform.rotation);

		if (collision == null || collision.gameObject.tag == Tags.staticObject || collision.gameObject.tag == Tags.shell || collision.gameObject.tag == Tags.dynamicPart || collision.gameObject.tag == Tags.dynamicObject)
		{
			shellExplosionClone.gameObject.GetComponent <RpcSoundController>().PlaySound();
		}
	}
}
