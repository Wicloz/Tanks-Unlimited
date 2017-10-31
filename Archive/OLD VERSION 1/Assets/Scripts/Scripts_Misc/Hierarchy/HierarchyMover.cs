using UnityEngine;
using System.Collections;

public class HierarchyMover : MonoBehaviour
{
	public bool isMine;
	public bool isShell;
	public bool isProbe;
	public bool isTank;
	public bool isObject;
	public bool isEffect;

	void OnEnable ()
	{
		if (isMine)
		{
			transform.parent = HierarchyManager.mineParent.transform;
		}
		else if (isShell)
		{
			transform.parent = HierarchyManager.shellParent.transform;
		}
		else if (isProbe)
		{
			transform.parent = HierarchyManager.probeParent.transform;
		}
		else if (isTank)
		{
			transform.parent = HierarchyManager.tankParent.transform;
		}
		else if (isObject)
		{
			transform.parent = HierarchyManager.objectParent.transform;
		}
		else if (isEffect)
		{
			transform.parent = HierarchyManager.effectParent.transform;
		}
	}
}
