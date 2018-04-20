using UnityEngine;
using System.Collections;

public class EscapeMenu : PersistentSingleton<EscapeMenu>
{
	public GameObject menuParent;

	public InstantGuiButton mainMenuButton;
	public InstantGuiButton optionsButton;
	public InstantGuiElement returnButton1;
	public InstantGuiButton returnButton2;

	public bool isOpened
	{
		get
		{
			return menuParent.activeSelf;
		}
	}

	public bool shouldPause
	{
		get
		{
			return !SwitchBox.isServerOn && (OptionsManager.acces.isOpened || isOpened);
		}
	}

	public static bool blockInput
	{
		get
		{
			return (OptionsManager.acces.isOpened || EscapeMenu.acces.isOpened || MultiplayerManager.acces.gameState != GameState.Running);
		}
	}

	public void OpenWindow ()
	{
		menuParent.SetActive (true);
	}

	public void CloseWindow ()
	{
		menuParent.SetActive (false);
	}

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	void Update ()
	{
		GameObject[] delete = GameObject.FindGameObjectsWithTag (Tags.deleteUI);
		foreach (GameObject go in delete)
		{
			Destroy (go);
		}

		if (MultiplayerManager.acces.gameState != GameState.Stopped && MultiplayerManager.acces.gameState != GameState.Loading)
		{
			if (Input.GetButtonDown ("Pause Menu") && !isOpened)
			{
				OpenWindow ();
			}
			
			else if (Input.GetButtonDown ("Pause Menu") && isOpened)
			{
				CloseWindow ();
			}
		}

		else if (isOpened)
		{
			CloseWindow ();
		}

		// Buttons
		if (mainMenuButton.pressed)
		{
			StateManager.acces.EndGame("Quitting");
			CloseWindow();
			mainMenuButton.pressed = false;
		}

		if (optionsButton.pressed)
		{
			OptionsManager.acces.OpenWindow("EscapeMenu");
			CloseWindow();
			optionsButton.pressed = false;
		}

		if (returnButton1.pressed)
		{
			CloseWindow();
			returnButton1.pressed = false;
		}
		if (returnButton2.pressed)
		{
			CloseWindow();
			returnButton2.pressed = false;
		}
	}
}
