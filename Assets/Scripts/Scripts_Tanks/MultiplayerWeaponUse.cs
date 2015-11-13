using UnityEngine;
using System.Collections;

public class MultiplayerWeaponUse : MonoBehaviour
{
	public GameObject standardShell;
	public GameObject speedShell;

	public GameObject proxMine;
	public GameObject fieldMine;
	
	public GameObject shellSound;
	public GameObject mineSound;

	private GameObject[] mineList;

	[RPC] public void SetMineList (int amount)
	{
		mineList = new GameObject[amount];
	}

	[RPC] public void FirePrimaryA (Vector3 setPosition, Quaternion setRotation, string setShooter, int setMaxBounces, float setSpeed)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Instantiate (shellSound, Vector3.zero, Quaternion.identity);
			GameObject shellClone = SwitchBox.Instantiate (standardShell, setPosition, setRotation);
			
			ShellBehaviour shellProperties = shellClone.GetComponent <ShellBehaviour>();
			shellProperties.Initialise (setMaxBounces, setSpeed, setShooter);
		}
	}

	[RPC] public void FirePrimaryB (Vector3 setPosition, Quaternion setRotation, string setShooter, int setMaxBounces)
	{
		if (SwitchBox.isHost)
		{
			SwitchBox.Instantiate (shellSound, Vector3.zero, Quaternion.identity);
			GameObject shellClone = SwitchBox.Instantiate (speedShell, setPosition, setRotation);
			
			ShellBehaviour shellProperties = shellClone.GetComponent <ShellBehaviour>();
			shellProperties.Initialise (setMaxBounces, 20, setShooter);
		}
	}

	[RPC] public void FireSecondaryA (Vector3 setPosition, Quaternion setRotation, string setShooter, string setTeam, float setRadius)
	{
		if (SwitchBox.isHost)
		{
			for (int i = 0; i < mineList.Length; i++)
			{
				if (mineList[i] == null)
				{
					RaycastHit hit;
					if (Physics.Raycast (new Vector3 (transform.position.x, 2.0f, transform.position.z), Vector3.down, out hit))
					{
						if (hit.transform.gameObject.tag != Tags.mine)
						{
							SwitchBox.Instantiate (mineSound, Vector3.zero, Quaternion.identity);
							mineList[i] = SwitchBox.Instantiate (proxMine, new Vector3 (setPosition.x, 0, setPosition.z), setRotation);

							MineBehaviour mineProperties = mineList[i].GetComponent <MineBehaviour>();
							mineProperties.Initialise (setRadius, 1, setShooter, setTeam, WeaponUpgradeList.SecondaryWeaponNames()[0]);
							
							break;
						}
					}
				}
			}
		}
	}

	[RPC] public void FireSecondaryB (Vector3 setPosition, Quaternion setRotation, string setShooter, float setRadius, float setDuration)
	{
		if (SwitchBox.isHost)
		{
			for (int i = 0; i < mineList.Length; i++)
			{
				if (mineList[i] == null)
				{
					RaycastHit hit;
					if (Physics.Raycast (new Vector3 (transform.position.x, 2.0f, transform.position.z), Vector3.down, out hit))
					{
						if (hit.transform.gameObject.tag != Tags.mine)
						{
							SwitchBox.Instantiate (mineSound, Vector3.zero, Quaternion.identity);
							mineList[i] = SwitchBox.Instantiate (fieldMine, new Vector3 (setPosition.x, 0, setPosition.z), setRotation);
							
							MineBehaviour mineProperties = mineList[i].GetComponent <MineBehaviour>();
							mineProperties.Initialise (setRadius, setDuration, setShooter, "", WeaponUpgradeList.SecondaryWeaponNames()[1]);
							
							break;
						}
					}
				}
			}
		}
	}
}
