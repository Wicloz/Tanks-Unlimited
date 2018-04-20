using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public static PauseMenu acces;
	
	private float musicVolume;
	public bool paused = false;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		musicVolume = GlobalValues.acces.musicVolume;
	}

	void Update ()
	{
		if (GameStates.acces.endInitialised)
		{
			if (CrossPlatformInput.GetButtonDown ("Pause Menu") && !paused && !GameStates.acces.gameDone && !GameStates.acces.stageCleared)
			{
				Pause ();
			}

			else if (CrossPlatformInput.GetButtonDown ("Pause Menu") && paused && !GameStates.acces.gameDone && !GameStates.acces.stageCleared)
			{
				Resume ();
			}
		}
	}

	void Pause ()
	{
		paused = true;
		TimeScale (0);
		
		Screen.showCursor = true;
	}

	void Resume ()
	{
		paused = false;
		TimeScale (1);
		
		if (!GlobalValues.acces.firstPerson)
		{
			Screen.showCursor = false;
		}
	}

	void OnGUI ()
	{
		if (paused)
		{
			int Width = 150;
			int StartingX = (Screen.width / 2) - (Width / 2);
			int StartingY = (Screen.height / 2) - (134 / 2);
			
			GUI.Box(new Rect(StartingX - 5, StartingY, Width + 10, 92), "Game Paused");
			
			if(GUI.Button(new Rect(StartingX, StartingY + 30, Width, 24), "Resume Playing"))
			{
				Resume ();
			}
			
			if(GUI.Button(new Rect(StartingX, StartingY + 60, Width, 24), "Exit to Main Menu"))
			{
				if (SwitchBox.isServer)
				{
					networkView.RPC ("ExitMainMenu", RPCMode.AllBuffered);
				}
				else
				{
					ExitMainMenu ();
				}
			}
			
			GUI.Box(new Rect(StartingX - 5, StartingY + 100, Width + 10, 34), "Music Volume");
			musicVolume = GUI.HorizontalSlider (new Rect (StartingX, StartingY + 120, Width, 30), musicVolume, 0.0f, 1.0f);

			GlobalValues.acces.musicVolume = musicVolume;
		}
	}

	public void TimeScale (float scale)
	{
		if (GameStates.acces.GameOver || GameStates.acces.stageCleared || !GlobalValues.acces.multiplayer)
		{
			if (scale == 0)
			{
				GameObject flag = GameObject.Find ("part_flag_cloth");
				if (flag != null)
				{
					flag.GetComponent <InteractiveCloth>().enabled = false;
				}
			}
			
			if (scale != 0)
			{
				GameObject flag = GameObject.Find ("part_flag_cloth");
				if (flag != null)
				{
					flag.GetComponent <InteractiveCloth>().enabled = true;
				}
			}

			Time.timeScale = 1f * scale;
			Time.fixedDeltaTime = 0.02f * scale;
		}
	}

	[RPC] void ExitMainMenu ()
	{
		paused = false;
		TimeScale (1);

		GameObject playerTank = PlayerManager.acces.playerTanks [PlayerManager.acces.thisSlot];
		if (playerTank != null)
		{
			Destructor.PlayerTank (playerTank);
		}
		
		StartCoroutine (GameController.acces.GameOverSet());
	}
}
