using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour
{
	public string shooter;

	public ParticleSystem explosion;
	
	protected float explosionDuration;
	protected bool exploding = true;
	
	void Start ()
	{
		ClientEffects ();

		if (SwitchBox.isClient)
		{
			Destroy (GetComponent<MineExplosion>());
		}
	}

	public virtual void ClientEffects ()
	{}
	
	public void Initialise (float setRadius, float setDuration, string shooter)
	{
		this.shooter = shooter;
		explosionDuration = setDuration;

		explosion.startSize = setRadius / 0.7f;
		explosion.startLifetime = setDuration;

		transform.localScale = new Vector3 (setRadius, setRadius, setRadius);
		StartCoroutine (StopExploding());
	}

	protected IEnumerator StopExploding ()
	{
		yield return new WaitForSeconds (explosionDuration);
		exploding = false;
		yield return new WaitForSeconds (4);
		SwitchBox.Destroy (gameObject);
	}

	void OnTriggerEnter (Collider other)
	{
		if (exploding)
		{
			ReactToTrigger (other.gameObject);
		}
	}

	public virtual void ReactToTrigger (GameObject other)
	{}

	public string GetShooter ()
	{
		return shooter;
	}
}
