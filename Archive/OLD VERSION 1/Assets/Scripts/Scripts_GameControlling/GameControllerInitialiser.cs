using UnityEngine;
using System.Collections;

public class GameControllerInitialiser : MonoBehaviour
{
	public GameObject[] largeObjects;
	public GameObject[] smallObjects;
	public GameObject[] destObjects;
	public GameObject enemyTank;

	public GameObject hudItems;
	public GameObject gameOverItems;
	public GameObject gameOverText;
	public GameObject viewObstructor;
	public GameObject scoreText;

	public GameObject EXPLOSION;

	void Start ()
	{
		GameController.acces.SetReferences (largeObjects, smallObjects, destObjects, enemyTank, hudItems, gameOverItems, gameOverText, viewObstructor, scoreText, EXPLOSION);
		Destroy (gameObject);
	}
}
