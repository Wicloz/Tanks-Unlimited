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
		initialCameraFOV = cameraMain.transform.Find("Main Camera").camera.fieldOfView;
	}

	void OnDestroy ()
	{
		SavePosition ();
	}

	void Update ()
	{
		if (CrossPlatformInput.GetButtonDown ("First Person Toggle") && !GlobalValues.acces.firstPerson && !GlobalValues.acces.coOp)
		{
			GlobalValues.acces.firstPerson = true;
			FirstPersonMode ();
		}
		else if (CrossPlatformInput.GetButtonDown ("First Person Toggle") && GlobalValues.acces.firstPerson)
		{
			GlobalValues.acces.firstPerson = false;
			TopViewMode ();
		}

		if (inFirstPerson)
		{
			ZoomCamera ();
		}
		TeleportCamera ();
	}

	void SavePosition ()
	{
		if (firstPersonTransform != null)
		{
			GlobalValues.acces.firstPersonCameraPosition = firstPersonTransform.localPosition;
			GlobalValues.acces.firstPersonCameraRotation = firstPersonTransform.localRotation;
		}
	}

	void LoadPosition ()
	{
		if (firstPersonTransform != null)
		{
			firstPersonTransform.localPosition = GlobalValues.acces.firstPersonCameraPosition;
			firstPersonTransform.localRotation = GlobalValues.acces.firstPersonCameraRotation;
		}
	}

	public void OnPlayerCameraDestroy ()
	{
		TopViewMode ();
	}

	public void CameraRespawn ()
	{
		firstPersonTransform = HierarchyManager.tankParent.transform.Find ("ThisPlayerTank/TurretPivoter/Gturret/First Person Camera");
		firstPersonReticule = HierarchyManager.tankParent.transform.Find ("ThisPlayerTank/TurretPivoter/Gturret/part_reticule").gameObject;
		
		if (GlobalValues.acces.firstPerson)
		{
			FirstPersonMode ();
		}
		else
		{
			TopViewMode ();
		}
	}

	void FirstPersonMode ()
	{
		inFirstPerson = true;
		LoadPosition ();

		minimap.SetActive (true);
		Screen.showCursor = true;

		if (firstPersonTransform != null)
		{
			firstPersonTransform.gameObject.SetActive (true);
			firstPersonReticule.gameObject.SetActive (true);
		}
	}
	
	void TopViewMode ()
	{
		inFirstPerson = false;
		SavePosition ();

		minimap.SetActive (false);
		Screen.showCursor = false;

		if (firstPersonTransform != null)
		{
			firstPersonTransform.gameObject.SetActive (false);
			firstPersonReticule.gameObject.SetActive (false);
		}
	}
	
	void TeleportCamera ()
	{
		if (inFirstPerson)
		{
			cameraMain.transform.position = firstPersonTransform.transform.position;
			cameraMain.transform.rotation = firstPersonTransform.transform.rotation;
			cameraMain.transform.Find("Main Camera").camera.fieldOfView = newCameraFOV;
		}
		else
		{
			cameraMain.transform.position = initialCameraPosition;
			cameraMain.transform.rotation = initialCameraRotation;
			cameraMain.transform.Find("Main Camera").camera.fieldOfView = initialCameraFOV;
		}
	}
	
	void ZoomCamera ()
	{
		float scroll = CrossPlatformInput.GetAxis ("Mouse ScrollWheel");
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
