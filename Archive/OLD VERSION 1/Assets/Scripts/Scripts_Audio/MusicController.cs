using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void StopPlaying ()
	{
		Destroy (gameObject);
	}
}
