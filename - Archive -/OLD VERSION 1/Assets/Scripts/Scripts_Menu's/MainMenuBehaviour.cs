using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour
{
	public GameObject singlePlayerObjects;
	public GameObject challangeObjects;
	public GameObject arcadeObjects;

	public GameObject multiPlayerObjects;
	public GameObject joinObjects;
	public GameObject hostObjects;

	public static MainMenuBehaviour acces;
	public static ScreenFader screenFader;

	public GameObject alarmSound;
	public float waitBeforeDoor;
	public float waitAfterDoor;

	public GameObject mainCamera;
	public GameObject menuMusic;
	public GameObject editorMusic;
	
	public bool disableLevelOptions = false;
	public bool disableJoinMenu = false;
	
	public float moveSpeed;
	public float musicSpeed;

	public float newZoomCamera;
	public Vector3 newTransformCamera;
	public Quaternion newRotationCamera;
	private float menuMusicIntensity = 0;
	private float editorMusicIntensity = 0;

	public GameObject loadingScreen;
	public bool mainMenu = true;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		Time.timeScale = 1;
		
		screenFader = GameObject.Find ("screenFader").GetComponent <ScreenFader>();
		
		newTransformCamera = mainCamera.transform.position;
		newRotationCamera = mainCamera.transform.rotation;
		newZoomCamera = mainCamera.camera.fieldOfView;
		
		SwitchTrack ("menu");
		DeactivateAll ();
	}

	public void DeactivateAll ()
	{
		singlePlayerObjects.SetActive (false);
		challangeObjects.SetActive (false);
		arcadeObjects.SetActive (false);

		multiPlayerObjects.SetActive (false);
		joinObjects.SetActive (false);
		hostObjects.SetActive (false);
	}

	public void ChangeLoading (bool state)
	{
		loadingScreen.SetActive (state);
		JoinGameMenu.acces.isEnabled = false;
	}
	
	public void SwitchTrack (string track)
	{
		switch (track)
		{
		case ("none"):
			menuMusicIntensity = 0.0f;
			editorMusicIntensity = 0.0f;
			break;
		case ("menu"):
			menuMusicIntensity = 0.6f;
			editorMusicIntensity = 0.0f;
			break;
		case ("editor"):
			menuMusicIntensity = 0.0f;
			editorMusicIntensity = 0.6f;
			break;
		default:
			Debug.LogError ("Track Invalid");
			break;
		}
	}

	public void PlayButton (AudioClip clip)
	{
		audio.Stop ();

		audio.clip = clip;

		audio.Play ();
	}

	void Update ()
	{
		menuMusic.audio.volume = Mathf.Lerp (menuMusic.audio.volume, menuMusicIntensity, Time.deltaTime * musicSpeed);
		editorMusic.audio.volume = Mathf.Lerp (editorMusic.audio.volume, editorMusicIntensity, Time.deltaTime * musicSpeed);
		
		mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position, newTransformCamera, moveSpeed * Time.deltaTime);
		mainCamera.transform.rotation = Quaternion.Slerp (mainCamera.transform.rotation, newRotationCamera, moveSpeed * Time.deltaTime);
		mainCamera.camera.fieldOfView = Mathf.Lerp (mainCamera.camera.fieldOfView, newZoomCamera, moveSpeed * Time.deltaTime);
		
		CheckStates ();
	}

	void CheckStates ()
	{
		if (arcadeObjects.activeSelf)
		{
			LevelOptionEditor.acces.isEnabled = true;
		}
		else
		{
			LevelOptionEditor.acces.isEnabled = false;
		}
		
		if (joinObjects.activeSelf && !disableJoinMenu)
		{
			JoinGameMenu.acces.isEnabled = true;
		}
		else
		{
			JoinGameMenu.acces.isEnabled = false;
		}

		if (hostObjects.activeSelf && !disableJoinMenu)
		{
			HostGameMenu.acces.isEnabled = true;
		}
		else
		{
			HostGameMenu.acces.isEnabled = false;
		}
	}
}
