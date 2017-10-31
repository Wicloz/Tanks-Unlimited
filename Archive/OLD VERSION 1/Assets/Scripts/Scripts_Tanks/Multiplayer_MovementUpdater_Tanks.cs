using UnityEngine;
using System.Collections;

public class Multiplayer_MovementUpdater_Tanks : MonoBehaviour
{
	public GameObject turretRotator;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	
	private Vector3 syncStartPosition = new Vector3 (0, -10, 0);
	private Vector3 syncEndPosition = new Vector3 (0, -10, 0);
	private Quaternion syncStartRotation = Quaternion.identity;
	private Quaternion syncEndRotation = Quaternion.identity;
	private Quaternion syncStartTurret = Quaternion.identity;
	private Quaternion syncEndTurret = Quaternion.identity;

	void Start ()
	{
		turretRotator = transform.Find ("TurretPivoter").gameObject;

		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		syncStartRotation = transform.rotation;
		syncEndRotation = transform.rotation;
		syncStartTurret = turretRotator.transform.rotation;
		syncEndTurret = turretRotator.transform.rotation;
	}

	void Update ()
	{
		if (GlobalValues.acces.multiplayer && !networkView.isMine)
		{
			SyncedMovement();
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = new Vector3 (0, -10, 0);
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		Quaternion syncTurret = Quaternion.identity;
		
		if (stream.isWriting)
		{
			syncPosition = gameObject.transform.position;
			stream.Serialize(ref syncPosition);
			
			syncRotation = gameObject.transform.rotation;
			stream.Serialize(ref syncRotation);
			
			syncTurret = turretRotator.gameObject.transform.rotation;
			stream.Serialize(ref syncTurret);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			syncStartPosition = transform.position;
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			
			stream.Serialize(ref syncRotation);
			syncStartRotation = transform.rotation;
			syncEndRotation = syncRotation;

			stream.Serialize(ref syncTurret);
			syncStartTurret = turretRotator.transform.rotation;
			syncEndTurret = syncTurret;
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		transform.rotation = Quaternion.Slerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
		turretRotator.transform.rotation = Quaternion.Slerp(syncStartTurret, syncEndTurret, syncTime / syncDelay);
	}
}
