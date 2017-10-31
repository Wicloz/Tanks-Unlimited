using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
	public float positionY;
	public AudioClip buttonSound;
	private float menuHeight = 59f;

	void OnMouseEnter ()
	{
		if (!gameObject.name.Contains ("Door") && !gameObject.name.Contains ("Sing"))
		{
			if (buttonSound.name != "Exit Button" || gameObject.name == "ExitGame")
			{
				transform.Translate (new Vector3(-0.1f, 0 ,-0.1f));
			}
		}
	}

	void OnMouseExit ()
	{
		if (!gameObject.name.Contains ("Door") && !gameObject.name.Contains ("Sing"))
		{
			if (buttonSound.name != "Exit Button" || gameObject.name == "ExitGame")
			{
				transform.Translate (new Vector3(0.1f, 0 ,0.1f));
			}
		}
	}

	void OnMouseDown ()
	{
		MainMenuBehaviour.acces.PlayButton (buttonSound);
	}

	void OnMouseUp ()
	{
		switch (transform.name)
		{
		case "SinglePlayerDoor":
			if (MainMenuBehaviour.acces.mainMenu)
			{
				StartCoroutine(SinglePlayer());
			}
			break;
		case "MultiPlayerDoor":
			if (MainMenuBehaviour.acces.mainMenu)
			{
				StartCoroutine(MultiPlayer());
			}
			break;
		case "TankEditorDoor":
			if (MainMenuBehaviour.acces.mainMenu)
			{
				StartCoroutine(TankEditor());
			}
			break;

		case "SinglePlayer":
			SinglePlayerButton ();
			break;
		case "MultiPlayer":
			MultiPlayerButton ();
			break;

		case "MainMenu":
			if (!MainMenuBehaviour.acces.mainMenu)
			{
				StartCoroutine(MainMenu());
			}
			break;

		case "ExitGame":
			GlobalValues.acces.SaveGame ();
			Application.Quit ();
			break;
		case "ChangeUserSing":
			SaveFileDialog.acces.ChangeUser ();
			break;

		case "Challenges":
			Challenges();
			break;
		case "GameModes":
			LevelSelection();
			break;
			
		case "InfiniteMode":
			if (LevelOptionEditor.acces.isEnabled)
			{
				InfiniteMode();
			}
			break;
		case "DefenseMode":
			if (LevelOptionEditor.acces.isEnabled)
			{
				DefenseMode();
			}
			break;

		case "JoinGame":
			JoinGame();
			break;
		case "HostGame":
			HostGame();
			break;

		default:
			Debug.LogError ("Button Undefined");
			break;
		}
	}

	// On Main Menu
	IEnumerator SinglePlayer ()
	{
		MainMenuBehaviour.acces.mainMenu = false;

		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -171);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
		StartCoroutine ("BlinkLight", "light_SP");

		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitBeforeDoor);
		yield return StartCoroutine (OpenDoor ());
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitAfterDoor);

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -70);

		yield return new WaitForSeconds (2);
		CloseDoor ();
	}

	void SinglePlayerButton ()
	{
		MainMenuBehaviour.acces.mainMenu = false;
		
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -70);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
	}

	IEnumerator MultiPlayer ()
	{
		MainMenuBehaviour.acces.mainMenu = false;

		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -171);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
		StartCoroutine ("BlinkLight", "light_MP");
		
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitBeforeDoor);
		yield return StartCoroutine (OpenDoor ());
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitAfterDoor);
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -70);

		yield return new WaitForSeconds (2);
		CloseDoor ();
	}

	void MultiPlayerButton ()
	{
		MainMenuBehaviour.acces.mainMenu = false;
		
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -70);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
	}

	IEnumerator TankEditor ()
	{
		MainMenuBehaviour.acces.mainMenu = false;

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (0, 56, -171);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
		StartCoroutine ("BlinkLight", "light_TE");

		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitBeforeDoor);
		yield return StartCoroutine (OpenDoor ());
		MainMenuBehaviour.acces.SwitchTrack ("editor");
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitAfterDoor);
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (0, 56, -70);

		yield return new WaitForSeconds (2);
		CloseDoor ();
	}

	IEnumerator OpenDoor ()
	{
		positionY = transform.localPosition.y;
		float currentTime = Time.time;

		float duration = 1;
		float increment = 0.02f / duration;

		while (Time.time <= currentTime + duration)
		{
			if (transform.localScale.y >= increment)
			{
				transform.localScale = new Vector3 (1, transform.localScale.y - increment, 1);
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - increment, transform.localPosition.z);
			}
			else if (transform.localScale.y < increment)
			{
				transform.localScale = new Vector3 (1, 0, 1);
			}

			yield return null;
		}
	}

	IEnumerator BlinkLight (string lightname)
	{
		GameObject lamp = GameObject.Find (lightname);
		float currentTime = Time.time;
		MainMenuBehaviour.acces.alarmSound.audio.Play ();

		while (true)
		{
			if (Time.time - currentTime <= 4)
			{
				MainMenuBehaviour.acces.alarmSound.audio.volume = Mathf.PingPong ((Time.time - currentTime) * 0.8f, 0.4f);
			}
			else
			{
				MainMenuBehaviour.acces.alarmSound.audio.Stop ();
			}

			lamp.light.intensity = Mathf.PingPong ((Time.time - currentTime) * 10, 4);
			yield return null;
		}
	}

	void CloseDoor ()
	{
		StopCoroutine ("BlinkLight");
		transform.parent.Find ("light_TE").light.intensity = 0;
		transform.parent.Find ("light_SP").light.intensity = 0;
		transform.parent.Find ("light_MP").light.intensity = 0;

		transform.localScale = new Vector3 (1, 1, 1);
		transform.localPosition = new Vector3 (transform.localPosition.x, positionY, transform.localPosition.z);
	}

	IEnumerator MainMenu ()
	{
		MainMenuBehaviour.acces.mainMenu = true;

		MainMenuBehaviour.acces.SwitchTrack ("menu");
		GlobalValues.acces.SaveGame ();

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (Camera.main.transform.position.x, 56, -171);

		yield return new WaitForSeconds (2);

		if (MainMenuBehaviour.acces.mainMenu == true)
		{
			MainMenuBehaviour.acces.newTransformCamera = new Vector3 (0, 150, -220);
			MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (40, 0, 0);
		}

		yield return new WaitForSeconds (1);
		if (MainMenuBehaviour.acces.mainMenu == true)
		{
			MainMenuBehaviour.acces.DeactivateAll ();
		}
	}
	
	// Left of Main Menu
	void Challenges ()
	{
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.challangeObjects.SetActive (true);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);
	}

	void LevelSelection ()
	{
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (true);
	}

	// Right of Main Menu
	void JoinGame ()
	{
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.joinObjects.SetActive (true);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);

		JoinGameMenu.acces.OnButtonClicked ();
	}
	
	void HostGame ()
	{
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (true);
	}

	// Singleplayer Game Modes
	void InfiniteMode ()
	{
		GameModeList.InfiniMode (false, null);
	}

	void DefenseMode ()
	{
		GameModeList.DefenseMode (false, null);
	}
}
