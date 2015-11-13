using UnityEngine;
using System.Collections;

public class MineDestroyer : MonoBehaviour
{
	private string shooter;
	private float radiusMine;
	private float radiusExplosion;
	private Collider mineCollider;

	public GameObject explosion;

	void Start ()
	{
		if (SwitchBox.isClient)
		{
			Destroy (gameObject.GetComponent<MineDestroyer>());
		}

		if (gameObject.name == "weapon_mine_standard_(Clone)")
		{
			transform.localScale = new Vector3 (radiusMine, radiusMine, radiusMine);
			transform.position = new Vector3 (transform.position.x, radiusMine / -5, transform.position.z);
		}

		StartCoroutine (ExplodeByTime());
	}

	public void SetProperties (float setRadius, string setShooter)
	{
		radiusMine = setRadius * 0.3f;
		radiusExplosion = setRadius - 0.3f;
		shooter = setShooter;
	}

	public float GetProperties ()
	{
		return radiusExplosion;
	}

	void OnTriggerEnter (Collider other)
	{
		mineCollider = other;
		if (gameObject.name == "weapon_mine_standard_(Clone)")
		{
			CollisionOuter ();
		}
		if (gameObject.name == "mine_destroyCollider")
		{
			CollisionInner ();
		}
	}

	void CollisionOuter ()
	{
		if (shooter == Tags.npcTank && mineCollider.gameObject.tag == Tags.playerTank)
		{
			StartCoroutine (Explode ("outer"));
		}
		if (shooter == Tags.playerTank && mineCollider.gameObject.tag == Tags.npcTank)
		{
			StartCoroutine (Explode ("outer"));
		}
	}

	void CollisionInner ()
	{
		if (mineCollider.gameObject.tag == Tags.shell)
		{
			StartCoroutine (Explode ("inner"));
		}
	}

	IEnumerator ExplodeByTime ()
	{
		yield return new WaitForSeconds (21);
		StartCoroutine (Explode ("outer"));
	}

	public IEnumerator Explode (string mode)
	{
		switch (mode)
		{
		case "inner":
			SwitchBox.Destroy (gameObject.transform.parent.gameObject);
			radiusExplosion = gameObject.transform.parent.gameObject.GetComponent <MineDestroyer>().GetProperties();
			break;
		case "outer":
			gameObject.GetComponent <RpcSoundController>().PlaySound();
			yield return new WaitForSeconds (1);
			SwitchBox.Destroy (gameObject);
			break;
		}

		GameObject mineExplosionClone = SwitchBox.Instantiate (explosion, transform.position, transform.rotation);

		MineExplosion mineExplosion = mineExplosionClone.GetComponent <MineExplosion>();
		mineExplosion.SetProperties(radiusExplosion);
	}
}
