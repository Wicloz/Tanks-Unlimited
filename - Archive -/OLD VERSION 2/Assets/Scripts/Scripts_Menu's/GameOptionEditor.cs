using UnityEngine;
using System.Collections;

public class GameOptionEditor : MonoBehaviour
{
	public static GameOptionEditor acces;
	public bool isEnabled = false;

	void Awake ()
	{
		acces = this;
	}

	void OnGUI ()
	{
		if (isEnabled)
		{
			//Set Starting Values
			float difference = Screen.height / 7f;
			float startY = difference;
			float endY = Screen.height - difference;
			float startX1 = Screen.width / 3.6f;
			float startX2 = Screen.width / 1.33f;

			//Obstacle Density
			GUI.Box(new Rect(startX1 - 5, startY, 160, 45), "Obstacle Density:");
			MultiplayerManager.acces.gameSettings.objectDensity = (int) GUI.HorizontalSlider (new Rect (startX1, startY + 25, 150, 30), (float)MultiplayerManager.acces.gameSettings.objectDensity, 40, 100);
			GUI.Label (new Rect (startX1 + 160, startY + 20, 100, 30), (MultiplayerManager.acces.gameSettings.objectDensity / 10).ToString());
			
			//Enemy Tanks
			GUI.Box(new Rect(startX1 - 5, startY + 50, 160, 45), "Enemy Tanks:");
			MultiplayerManager.acces.gameSettings.enemyTanks = (int) GUI.HorizontalSlider (new Rect (startX1, startY + 25 + 50, 150, 30), (float)MultiplayerManager.acces.gameSettings.enemyTanks, 2, 6);
			GUI.Label (new Rect (startX1 + 160, startY + 20 + 50, 100, 30), MultiplayerManager.acces.gameSettings.enemyTanks.ToString());
			
			//Diffuculty
			GUI.Box(new Rect(startX1 - 5, startY + 100, 160, 45), "difficulty:");
			MultiplayerManager.acces.gameSettings.difficulty = (int) GUI.HorizontalSlider (new Rect (startX1, startY + 25 + 100, 150, 30), (float)MultiplayerManager.acces.gameSettings.difficulty, 1, 4);
			
			GUI.Box(new Rect(startX2 - 5, startY, 160, 45), "difficulty:");
			MultiplayerManager.acces.gameSettings.difficulty = (int) GUI.HorizontalSlider (new Rect (startX2, startY + 25, 150, 30), (float)MultiplayerManager.acces.gameSettings.difficulty, 1, 4);
			
			GUI.Label (new Rect (startX1 + 160, startY + 20 + 100, 100, 30), MultiplayerManager.acces.gameSettings.difficulty.ToString());
			GUI.Label (new Rect (startX2 + 160, startY + 20, 100, 30), MultiplayerManager.acces.gameSettings.difficulty.ToString());
			
			//First Person
			ProfileManager.acces.player.firstPerson = GUI.Toggle (new Rect (startX1, endY - 20, 90, 20), ProfileManager.acces.player.firstPerson, "First Person");
			ProfileManager.acces.player.firstPerson = GUI.Toggle (new Rect (startX2, endY - 20, 90, 20), ProfileManager.acces.player.firstPerson, "First Person");
		}
	}
}
