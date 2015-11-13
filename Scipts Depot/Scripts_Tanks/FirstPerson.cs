using UnityEngine;
using System.Collections;

public class FirstPerson : MonoBehaviour
{
	public static FirstPerson acces;

	// GameObjects
	private GameObject cameraMain;
	private Transform firstPersonTransform = null;
	private GameObject firstPersonReticule = null;
	public GameObject minimap;

	// Camara data
	private Vector3 initialCameraPosition;
	private Quaternion initialCameraRotation;
	private float initialCameraFOV;
	private float newCameraFOV = 60;
	public bool inFirstPerson;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		cameraMain = Camera.main.transform.parent.gameObject;
		initialCameraPosition = cameraMain.transform.position;
		initialCameraRotation = cameraMain.transform.rotation;
		initialCameraFOV = cameraMain.transform.Find("Main Camera").GetComponent<Camera>().fieldOfView;
	}

	void OnDestroy ()
	{
		SavePosition ();
	}

	void Update ()
	{
		if (Input.GetButtonDown ("First Person Toggle") && !ProfileManager.acces.player.firstPerson)
		{
			ProfileManager.acces.player.firstPerson = true;
			FirstPersonMode ();
		}
		else if (Input.GetButtonDown ("First Person Toggle") && ProfileManager.acces.player.firstPerson)
		{
			ProfileManager.acces.player.firstPerson = false;
			TopViewMode ();
		}

		if (inFirstPerson)
		{
			ZoomCamera ();
		}
		TeleportCamera ();
	}

	private void SavePosition ()
	{
		if (firstPersonTransform != null)
		{
			ProfileManager.acces.firstPersonCameraPosition = firstPersonTransform.localPosition;
			ProfileManager.acces.firstPersonCameraRotation = firstPersonTransform.localRotation;
		}
	}

	private void LoadPosition ()
	{
		if (firstPersonTransform != null)
		{
			firstPersonTransform.localPosition = ProfileManager.acces.firstPersonCameraPosition;
			firstPersonTransform.localRotation = ProfileManager.acces.firstPersonCameraRotation;
		}
	}

	public void CameraDestroyed ()
	{
		TopViewMode ();
	}

	public void CameraRespawned ()
	{
		firstPersonTransform = RespawnManager.acces.thisUser.tank.transform.Find ("TurretPivoter/Gturret/First Person Camera");
		firstPersonReticule = RespawnManager.acces.thisUser.tank.transform.Find ("TurretPivoter/Gturret/part_reticule").gameObject;
		
		if (ProfileManager.acces.player.firstPerson)
		{
			FirstPersonMode ();
		}
		else
		{
			TopViewMode ();
		}
	}

	private void FirstPersonMode ()
	{
		inFirstPerson = true;
		LoadPosition ();

		minimap.SetActive (true);

		if (firstPersonTransform != null)
		{
			firstPersonTransform.gameObject.SetActive (true);
			firstPersonReticule.gameObject.SetActive (true);
		}
	}
	
	private void TopViewMode ()
	{
		inFirstPerson = false;
		SavePosition ();

		minimap.SetActive (false);

		if (firstPersonTransform != null)
		{
			firstPersonTransform.gameObject.SetActive (false);
			firstPersonReticule.gameObject.SetActive (false);
		}
	}
	
	private void TeleportCamera ()
	{
		if (inFirstPerson)
		{
			cameraMain.transform.position = firstPersonTransform.transform.position;
			cameraMain.transform.rotation = firstPersonTransform.transform.rotation;
			cameraMain.transform.Find("Main Camera").GetComponent<Camera>().fieldOfView = newCameraFOV;
		}
		else
		{
			cameraMain.transform.position = initialCameraPosition;
			cameraMain.transform.rotation = initialCameraRotation;
			cameraMain.transform.Find("Main Camera").GetComponent<Camera>().fieldOfView = initialCameraFOV;
		}
	}
	
	private void ZoomCamera ()
	{
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll > 0)
		{
			firstPersonTransform.localPosition = Vector3.Lerp (firstPersonTransform.localPosition, new Vector3 (0, 1.8f, 2.4f), Time.deltaTime * scroll * 50);
			firstPersonTransform.localRotation = Quaternion.Slerp (firstPersonTransform.localRotation, Quaternion.identity, Time.deltaTime * scroll * 50);
		}
		else if (scroll < 0)
		{
			firstPersonTransform.localPosition = Vector3.Lerp (firstPersonTransform.localPosition, new Vector3 (0, 2.6f, -4.0f), Time.deltaTime * -scroll * 50);
			firstPersonTransform.localRotation = Quaternion.Slerp (firstPersonTransform.localRotation, Quaternion.Euler (20, 0, 0), Time.deltaTime * -scroll * 50);
		}
	}
}
