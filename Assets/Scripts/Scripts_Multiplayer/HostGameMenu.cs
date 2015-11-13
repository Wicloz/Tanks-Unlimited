using UnityEngine;
using System.Collections;

public class HostGameMenu : MonoBehaviour
{
	public static HostGameMenu acces;
	public bool isEnabled = false;

	private string gamemode = "";
	private Vector2 scrollPosition = Vector2.zero;

	public float modifier1;
	public float modifier2;
	
	private float boxWidth;
	private float halfBox;
	private float width;
	private float start;
	private float boxSpacing;
	private float itemSpacing;
	private float boxStartY;
	private float boxEndY;
	private float startY;
	private float endY;
	private float boxStartX1;
	private float boxStartX2;
	private float boxStartX3;
	private float startX1;
	private float startX2;
	private float startX3;
	
	void Update ()
	{
		if (isEnabled)
		{
			boxSpacing = Screen.height / modifier1;
			itemSpacing = Screen.height / modifier2;
			
			boxWidth = (Screen.width - boxSpacing * 4) / 3;
			halfBox = boxWidth / 2;
			width = boxWidth - itemSpacing * 2;
			start = boxSpacing;
			
			boxStartY = start;
			boxEndY = Screen.height - start;
			
			boxStartX1 = boxSpacing;
			boxStartX2 = boxStartX1 + boxWidth + boxSpacing;
			boxStartX3 = boxStartX2 + boxWidth + boxSpacing;
			
			startX1 = boxStartX1 + itemSpacing;
			startX2 = boxStartX2 + itemSpacing;
			startX3 = boxStartX3 + itemSpacing;
			
			startY = boxStartY + itemSpacing + 20;
			endY = boxEndY - itemSpacing;
		}
	}

	void Awake ()
	{
		acces = this;
	}

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
				gamemode = "Play_InfiniMode";
			}
			if (GUI.Button(new Rect(0, 60, width - itemSpacing, 22), "Defend the Flag"))
			{
				gamemode = "Play_DefenseMode";
			}
			
			GUI.EndScrollView();

			if (gamemode != "")
			{
				// Second Section
				//Diffuculty
				GUI.Box(new Rect(startX2, startY, 160, 45), "difficulty:");
				MultiplayerManager.acces.gameSettings.difficulty = (int) GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25, 150, 30), (float)MultiplayerManager.acces.gameSettings.difficulty, 1, 4);
				GUI.Label (new Rect (startX2 + 165, startY + 20, 100, 22), MultiplayerManager.acces.gameSettings.difficulty.ToString());
				
				if (gamemode == "Play_InfiniMode")
				{
					//Obstacle Density
					GUI.Box(new Rect(startX2, startY + 50, 160, 45), "Obstacle Density:");
					MultiplayerManager.acces.gameSettings.objectDensity = (int) GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25 + 50, 150, 30), (float)MultiplayerManager.acces.gameSettings.objectDensity, 40, 100);
					GUI.Label (new Rect (startX2 + 165, startY + 20 + 50, 100, 30), (MultiplayerManager.acces.gameSettings.objectDensity / 10).ToString());
					
					//Enemy Tanks
					GUI.Box(new Rect(startX2, startY + 100, 160, 45), "Enemy Tanks:");
					MultiplayerManager.acces.gameSettings.enemyTanks = (int) GUI.HorizontalSlider (new Rect (startX2 + 5, startY + 25 + 100, 150, 30), (float)MultiplayerManager.acces.gameSettings.enemyTanks, 2, 6);
					GUI.Label (new Rect (startX2 + 165, startY + 20 + 100, 100, 30), MultiplayerManager.acces.gameSettings.enemyTanks.ToString());
				}
				
				// Third Section
				GUI.Label (new Rect(startX3, startY, 200, 22), "Gamemode: " + FilterName(gamemode));
				
				GUI.Label (new Rect(startX3, startY + 30, 200, 22), "Password:");
				MultiplayerManager.acces.passWord = GUI.TextField(new Rect(startX3, startY + 52, width, 22), MultiplayerManager.acces.passWord);
				
				// Start Button
				if (GUI.Button(new Rect(boxStartX3 + halfBox - 100, endY - 40, 200, 40), "Start Game"))
				{
					StateManager.acces.StartLevel (gamemode, true, true, null);
				}
			}
		}
	}

	private string FilterName (string name)
	{
		return name.
			Replace("Play_", "").
			Replace("InfiniMode", "Battle Arena").
			Replace("DefenseMode", "Defend the Flag");
	}
}
