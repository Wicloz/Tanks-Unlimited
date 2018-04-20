using UnityEngine;
using System.Collections;

public class IngameMusicRandomiser : MonoBehaviour
{
	public static IngameMusicRandomiser acces;

	public AudioClip gameOverClip;
	public AudioClip lobbyClip;
	public AudioClip[] musicTracks;

	private AudioSource musicPlayer;

	private bool gameOverRunning = false;
	private bool lobbyRunning = false;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		musicPlayer = gameObject.GetComponent<AudioSource>();
		musicPlayer.volume = OptionsManager.acces.musicVolume;
		PickRandomMusic ();
	}

	void Update ()
	{
		musicPlayer.volume = Mathf.Lerp (musicPlayer.volume, OptionsManager.acces.musicVolume, Time.deltaTime * OptionsManager.acces.volumeSpeed);

		if (MultiplayerManager.acces.gameState == GameState.Lobby)
		{
			if (!lobbyRunning)
			{
				lobbyRunning = true;
				GetComponent<AudioSource>().Stop ();
				musicPlayer.clip = lobbyClip;
				musicPlayer.GetComponent<AudioSource>().loop = true;
				GetComponent<AudioSource>().Play ();
			}
		}

		else if (GameController.acces.gameOverPost)
		{
			if (!gameOverRunning)
			{
				gameOverRunning = true;
				GetComponent<AudioSource>().Stop ();
				musicPlayer.clip = gameOverClip;
				musicPlayer.GetComponent<AudioSource>().loop = true;
				GetComponent<AudioSource>().Play ();
			}
		}

		else if (lobbyRunning || gameOverRunning)
		{
			PickRandomMusic ();
			lobbyRunning = false;
			gameOverRunning = false;
		}

		if (!GetComponent<AudioSource>().isPlaying && SwitchBox.isHost)
		{
			PickRandomMusic ();
		}
	}

	private void PickRandomMusic ()
	{
		int random = Random.Range (0, musicTracks.Length);

		if (SwitchBox.isServer)
		{
			GetComponent<NetworkView>().RPC ("PlayTrack", RPCMode.AllBuffered, musicTracks[random].name);
		}
		else if (!SwitchBox.isClient)
		{
			PlayTrack (musicTracks[random].name);
		}
	}

	[RPC] private void PlayTrack (string trackName)
	{
		musicPlayer.GetComponent<AudioSource>().loop = false;
		musicPlayer.Stop ();

		foreach (AudioClip clip in musicTracks)
		{
			if (clip.name == trackName)
			{
				musicPlayer.clip = clip;
				break;
			}
		}

		musicPlayer.Play ();
	}
}
