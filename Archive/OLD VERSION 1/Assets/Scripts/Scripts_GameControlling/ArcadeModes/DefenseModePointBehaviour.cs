using UnityEngine;
using System.Collections;

public class DefenseModePointBehaviour : MonoBehaviour
{
	private GameObject flagPole;
	private bool flagFound = false;

	void Update ()
	{
		if (!flagFound)
		{
			flagPole = GameObject.Find ("trigger_flag_pole(Clone)");
			if (flagPole != null)
			{
				flagFound = true;
			}
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == Tags.npcTank && !SwitchBox.isClient)
		{
			flagPole.rigidbody.isKinematic = false;
			Invoke ("DetachFlag", 2);

			((GC_Defensemode)GameController.acces).SetPointCapture ();
		}
	}

	void DetachFlag ()
	{
		SphereCollider[] flagAttachments = flagPole.GetComponents <SphereCollider>();
		
		for (int i = 0; i < flagAttachments.Length; i++)
		{
			flagAttachments[i].enabled = false;
		}
	}
}
