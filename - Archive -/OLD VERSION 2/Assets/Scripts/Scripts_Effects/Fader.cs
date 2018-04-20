using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
	public float originalAlpha = 1;

	void OnEnable ()
	{
		if (GetComponent<GUIText>() != null)
		{
			originalAlpha = GetComponent<GUIText>().color.a;
			GetComponent<GUIText>().color = new Vector4 (GetComponent<GUIText>().color.r, GetComponent<GUIText>().color.g, GetComponent<GUIText>().color.b, 0);
		}
		
		if (GetComponent<GUITexture>() != null)
		{
			originalAlpha = GetComponent<GUITexture>().color.a;
			GetComponent<GUITexture>().color = new Vector4 (GetComponent<GUITexture>().color.r, GetComponent<GUITexture>().color.g, GetComponent<GUITexture>().color.b, 0);
		}
	}

	void Update ()
	{
		if (GetComponent<GUIText>() != null)
		{
			GetComponent<GUIText>().color = Vector4.Lerp (GetComponent<GUIText>().color, new Vector4 (GetComponent<GUIText>().color.r, GetComponent<GUIText>().color.g, GetComponent<GUIText>().color.b, originalAlpha), Time.deltaTime * (5 / originalAlpha));
		}

		if (GetComponent<GUITexture>() != null)
		{
			GetComponent<GUITexture>().color = Vector4.Lerp (GetComponent<GUITexture>().color, new Vector4 (GetComponent<GUITexture>().color.r, GetComponent<GUITexture>().color.g, GetComponent<GUITexture>().color.b, originalAlpha), Time.deltaTime * (5 / originalAlpha));
		}
	}
}
