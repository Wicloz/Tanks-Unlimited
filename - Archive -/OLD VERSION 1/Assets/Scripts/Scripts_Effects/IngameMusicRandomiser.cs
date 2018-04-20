using UnityEngine;
using System.Collections;

public class IngameMusicRandomiser : MonoBehaviour
{
	public static IngameMusicRandomiser acces;

	public AudioClip gameOverClip;
	public AudioClip[] musicTracks;
	private AudioSource musicPlayer;
	private bool gameOverRunning = false;

	void Awake ()
	{
		if (acces == null)
		{
			DontDestroyOnLoad(gameObject);
			acces = this;
		}
		else if (acces != this)
		{
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		musicPlayer = gameObject.AddComponent<AudioSource>();
		PickRandomMusic ();
	}

	void Update ()
	{
		musicPlayer.volume = GlobalValues.acces.musicVolume;

		if (GameStates.acces.GameOver && !gameOverRunning)
		{
			gameOverRunning = true;
			audio.Stop ();
			musicPlayer.clip = gameOverClip;
			musicPlayer.audio.loop = true;
			audio.Play ();
		}

		if (!audio.isPlaying)
		{
			PickRandomMusic ();
		}
	}

	void PickRandomMusic ()
	{
		int random = (int)Random.Range (-0.5f, musicTracks.Length - 0.5f);
		for (int i = 0; i < musicTracks.Length; i ++)
		{
			if (i == random)
			{
				musicPlayer.clip = musicTracks[i];
				audio.Play ();
			}
		}
	}
}
