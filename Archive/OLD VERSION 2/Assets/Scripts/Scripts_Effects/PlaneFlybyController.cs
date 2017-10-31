using UnityEngine;
using System.Collections;

public class PlaneFlybyController : MonoBehaviour
{
	public GameObject ambientPlane;
	private bool spawned = false;
	private Transform mainCamera;

	private float waitTime;

	void Start ()
	{
		mainCamera = Camera.main.transform.parent;
		ambientPlane.SetActive (false);

		waitTime = Random.Range (10f, 60f);
	}
	
	void Update ()
	{
		if (SwitchBox.isHost && !spawned)
		{
			StartCoroutine (SpawnPlane ());
			spawned = true;
		}

		float cameraDistance = Mathf.Abs(mainCamera.position.x - (transform.position.x + ambientPlane.transform.localPosition.z));
		if (cameraDistance <= 100)
		{
			CameraShake.acces.SetAmplitude ("PlaneFlybyController", (100 - cameraDistance) / 400);
		}
		else if (cameraDistance <= 200)
		{
			CameraShake.acces.Unsubscribe ("PlaneFlybyController");
		}

		if (ambientPlane.transform.position.x > 1000)
		{
			Destroy (gameObject);
		}
	}

	IEnumerator SpawnPlane ()
	{
		yield return new WaitForSeconds (waitTime);

		if (SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC("EnablePlane", RPCMode.All);
		}
		else
		{
			EnablePlane ();
		}
	}

	[RPC] public void EnablePlane ()
	{
		ambientPlane.SetActive (true);
	}
}
