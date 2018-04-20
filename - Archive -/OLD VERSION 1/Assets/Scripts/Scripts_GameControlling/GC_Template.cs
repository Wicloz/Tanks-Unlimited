using UnityEngine;
using System.Collections;

public class GC_Template : GameController
{
	new void Update ()
	{
		base.Update ();

		if (GameStates.acces.endInitialised && !SwitchBox.isClient)
		{
			GameDoneCheck ();
		}
	}
	
	// Initialisation
	public override void ChildInitialise ()
	{
	}
	
	// Game Controlling
	void GameDoneCheck ()
	{
	}
	
	// Game Done
	public override void ChildGameOver ()
	{
		GameOverText.acces.done = true;
	}
	
	public override void ChildStageCleared ()
	{
	}
	
	// Functions
	public override void OnNpcTankDestroy ()
	{
		base.OnNpcTankDestroy ();
	}
	
	// Score GUI
	public override void ChildUpdateScore ()
	{
	}
}
