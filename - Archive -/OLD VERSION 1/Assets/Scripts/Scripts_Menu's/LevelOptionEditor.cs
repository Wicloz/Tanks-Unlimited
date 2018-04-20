using UnityEngine;
using System.Collections;
using System;

public class LevelOptionEditor : MenuLayoutHelper
{
	public static LevelOptionEditor acces;

	public bool isEnabled = false;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		ReadValues ();

		enemyTanksFloat = enemyTanks;
		difficultyFloat = difficulty;
	}

	void OnDestroy ()
	{
		StoreValues ();
	}

	void OnGUI ()
	{
		if (isEnabled)
		{
			//Set Starting Values
			float start = Screen.height / 7f;

			float startY = start;
			float endY = Screen.height - start;
			float startX1 = Screen.width / 3.6f;
			float startX2 = Screen.width / 1.33f;

			//Obstacle Density
			GUI.Box(new Rect(startX1 - 5, startY, 160, 45), "Obstacle Density:");
			chance = GUI.HorizontalSlider (new Rect (startX1, startY + 25, 150, 30), chance, 0.01f, 0.08f);
			GUI.Label (new Rect (startX1 + 160, startY + 20, 100, 30), chanceString);

			//Enemy Tanks
			GUI.Box(new Rect(startX1 - 5, startY + 50, 160, 45), "Enemy Tanks:");
			enemyTanksFloat = GUI.HorizontalSlider (new Rect (startX1, startY + 25 + 50, 150, 30), enemyTanksFloat, 2, 6);
			GUI.Label (new Rect (startX1 + 160, startY + 20 + 50, 100, 30), enemyTanksString);

			//Diffuculty
			GUI.Box(new Rect(startX1 - 5, startY + 100, 160, 45), "difficulty:");
			difficultyFloat = GUI.HorizontalSlider (new Rect (startX1, startY + 25 + 100, 150, 30), difficultyFloat, 1, 4);

			GUI.Box(new Rect(startX2 - 5, startY, 160, 45), "difficulty:");
			difficultyFloat = GUI.HorizontalSlider (new Rect (startX2, startY + 25, 150, 30), difficultyFloat, 1, 4);

			GUI.Label (new Rect (startX1 + 160, startY + 20 + 100, 100, 30), difficultyString);
			GUI.Label (new Rect (startX2 + 160, startY + 20, 100, 30), difficultyString);

			//First Person
			firstPerson = GUI.Toggle (new Rect (startX1, endY - 20, 90, 20), firstPerson, "First Person");
			firstPerson = GUI.Toggle (new Rect (startX2, endY - 20, 90, 20), firstPerson, "First Person");

			// CoOp
			coOp = GUI.Toggle (new Rect (startX1, endY - 45, 90, 20), coOp, "Co-Op");
			coOp = GUI.Toggle (new Rect (startX2, endY - 45, 90, 20), coOp, "Co-Op");
		}
	}

	public void ReadValues ()
	{
		chance = GlobalValues.acces.chance;
		enemyTanks = GlobalValues.acces.enemyTanks;
		difficulty = GlobalValues.acces.difficulty;
		firstPerson = GlobalValues.acces.firstPerson;
		coOp = GlobalValues.acces.coOp;
	}
	
	public void StoreValues ()
	{
		GlobalValues.acces.chance = chance;
		GlobalValues.acces.enemyTanks = enemyTanks;
		GlobalValues.acces.difficulty = difficulty;
		GlobalValues.acces.firstPerson = firstPerson;
		GlobalValues.acces.coOp = coOp;
		
		if (GlobalValues.acces.multiplayer)
		{
			coOp = false;
		}
		
		if (coOp)
		{
			GlobalValues.acces.playerAmount = 2;
		}
		else
		{
			GlobalValues.acces.playerAmount = 1;
		}
	}
}
