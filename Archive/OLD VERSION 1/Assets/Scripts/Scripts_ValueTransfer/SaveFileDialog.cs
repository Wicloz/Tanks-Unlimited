using UnityEngine;
using System.Collections;

public class SaveFileDialog : MonoBehaviour
{
	public static SaveFileDialog acces;

	private bool changingUser = false;
	private string newUser;

	void Awake ()
	{
		acces = this;
	}

	public void ChangeUser ()
	{
		newUser = GlobalValues.acces.userName;
		changingUser = true;
	}

	void OnGUI ()
	{
		if (changingUser)
		{
			int Width = 150;
			int StartingX = 15;
			int StartingY = 10;
			
			GUI.Box(new Rect(StartingX - 5, StartingY, Width + 10, 92), "Change User");

			newUser = GUI.TextField(new Rect(StartingX, StartingY + 30, Width, 24), newUser);

			if(GUI.Button(new Rect(StartingX, StartingY + 60, Width, 24), "Load Game"))
			{
				GlobalValues.acces.SaveGame ();
				GlobalValues.acces.userName = newUser;
				GlobalValues.acces.LoadGame ();

				changingUser = false;
			}
		}
	}
}
