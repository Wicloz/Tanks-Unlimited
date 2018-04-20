using UnityEngine;
using System.Collections;

public class GameLoadingScreen : MonoBehaviour
{
	void OnGUI()
	{
		if (!GameStates.acces.gameStarting && SwitchBox.isServer)
		{
			if(GUI.Button(new Rect((Screen.width / 2) - 75, (Screen.height / 2) - 25, 150, 50), "Start Game"))
			{
				GameStates.acces.StartGame ();
			}
		}
	}
}
