using UnityEngine;
using System.Collections;

public class HostGameMenu : MenuLayoutHelper
{
	public static HostGameMenu acces;
	
	public bool isEnabled = false;

	private string gameMode = "";
	public string passWord = "";

	public const string gameName = "WilcosTanks";
	private string roomName = "null";
	private bool serverStarted = false;

	void Awake ()
	{
		acces = this;
	}

	private Vector2 scrollPosition = Vector2.zero;

	void OnGUI ()
	{
		if (isEnabled)
		{
			// Boxes
			GUI.Box(new Rect(boxStartX1, boxStartY, boxWidth, boxEndY - boxStartY), "Game Modes");
			GUI.Box(new Rect(boxStartX2, boxStartY, boxWidth, boxEndY - boxStartY), "General Settings");
			GUI.Box(new Rect(boxStartX3, boxStartY, boxWidth, boxEndY - boxStartY), "Multiplayer Settings");

			// First Section
			scrollPosition = GUI.BeginScrollView(new Rect(startX1, startY, width, boxEndY - startY - itemSpacing), scrollPosition, new Rect(0, 0, width - itemSpacing, 1000));

			GUI.Label (new Rect (0, 0, 200, 22), "-- Arcade Modes");
			if (GUI.Button(new Rect(0, 30, width - itemSpacing, 22), "Battle Arena"))
			{
				gameMode = "Play_InfiniMode";
			}
			if (GUI.Button(new Rect(0, 60, width - itemSpacing, 22), "Defend the Flag"))
			{
				gameMode = "Play_DefenseMode";
			}

			GUI.EndScrollView();

			if (gameMode != "")
			{
			// Second Section

				//Diffuculty
				GUI.Box(new Rect(startX2, startY, 160, 45), "difficulty:");
				difficultyFloat = GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25, 150, 30), difficultyFloat, 1, 4);
				GUI.Label (new Rect (startX2 + 165, startY + 20, 100, 22), difficultyString);
				
				if (gameMode == "Play_InfiniMode")
				{
					//Obstacle Density
					GUI.Box(new Rect(startX2, startY + 50, 160, 45), "Obstacle Density:");
					chance = GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25 + 50, 150, 30), chance, 0.01f, 0.08f);
					GUI.Label (new Rect (startX2 + 165, startY + 20 + 50, 100, 30), chanceString);
					
					//Enemy Tanks
					GUI.Box(new Rect(startX2, startY + 100, 160, 45), "Enemy Tanks:");
					enemyTanksFloat = GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25 + 100, 150, 30), enemyTanksFloat, 2, 6);
					GUI.Label (new Rect (startX2 + 165, startY + 20 + 100, 100, 30), enemyTanksString);
				}

			// Third Section
				GUI.Label (new Rect(startX3, startY, 200, 22), "Gamemode: " + FilterName(gameMode));

				GUI.Label (new Rect(startX3, startY + 30, 200, 22), "Password:");
				passWord = GUI.TextField(new Rect(startX3, startY + 52, width, 22), passWord);

			// Start Button
				if (GUI.Button(new Rect(boxStartX3 + halfBox - 100, endY - 40, 200, 40), "Start Game"))
				{
					MainMenuBehaviour.acces.disableJoinMenu = true;

					switch (gameMode)
					{
					case "Play_InfiniMode":
						GameModeList.InfiniMode (true, null);
						break;
					case "Play_DefenseMode":
						GameModeList.DefenseMode (true, null);
						break;
					default:
						Debug.LogError ("GameMode Undefined");
						break;
					}
				}
			}
		}
	}

	// Hosting a server
	public void StartServer()
	{
		roomName = GlobalValues.acces.userName + "'s Room";
		
		Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName, roomName, SerialiseComment());
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		// On Server Started !!
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
		{
			if (!serverStarted)
			{
				serverStarted = true;
				Debug.LogError  ("Server Started");
				MainMenuBehaviour.acces.SwitchTrack ("none");
				Application.LoadLevel (GlobalValues.acces.gameMode);
			}
		}
	}

	// Serialising the Comment
	[System.Serializable]
	public class CommentData
	{
		public string gameMode;
		public string passWord;
		public string hostName;
		public string difficulty;
	}
	
	void SetProperties (CommentData commentData)
	{
		commentData.gameMode = GlobalValues.acces.gameMode;
		commentData.passWord = HostGameMenu.acces.passWord;
		commentData.hostName = GlobalValues.acces.userName;
		commentData.difficulty = MenuLayoutHelper.difficultyString;
	}
	
	string SerialiseComment ()
	{
		CommentData commentData = new CommentData ();
		SetProperties (commentData);
		
		return SerializerHelper.SerializeToString (commentData);
	}
}
