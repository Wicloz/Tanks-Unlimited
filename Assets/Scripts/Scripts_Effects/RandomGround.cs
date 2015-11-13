using UnityEngine;
using System.Collections;

public class RandomGround : MonoBehaviour
{
	public Material[] groundMat;
	
	void Start ()
	{
		int random = (int)Random.Range (-0.5f, groundMat.Length - 0.5f);
		for (int i = 0; i < groundMat.Length; i ++)
		{
			if (i == random)
			{
				gameObject.GetComponent <MeshRenderer>().material = groundMat[i];
			}
		}
	}
}
