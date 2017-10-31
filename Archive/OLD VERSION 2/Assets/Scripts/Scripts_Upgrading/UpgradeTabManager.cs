using UnityEngine;
using System.Collections;

public class UpgradeTabManager : MonoBehaviour
{
	public Material activated;
	public Material deactivated;

	void Start ()
	{
		SetActiveLeft (UpgradeManagerScreens.acces.body);
		SetActiveRight (UpgradeManagerScreens.acces.primaryUpgrades);
	}

	void Update ()
	{
		switch (transform.name)
		{
		case "body_tab":
			if (UpgradeManagerScreens.acces.body.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "primary1_tab":
			if (UpgradeManagerScreens.acces.primary1.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "primary2_tab":
			if (UpgradeManagerScreens.acces.primary2.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "secondary_tab":
			if (UpgradeManagerScreens.acces.secondary.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;

		case "primaryU_tab":
			if (UpgradeManagerScreens.acces.primaryUpgrades.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "secondaryU_tab":
			if (UpgradeManagerScreens.acces.secondaryUpgrades.activeSelf)
			{
				gameObject.GetComponent <MeshRenderer>().material = activated;
			}
			else
			{
				gameObject.GetComponent <MeshRenderer>().material = deactivated;
			}
			break;
		case "upgrades_tab":
			if (UpgradeManagerScreens.acces.generalUpgrades.activeSelf)
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
			SetActiveLeft (UpgradeManagerScreens.acces.body);
			break;
		case "primary1_tab":
			SetActiveLeft (UpgradeManagerScreens.acces.primary1);
			break;
		case "primary2_tab":
			SetActiveLeft (UpgradeManagerScreens.acces.primary2);
			break;
		case "secondary_tab":
			SetActiveLeft (UpgradeManagerScreens.acces.secondary);
			break;

		case "primaryU_tab":
			SetActiveRight (UpgradeManagerScreens.acces.primaryUpgrades);
			break;
		case "secondaryU_tab":
			SetActiveRight (UpgradeManagerScreens.acces.secondaryUpgrades);
			break;
		case "upgrades_tab":
			SetActiveRight (UpgradeManagerScreens.acces.generalUpgrades);
			break;
			
		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	private void SetActiveLeft (GameObject things)
	{
		UpgradeManagerScreens.acces.body.SetActive (false);
		UpgradeManagerScreens.acces.primary1.SetActive (false);
		UpgradeManagerScreens.acces.primary2.SetActive (false);
		UpgradeManagerScreens.acces.secondary.SetActive (false);

		things.SetActive (true);
	}

	private void SetActiveRight (GameObject things)
	{
		UpgradeManagerScreens.acces.generalUpgrades.SetActive (false);
		UpgradeManagerScreens.acces.primaryUpgrades.SetActive (false);
		UpgradeManagerScreens.acces.secondaryUpgrades.SetActive (false);
		
		things.SetActive (true);
	}
}
