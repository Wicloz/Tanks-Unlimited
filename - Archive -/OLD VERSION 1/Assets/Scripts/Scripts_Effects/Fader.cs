using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
	void Start ()
	{
		if (guiText != null)
		{
			guiText.color = new Vector4 (guiText.color.r, guiText.color.g, guiText.color.b, 0);
		}
		
		if (guiTexture != null)
		{
			guiTexture.color = new Vector4 (guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, 0);
		}
	}

	void Update ()
	{
		if (guiText != null)
		{
			guiText.color = Vector4.Lerp (guiText.color, new Vector4 (guiText.color.r, guiText.color.g, guiText.color.b, 255), Time.deltaTime / 100);
		}

		if (guiTexture != null)
		{
			guiTexture.color = Vector4.Lerp (guiTexture.color, new Vector4 (guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, 255), Time.deltaTime / 100);
		}
	}
}
