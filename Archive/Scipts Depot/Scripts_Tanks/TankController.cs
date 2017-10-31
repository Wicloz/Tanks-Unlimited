using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
	public string type;
	protected float mSpeed, tSpeed;
	protected float turretSpeed;
	protected float reloadTime;
	protected int burstAmount;
	protected float shellSpeed;
	protected int shellMaxBounces;
	protected float mineRadius;
	protected int mineAmount;

	public GameObject turretRotator;
	public Transform shotSpawn;

	public float reloadness;
	public int shells;
	protected float lastFiredTime = 0.0f;
	protected float nextFire = 0.0f;
	protected float nextLay = 0.0f;

	private bool recoil;
	private Quaternion recoilDestination;

	protected void Initialise ()
	{
		if (SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC("SetMineList", RPCMode.AllBuffered, mineAmount);
		}
		else
		{
			gameObject.GetComponent <MultiplayerWeaponUse>().SetMineList (mineAmount);
		}
	}

	protected void FireShell (string shooter)
	{
		if (Time.time > nextFire && Time.timeScale != 0)
		{
			if (shells > 0 && !shotSpawn.gameObject.GetComponent <ShotSpawnProber>().isInObject)
			{
				shells--;
				lastFiredTime = Time.time;
				nextFire = Time.time + 0.2f;
					
				if (SwitchBox.isServerOn)
				{
					GetComponent<NetworkView>().RPC("InstantiateShell", RPCMode.AllBuffered, shotSpawn.position, shotSpawn.rotation, shellMaxBounces, shellSpeed, shooter);
				}
				else
				{
					gameObject.GetComponent <MultiplayerWeaponUse>().InstantiateShell (shotSpawn.position, shotSpawn.rotation, shellMaxBounces, shellSpeed, shooter);
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
			reloadness = (Time.time - startTime) / reloadTime;
			yield return null;

			if (shells == burstAmount)
			{
				StopCoroutine ("ReloadShell");
			}

			if (Time.time >= startTime + reloadTime)
			{
				shells ++;
				startTime = Time.time;
			}
		}
	}

	protected void FireMine (string shooter, float delay)
	{
		if (Time.time > nextLay && Time.timeScale != 0)
		{
			nextLay = Time.time + delay;
				
			if (SwitchBox.isServerOn)
			{
				GetComponent<NetworkView>().RPC("InstantiateMine", RPCMode.AllBuffered, transform.position, transform.rotation, mineRadius, shooter);
			}
			else
			{
				gameObject.GetComponent <MultiplayerWeaponUse>().InstantiateMine (transform.position, transform.rotation, mineRadius, shooter);
			}
		}
	}

	protected IEnumerator SetRecoil ()
	{
		float angle1 = Mathf.PingPong (turretRotator.transform.localRotation.eulerAngles.y, 180);
		float recoilAngle1 = (angle1 - 90) / 90;
		
		float angle2 = Mathf.PingPong (turretRotator.transform.localRotation.eulerAngles.y - 90, 180);
		float recoilAngle2 = (angle2 - 90) / -90;
		
		recoilDestination = Quaternion.Euler (6 * recoilAngle1, transform.rotation.eulerAngles.y, 6 * recoilAngle2);
		
		recoil = true;
		yield return new WaitForSeconds (0.1f);
		recoil = false;
	}
	
	protected void Recoil ()
	{
		if (recoil)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, recoilDestination, Time.deltaTime * 40);
		}
	}

	protected void FixPosition ()
	{
		Quaternion current = transform.rotation;
		Quaternion dRotation = Quaternion.Euler (0, transform.eulerAngles.y, 0);
		transform.position = new Vector3 (transform.position.x, 0.01f, transform.position.z);
		transform.rotation = Quaternion.Slerp (current, dRotation, Time.deltaTime * 10);
	}
}
