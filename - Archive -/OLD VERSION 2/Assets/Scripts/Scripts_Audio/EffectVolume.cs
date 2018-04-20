using UnityEngine;
using System.Collections;

public class EffectVolume : MonoBehaviour
{
	private AudioSource player;

	void Start ()
	{
		player = GetComponent<AudioSource>();
		player.volume = OptionsManager.acces.effectsVolume;
	}

	void Update ()
	{
		player.volume = Mathf.Lerp (player.volume, OptionsManager.acces.effectsVolume, Time.deltaTime * OptionsManager.acces.volumeSpeed);
	}
}
