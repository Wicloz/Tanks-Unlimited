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

	public TypogenicText userText;
	public TypogenicText[] pointText;

	public static MainMenuBehaviour acces;

	public AudioSource alarmSound;
	public float waitBeforeDoor;
	public float waitAfterDoor;

	public GameObject mainCamera;
	public AudioSource menuMusic;
	public AudioSource editorMusic;
	
	public float moveSpeed;
	public float musicSpeed;

	public float newZoomCamera;
	public Vector3 newTransformCamera;
	public Quaternion newRotationCamera;
	private float menuMusicIntensity = 0;
	private float editorMusicIntensity = 0;

	public bool mainMenu = true;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		newTransformCamera = mainCamera.transform.position;
		newRotationCamera = mainCamera.transform.rotation;
		newZoomCamera = mainCamera.GetComponent<Camera>().fieldOfView;
		
		SwitchTrack ("menu");
		ResetMenu ();
		DeactivateGui ();
	}

	public void ResetMenu ()
	{
		singlePlayerObjects.SetActive (true);
		challangeObjects.SetActive (false);
		arcadeObjects.SetActive (false);
		multiPlayerObjects.SetActive (true);
		joinObjects.SetActive (false);
		hostObjects.SetActive (false);
		DeactivateGui ();
	}

	public void DeactivateGui ()
	{
		JoinGameMenu.acces.isEnabled = false;
		HostGameMenu.acces.isEnabled = false;
		GameOptionEditor.acces.isEnabled = false;
	}

	public void SwitchTrack (string track)
	{
		switch (track)
		{
		case ("none"):
			menuMusicIntensity = 0;
			editorMusicIntensity = 0;
			break;
		case ("menu"):
			menuMusicIntensity = 1;
			editorMusicIntensity = 0;
			break;
		case ("editor"):
			menuMusicIntensity = 0;
			editorMusicIntensity = 1;
			break;
		default:
			Debug.LogError ("Track Invalid");
			break;
		}
	}

	public void PlayButton (AudioClip clip)
	{
		GetComponent<AudioSource>().Stop ();
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().Play ();
	}

	void Update ()
	{
		menuMusic.volume = Mathf.Lerp (menuMusic.volume, OptionsManager.acces.musicVolume * menuMusicIntensity, Time.deltaTime * musicSpeed);
		editorMusic.volume = Mathf.Lerp (editorMusic.volume, OptionsManager.acces.musicVolume * editorMusicIntensity, Time.deltaTime * musicSpeed);
		
		mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position, newTransformCamera, moveSpeed * Time.deltaTime);
		mainCamera.transform.rotation = Quaternion.Slerp (mainCamera.transform.rotation, newRotationCamera, moveSpeed * Time.deltaTime);
		mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp (mainCamera.GetComponent<Camera>().fieldOfView, newZoomCamera, moveSpeed * Time.deltaTime);

		userText.Text = "Welcome " + ProfileManager.acces.player.username;
		foreach (TypogenicText text in pointText)
		{
			text.Text = "Total Points: " + ProfileManager.acces.player.points;
		}
	}
}
