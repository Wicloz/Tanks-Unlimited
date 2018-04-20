using UnityEngine;
using System.Collections;

public class Multiplayer_MovementUpdater_GeneralObjects : MonoBehaviour
{
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	
	private Vector3 syncStartPosition = new Vector3 (0, -10, 0);
	private Vector3 syncEndPosition = new Vector3 (0, -10, 0);
	private Quaternion syncStartRotation = Quaternion.identity;
	private Quaternion syncEndRotation = Quaternion.identity;

	void Start ()
	{
		syncStartPosition = transform.position;
		syncEndPosition = transform.position;
		syncStartRotation = transform.rotation;
		syncEndRotation = transform.rotation;
	}

	void Update ()
	{
		if (SwitchBox.isClient)
		{
			SyncedMovement();
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = new Vector3 (0, -10, 0);
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		
		if (stream.isWriting)
		{
			syncPosition = gameObject.transform.position;
			stream.Serialize(ref syncPosition);
			
			syncRotation = gameObject.transform.rotation;
			stream.Serialize(ref syncRotation);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			syncStartPosition = transform.position;
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			
			stream.Serialize(ref syncRotation);
			syncStartRotation = transform.rotation;
			syncEndRotation = syncRotation;
			
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
	}
}
