using UnityEngine;
using System.Collections;

public class LoadingScreen : PersistentSingleton<LoadingScreen>
{
	public float loadingSpeed;

	public Camera loadingCam;
	public Material loadingMat;
	public MeshCollider loadingCol;

	private float alpha = 0;

	private string fadeState = "idle";
	private string nextLevel = "none";

	public bool isIdle
	{
		get
		{
			return fadeState == "idle";
		}
	}

	private GameObject menuManager;
	private bool[] guiState = new bool[5];

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
			StartLoadingFrom ();
		}
	}

	public void StartLoadingTo ()
	{
		if (fadeState != "to")
		{
			loadingCam.enabled = true;
			loadingCol.enabled = true;
			alpha = 0;
			fadeState = "to";
			DisableGui ();
		}
	}

	public void StartLoadingFrom ()
	{
		if (fadeState != "from")
		{
			loadingCam.enabled = true;
			loadingCol.enabled = false;
			alpha = 1;
			fadeState = "from";
			DisableGui ();
		}
	}

	public void StopLoading ()
	{
		loadingCam.enabled = false;
		fadeState = "idle";
		nextLevel = "none";
		EnableGui ();
	}

	public void QueLevelChange (string level)
	{
		nextLevel = level;
		StartLoadingTo ();
	}

	private void DisableGui ()
	{
		if (menuManager != null)
		{
			guiState[0] = menuManager.GetComponent<JoinGameMenu>().isEnabled;
			menuManager.GetComponent<JoinGameMenu>().isEnabled = false;

			guiState[1] = menuManager.GetComponent<HostGameMenu>().isEnabled;
			menuManager.GetComponent<HostGameMenu>().isEnabled = false;

			guiState[2] = menuManager.GetComponent<GameOptionEditor>().isEnabled;
			menuManager.GetComponent<GameOptionEditor>().isEnabled = false;
		}

		if (GameController.acces != null)
		{
			guiState[3] = GameController.acces.gameOverItems.activeSelf;
			GameController.acces.gameOverItems.SetActive (false);

			guiState[4] = GameController.acces.stageCompleteItems.activeSelf;
			GameController.acces.stageCompleteItems.SetActive (false);
		}
	}

	private void EnableGui ()
	{
		if (menuManager != null)
		{
			menuManager.GetComponent<JoinGameMenu>().isEnabled = guiState[0];
			menuManager.GetComponent<HostGameMenu>().isEnabled = guiState[1];
			menuManager.GetComponent<GameOptionEditor>().isEnabled = guiState[2];
		}

		if (GameController.acces != null)
		{
			GameController.acces.gameOverItems.SetActive (guiState[3]);
			GameController.acces.stageCompleteItems.SetActive (guiState[4]);
		}
	}

	void Update ()
	{
		menuManager = GameObject.Find ("MainMenuManager");

		switch (fadeState)
		{
		case "from":
			alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * loadingSpeed);
			if (alpha < 0.01f)
			{
				alpha = 0;
				StopLoading ();
			}
			break;

		case "to":
			alpha = Mathf.Lerp(alpha, 1, Time.deltaTime * loadingSpeed);
			if (alpha > 0.99f)
			{
				alpha = 1;
				fadeState = "idle";
			}
			break;

		case "idle":
			if (nextLevel != "none")
			{
				string tempLevel = nextLevel;
				nextLevel = "none";
				Application.LoadLevel (tempLevel);
				StartLoadingFrom ();
			}
			break;
		}

		loadingMat.color = new Color(loadingMat.color.r, loadingMat.color.g, loadingMat.color.b, alpha);
	}
}
