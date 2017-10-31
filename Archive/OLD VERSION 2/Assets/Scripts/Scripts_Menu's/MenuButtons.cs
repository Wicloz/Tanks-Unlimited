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
				transform.Translate (new Vector3(-0.1f, 0, -0.1f));
			}
		}
	}

	void OnMouseExit ()
	{
		if (!gameObject.name.Contains ("Door") && !gameObject.name.Contains ("Sing"))
		{
			if (buttonSound.name != "Exit Button" || gameObject.name == "ExitGame")
			{
				transform.Translate (new Vector3(0.1f, 0, 0.1f));
			}
		}
	}

	void OnMouseDown ()
	{
		MainMenuBehaviour.acces.PlayButton (buttonSound);
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cancel") && buttonSound != null && buttonSound.name == "Exit Button" && gameObject.name != "ExitGame" && LoadingScreen.acces.isIdle && Vector3.Distance(transform.position, MainMenuBehaviour.acces.mainCamera.transform.position) <= 70)
		{
			OnMouseDown ();
			OnMouseUp ();
		}
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
			ProfileManager.acces.SaveProfile ();
			Application.Quit ();
			break;
		case "Options":
			OptionsManager.acces.OpenWindow ("MainMenu");
			break;

		case "Challenges":
			Challenges();
			break;
		case "GameModes":
			LevelSelection();
			break;
			
		case "InfiniteMode":
			InfiniMode();
			break;
		case "DefenseMode":
			DefenseMode();
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

	// Door Functionality
	private IEnumerator OpenDoor ()
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

	public void CloseDoorRemote ()
	{
		if (transform.localScale.y != 1)
		{
			StartCoroutine (CloseDoor ());
		}
	}

	private IEnumerator CloseDoor ()
	{
		positionY = transform.localPosition.y;
		float currentTime = Time.time;
		
		float duration = 1;
		float increment = 0.02f / duration;
		
		while (Time.time <= currentTime + duration)
		{
			if (transform.localScale.y <= 1 - increment)
			{
				transform.localScale = new Vector3 (1, transform.localScale.y + increment, 1);
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + increment, transform.localPosition.z);
			}
			else if (transform.localScale.y > 1 - increment)
			{
				transform.localScale = new Vector3 (1, 1, 1);
			}
			
			yield return null;
		}

		StopCoroutine ("BlinkLight");
		transform.parent.Find ("light_TE").GetComponent<Light>().intensity = 0;
		transform.parent.Find ("light_SP").GetComponent<Light>().intensity = 0;
		transform.parent.Find ("light_MP").GetComponent<Light>().intensity = 0;
	}

	private IEnumerator BlinkLight (string lightname)
	{
		GameObject lamp = GameObject.Find (lightname);
		float currentTime = Time.time;
		MainMenuBehaviour.acces.alarmSound.Play ();
		
		while (true)
		{
			lamp.GetComponent<Light>().intensity = Mathf.PingPong ((Time.time - currentTime) * 10, 4);
			yield return null;
		}
	}

	// On Main Menu
	private IEnumerator SinglePlayer ()
	{
		MainMenuBehaviour.acces.mainMenu = false;

		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);
		MainMenuBehaviour.acces.DeactivateGui ();

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -171);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
		StartCoroutine ("BlinkLight", "light_SP");

		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitBeforeDoor);
		yield return StartCoroutine (OpenDoor ());
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitAfterDoor);

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -70);
	}

	private void SinglePlayerButton ()
	{
		MainMenuBehaviour.acces.mainMenu = false;
		
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);
		MainMenuBehaviour.acces.DeactivateGui ();
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (-96.5f, menuHeight, -70);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
	}

	private IEnumerator MultiPlayer ()
	{
		MainMenuBehaviour.acces.mainMenu = false;

		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);
		MainMenuBehaviour.acces.DeactivateGui ();

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -171);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
		StartCoroutine ("BlinkLight", "light_MP");
		
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitBeforeDoor);
		yield return StartCoroutine (OpenDoor ());
		yield return new WaitForSeconds (MainMenuBehaviour.acces.waitAfterDoor);
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -70);
	}

	private void MultiPlayerButton ()
	{
		MainMenuBehaviour.acces.mainMenu = false;
		
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (true);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);
		MainMenuBehaviour.acces.DeactivateGui ();
		
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (96.5f, menuHeight, -70);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (0, 0, 0);
	}

	private IEnumerator TankEditor ()
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
	}

	private IEnumerator MainMenu ()
	{
		MainMenuBehaviour.acces.mainMenu = true;

		MainMenuBehaviour.acces.SwitchTrack ("menu");
		ProfileManager.acces.SaveProfile ();

		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (Camera.main.transform.position.x, 56, -171);

		yield return new WaitForSeconds (2);
		MainMenuBehaviour.acces.newTransformCamera = new Vector3 (0, 150, -220);
		MainMenuBehaviour.acces.newRotationCamera = Quaternion.Euler (40, 0, 0);

		GameObject.Find ("SinglePlayerDoor").GetComponent<MenuButtons>().CloseDoorRemote ();
		GameObject.Find ("MultiPlayerDoor").GetComponent<MenuButtons>().CloseDoorRemote ();
		GameObject.Find ("TankEditorDoor").GetComponent<MenuButtons>().CloseDoorRemote ();

		yield return new WaitForSeconds (1);
		MainMenuBehaviour.acces.ResetMenu ();
		MainMenuBehaviour.acces.DeactivateGui ();
	}
	
	// Left of Main Menu
	private void Challenges ()
	{
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.challangeObjects.SetActive (true);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (false);
	}

	private void LevelSelection ()
	{
		MainMenuBehaviour.acces.singlePlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.challangeObjects.SetActive (false);
		MainMenuBehaviour.acces.arcadeObjects.SetActive (true);

		GameOptionEditor.acces.isEnabled = true;
	}

	// Right of Main Menu
	private void JoinGame ()
	{
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.joinObjects.SetActive (true);
		MainMenuBehaviour.acces.hostObjects.SetActive (false);

		JoinGameMenu.acces.ButtonClicked ();
		JoinGameMenu.acces.isEnabled = true;
	}
	
	private void HostGame ()
	{
		MainMenuBehaviour.acces.multiPlayerObjects.SetActive (false);
		MainMenuBehaviour.acces.joinObjects.SetActive (false);
		MainMenuBehaviour.acces.hostObjects.SetActive (true);

		HostGameMenu.acces.isEnabled = true;
	}

	// Singleplayer Game Modes
	private void InfiniMode ()
	{
		StateManager.acces.StartLevel ("Play_InfiniMode", false, true, null);
	}

	private void DefenseMode ()
	{
		StateManager.acces.StartLevel ("Play_DefenseMode", false, true, null);
	}
}
