using UnityEngine;
using System.Collections;

public class MineBehaviour : MonoBehaviour
{
	public string shooter;
	private string team;
	private string mineType;

	private float radiusMine;
	private float radiusExplosion;
	private float durationExplosion;
	
	public GameObject mineExplosionProx;
	public GameObject mineExplosionField;

	public void Initialise (float setRadius, float setDuration, string setShooter, string setTeam, string setType)
	{
		shooter = setShooter;
		team = setTeam;
		mineType = setType;

		durationExplosion = setDuration;
		radiusMine = setRadius * 0.3f;
		radiusExplosion = setRadius - 0.3f;

		transform.localScale = new Vector3 (radiusMine, radiusMine, radiusMine);
		transform.position = new Vector3 (transform.position.x, radiusMine / -5, transform.position.z);
		
		StartCoroutine (ExplodeByTime());
	}

	void OnTriggerEnter (Collider other)
	{
		if (SwitchBox.isHost)
		{
			if (team != Tags.npcTank && other.gameObject.tag == Tags.npcTank)
			{
				StartCoroutine (DelayedExplosion ());
			}
			
			if (team != Tags.playerTank && other.gameObject.tag == Tags.playerTank && other.GetComponent<MultiplayerTankDestruction>() != null && other.GetComponent<MultiplayerTankDestruction>().name != team)
			{
				StartCoroutine (DelayedExplosion ());
			}
		}
	}
	
	private IEnumerator ExplodeByTime ()
	{
		yield return new WaitForSeconds (21);
		StartCoroutine (DelayedExplosion ());
	}

	private IEnumerator DelayedExplosion ()
	{
		GetComponent <RpcSoundController>().PlaySound();
		yield return new WaitForSeconds (1);
		Explode ();
	}
	
	public void Explode ()
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Destroy (gameObject);
			GameObject mineExplosionClone = null;

			switch (mineType)
			{
			case "proxMine":
				mineExplosionClone = SwitchBox.Instantiate (mineExplosionProx, transform.position, transform.rotation);
				break;
			case "fieldMine":
				mineExplosionClone = SwitchBox.Instantiate (mineExplosionField, transform.position, transform.rotation);
				break;
			}

			MineExplosion explosionScript = mineExplosionClone.GetComponent <MineExplosion>();
			explosionScript.Initialise (radiusExplosion, durationExplosion, shooter);
		}
	}
}
