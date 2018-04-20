using UnityEngine;
using System.Collections;

public static class GameModeList
{
	private static void ResetValues ()
	{
		RunRecorder.acces.level = 1;
		RunRecorder.acces.runScore = 0;
		RunRecorder.acces.runTime = 0;
	}

	public static void InfiniMode (bool multiplayer, HostData hostData)
	{
		MainMenuBehaviour.acces.ChangeLoading (true);
		MainMenuBehaviour.acces.disableLevelOptions = true;
		MainMenuBehaviour.acces.disableJoinMenu = true;

		if (multiplayer)
		{
			GlobalValues.acces.lives = 0;
		}
		else
		{
			GlobalValues.acces.lives = 1;
		}
		GlobalValues.acces.multiplayer = multiplayer;
		LevelOptionEditor.acces.StoreValues();
		ResetValues ();
		GlobalValues.acces.gameMode = "Play_InfiniMode";

		if (!multiplayer)
		{
			MainMenuBehaviour.screenFader.ChangeScene ("Play_InfiniMode");
		}
		else if (hostData == null)
		{
			HostGameMenu.acces.StartServer ();
		}
		else
		{
			Network.Connect (hostData);
		}
	}

	public static void DefenseMode (bool multiplayer, HostData hostData)
	{
		MainMenuBehaviour.acces.ChangeLoading (true);
		MainMenuBehaviour.acces.disableLevelOptions = true;
		MainMenuBehaviour.acces.disableJoinMenu = true;

		GlobalValues.acces.lives = -1;
		GlobalValues.acces.multiplayer = multiplayer;
		LevelOptionEditor.acces.StoreValues ();
		ResetValues ();
		GlobalValues.acces.gameMode = "Play_DefenseMode";

		if (!multiplayer)
		{
			MainMenuBehaviour.screenFader.ChangeScene ("Play_DefenseMode");
		}
		else if (hostData == null)
		{
			HostGameMenu.acces.StartServer ();
		}
		else
		{
			Network.Connect (hostData);
		}
	}
}
