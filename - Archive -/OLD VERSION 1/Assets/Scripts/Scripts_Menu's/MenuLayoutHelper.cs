using UnityEngine;
using System.Collections;
using System;

public class MenuLayoutHelper : MonoBehaviour
{
	public float modifier1;
	public float modifier2;

	protected float boxWidth;
	protected float halfBox;
	protected float width;
	protected float start;

	protected float boxSpacing;
	protected float itemSpacing;

	protected float boxStartY;
	protected float boxEndY;
	protected float startY;
	protected float endY;

	protected float boxStartX1;
	protected float boxStartX2;
	protected float boxStartX3;

	protected float startX1;
	protected float startX2;
	protected float startX3;

	// Level Option Editor
	protected static float chance;
	protected static int enemyTanks;
	protected static int difficulty;
	protected static bool firstPerson;
	public static bool coOp;
	
	protected static float enemyTanksFloat;
	protected static float difficultyFloat;
	protected static string enemyTanksString;
	public static string difficultyString;
	protected static string chanceString;

	protected void Update ()
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

		// Level Option Editor
		chanceString = Convert.ToString (Convert.ToInt32 (chance * 100000));

		enemyTanks = Convert.ToInt32 (enemyTanksFloat);
		enemyTanksString = Convert.ToString (enemyTanks);

		difficulty = Convert.ToInt32 (difficultyFloat);
		if (difficulty == 1)
		{
			difficultyString = "Easy";
		}
		if (difficulty == 2)
		{
			difficultyString = "Normal";
		}
		if (difficulty == 3)
		{
			difficultyString = "Hard";
		}
		if (difficulty == 4)
		{
			difficultyString = "Impossible";
		}
	}

	protected string FilterName (string name)
	{
		return name.
			Replace("Play_", "").
			Replace("InfiniMode", "Battle Arena").
			Replace("DefenseMode", "Defend the Flag");
	}
}
