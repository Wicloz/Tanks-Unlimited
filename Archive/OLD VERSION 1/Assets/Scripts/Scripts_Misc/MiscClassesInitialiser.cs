using UnityEngine;
using System.Collections;

public class MiscClassesInitialiser : MonoBehaviour
{
	public GameObject tankExplosion;
	public GameObject dynamicObject1Explosion;
	public GameObject dynamicPart1Explosion;

	void Start ()
	{
		Destructor.tankExplosion = tankExplosion;
		Destructor.dynamicObject1Explosion = dynamicObject1Explosion;
		Destructor.dynamicPart1Explosion = dynamicPart1Explosion;
	}
}
