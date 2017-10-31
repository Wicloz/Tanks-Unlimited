using UnityEngine;
using System.Collections;

public class ReticuleMover : MonoBehaviour
{
	private Vector3 reticuleLocation;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = gameObject.transform.position;
		if (stream.isWriting)
		{
			syncPosition = gameObject.transform.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			syncStartPosition = transform.position;
			syncEndPosition = syncPosition;
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
		}
	}

	void Update ()
	{
		if (!GlobalValues.acces.multiplayer || networkView.isMine)
		{
			if (!FirstPerson.acces.inFirstPerson && !GameStates.acces.GameOver)
			{
				RaycastHit hit;
				int layerMask = 1 << 10;
				
				Ray hitPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast (hitPointRay, out hit, Mathf.Infinity, layerMask))
				{
					reticuleLocation = hit.point;
				}
				
				transform.position = new Vector3 (reticuleLocation.x, 0.634f, reticuleLocation.z);
			}
			else
			{
				transform.position = new Vector3 (0, -100, 0);
			}
		}

		else if (!networkView.isMine)
		{
			if (!GameStates.acces.GameOver)
			{
				syncTime += Time.deltaTime;
				transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
			}
			else
			{
				transform.position = new Vector3 (0, -100, 0);
			}
		}
	}
}
