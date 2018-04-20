using UnityEngine;
using System.Collections;

public class MultiplayerWeaponUse : MonoBehaviour
{
	public GameObject shell;
	public GameObject mine;
	
	public GameObject shellSound;
	
	public GameObject mineSound;
	private GameObject[] mineList;

	[RPC] public void SetMineList (int amount)
	{
		mineList = new GameObject[amount];
	}

	[RPC] public void InstantiateShell (Vector3 setPosition, Quaternion setRotation, int setMaxBounces, float setSpeed, string setShooter)
	{
		if (!SwitchBox.isClient)
		{
			SwitchBox.Instantiate (shellSound, Vector3.zero, Quaternion.identity);
			
			GameObject shellClone = SwitchBox.Instantiate (shell, setPosition, setRotation);
			
			ShellBehaviour shellProperties = shellClone.GetComponent <ShellBehaviour>();
			
			shellProperties.SetProperties (setMaxBounces, setSpeed, setShooter);
		}
	}

	[RPC] public void InstantiateMine (Vector3 setPosition, Quaternion setRotation, float setRadius, string setShooter)
	{
		if (!SwitchBox.isClient)
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
							
							mineList[i] = SwitchBox.Instantiate (mine, new Vector3 (setPosition.x, 0, setPosition.z), setRotation);

							MineDestroyer mineProperties = mineList[i].GetComponent <MineDestroyer>();
							
							mineProperties.SetProperties(setRadius, setShooter);
							
							break;
						}
					}
				}
			}
		}
	}
}
