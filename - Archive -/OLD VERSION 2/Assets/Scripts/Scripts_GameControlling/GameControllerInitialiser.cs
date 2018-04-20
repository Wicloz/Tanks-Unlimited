using UnityEngine;
using System.Collections;

public class GameControllerInitialiser : MonoBehaviour
{
	public GameObject hudItems;
	public GameObject gameOverItems;
	public GameObject stageCompleteItems;
	public GameObject gameOverText;
	public GameObject viewObstructor;
	public GameObject scoreText;

	public GameObject gameoverExplosion;

	void Start ()
	{
		GameController.acces.SetReferences (hudItems, gameOverItems, stageCompleteItems, gameOverText, viewObstructor, scoreText, gameoverExplosion);
		Destroy (gameObject);
	}
}
