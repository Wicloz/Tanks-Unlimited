using UnityEngine;
using System.Collections;

public class UpgradeTabManager : MonoBehaviour
{
	public Material activated;
	public Material deactivated;

	public GameObject tracks;
	public GameObject turret;
	public GameObject shells;

	public GameObject mines;
	public GameObject supers;

	void Start ()
	{
		SetActive (tracks);
	}

	void Update ()
	{
		switch (transform.name)
		{
		case "body_tab":
			if (tracks.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "turret_tab":
			if (turret.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "shells_tab":
			if (shells.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;

		case "mines_tab":
			if (mines.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		}
	}

	void OnMouseDown ()
	{
		switch (transform.name)
		{
		case "body_tab":
			SetActive (tracks);
			break;
		case "turret_tab":
			SetActive (turret);
			break;
		case "shells_tab":
			SetActive (shells);
			break;
		case "mines_tab":
			SetActive (mines);
			break;
			
		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	void SetActive (GameObject things)
	{
		tracks.SetActive (false);
		turret.SetActive (false);
		shells.SetActive (false);
		mines.SetActive (false);

		things.SetActive (true);
	}
}
