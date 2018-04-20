using UnityEngine;
using System.Collections;

public class LandingGear : MonoBehaviour
{
	GearState state = GearState.Raised;
	Animator animator;

	enum GearState
	{
		Raised = -1,
		Lowered = 1
	}

	void Start ()
	{
		animator = GetComponent<Animator>();
		animator.SetInteger("GearState",(int)state);
	}
}
