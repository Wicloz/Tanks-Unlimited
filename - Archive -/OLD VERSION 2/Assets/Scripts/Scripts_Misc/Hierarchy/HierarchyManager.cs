using UnityEngine;
using System.Collections;

public class HierarchyManager : MonoBehaviour
{
	public static GameObject mineParent;
	public static GameObject shellParent;
	public static GameObject probeParent;

	public static GameObject tankParent;
	public static GameObject objectParent;
	public static GameObject effectParent;

	void Awake ()
	{
		mineParent = GameObject.Find ("Mines");
		shellParent = GameObject.Find ("Shells");
		probeParent = GameObject.Find ("Probes");

		tankParent = GameObject.Find ("Players and NPC's");
		objectParent = GameObject.Find ("Game Area Objects");
		effectParent = GameObject.Find ("Effects and Sounds");
	}
}
