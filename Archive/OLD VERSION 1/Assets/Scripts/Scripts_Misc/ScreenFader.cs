using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
	public float fadeSpeed;
	public bool sceneStarting = true;
	private bool sceneEnding = false;
	private string nextLevel;

	public void ChangeScene (string level)
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
		nextLevel = level;
		sceneStarting = false;
		sceneEnding = true;
		guiTexture.color = Color.clear;
		gameObject.SetActive (true);
	}
	
	public void SetClear ()
	{
		sceneStarting = false;
		guiTexture.color = Color.clear;
		gameObject.SetActive (false);
	}

	void Update ()
	{
		if (sceneStarting)
		{
			guiTexture.pixelInset = new Rect (0, 0, Screen.width, Screen.height);
			StartingScene ();
		}

		if (sceneEnding)
		{
			guiTexture.pixelInset = new Rect (0, 0, Screen.width, Screen.height);
			EndingScene ();
		}
	}

	void StartingScene ()
	{
		FadeToClear ();
		if (guiTexture.color.a <= 0.05f)
		{
			SetClear ();
		}
	}

	void EndingScene ()
	{
		FadeToBlack ();
		if (guiTexture.color.a >= 0.95f)
		{
			sceneEnding = false;
			guiTexture.color = Color.black;
			if (nextLevel != "none")
			{
				Application.LoadLevel (nextLevel);
			}
		}
	}

	void FadeToClear ()
	{
		guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	void FadeToBlack ()
	{
		guiTexture.color = Color.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
}
