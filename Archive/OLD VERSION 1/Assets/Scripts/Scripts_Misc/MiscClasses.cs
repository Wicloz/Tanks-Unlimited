using UnityEngine;
using System.Collections;

public class BounceAngleCalculations
{
	private static Vector3 leaveAngle = new Vector3(0,0,0);
	private static string relDirection = "null";
	
	public static Vector3 AngleCalculation (Transform currentTransform)
	{
		RaycastHit hit;
		
		if (Physics.Raycast (currentTransform.position, currentTransform.forward, out hit))
		{
			switch (hit.transform.gameObject.name)
			{
			case "Ground":
				break;
			case "Cube 1":
				CalcCubeVariant1 (hit);
				break;
			case "Cube 1(Clone)":
				CalcCubeVariant1 (hit);
				break;
			case "Cube 2":
				CalcCubeVariant1 (hit);
				break;
			case "Cube 2(Clone)":
				CalcCubeVariant1 (hit);
				break;
			case "collider":
				CalcArenaWall (hit);
				break;
			case "trigger_flag_pole(Clone)":
				FlagPole (hit);
				break;
			default:
				Debug.LogError ("Script is trying to calculate leaveAngle of non-static object " + hit.transform.gameObject.name);
				break;
			}
			
			switch (relDirection)
			{
			case "Top":
				leaveAngle = new Vector3(0, 180 - currentTransform.rotation.eulerAngles.y, 0);
				break;
			case "Bottom":
				leaveAngle = new Vector3(0, 180 - currentTransform.rotation.eulerAngles.y, 0);
				break;
			case "Left":
				leaveAngle = new Vector3(0, 360 - currentTransform.rotation.eulerAngles.y, 0);
				break;
			case "Right":
				leaveAngle = new Vector3(0, 360 - currentTransform.rotation.eulerAngles.y, 0);
				break;
			}
		}
		
		return leaveAngle;
	}
	
	static void CalcCubeVariant1 (RaycastHit hit)
	{
		Vector3 relativeHitPoint = hit.point - hit.transform.position;
		
		if (relativeHitPoint.x == 0.5f)
		{
			relDirection = "Left";
		}
		else if (relativeHitPoint.x == -0.5f)
		{
			relDirection = "Right";
		}
		else if (relativeHitPoint.z == 0.5f)
		{
			relDirection = "Top";
		}
		else if (relativeHitPoint.z == -0.5f)
		{
			relDirection = "Bottom";
		}
	}
	
	static void CalcArenaWall (RaycastHit hit)
	{
		Vector3 relativeHitPoint = hit.point - hit.transform.position;
		
		if (relativeHitPoint.x == 1.0f)
		{
			relDirection = "Left";
		}
		else if (relativeHitPoint.x == -1.0f)
		{
			relDirection = "Right";
		}
		else if (relativeHitPoint.z == 1.0f)
		{
			relDirection = "Top";
		}
		else if (relativeHitPoint.z == -1.0f)
		{
			relDirection = "Bottom";
		}
	}
	
	static void FlagPole (RaycastHit hit)
	{
		Vector3 relativeHitPoint = hit.point - hit.transform.position;
		
		if (relativeHitPoint.x == 0.03f)
		{
			relDirection = "Left";
		}
		else if (relativeHitPoint.x == -0.03f)
		{
			relDirection = "Right";
		}
		else if (relativeHitPoint.z == 0.03f)
		{
			relDirection = "Top";
		}
		else if (relativeHitPoint.z == -0.03f)
		{
			relDirection = "Bottom";
		}
	}
}

public class Destructor : MonoBehaviour
{
	public static GameObject tankExplosion;
	public static GameObject dynamicObject1Explosion;
	public static GameObject dynamicPart1Explosion;
	
	public static void PlayerTank (GameObject collision)
	{
		SwitchBox.Destroy (collision);
		SwitchBox.Instantiate (tankExplosion, collision.transform.position, collision.transform.rotation);
		CameraShake.acces.SetShake ("PlayerTank", ShakeValues.playerTank_intensity, ShakeValues.playerTank_duration, ShakeValues.playerTank_fade);
	}
	
	public static void NpcTank (GameObject collision)
	{
		if (collision.GetComponent <NpcController>() != null)
		{
			GameController.acces.AddScore (collision.GetComponent <NpcController>().GetScore());
			GameController.acces.OnNpcTankDestroy ();
			RunRecorder.acces.OnEnemyDestroyed (collision);
		}
		SwitchBox.Destroy (collision);
		SwitchBox.Instantiate (tankExplosion, collision.transform.position, collision.transform.rotation);
		CameraShake.acces.SetShake ("NpcTank", ShakeValues.npcTank_intensity, ShakeValues.npcTank_duration, ShakeValues.npcTank_fade);
	}
	
	public static void DynamicPart (GameObject collision)
	{
		SwitchBox.Destroy (collision);
		
		switch (collision.name)
		{
		case ("plane1"):
			DestroyDynamicPart1 (collision);
			break;
		case ("plane2"):
			DestroyDynamicPart1 (collision);
			break;
		case ("sidePlane1"):
			DestroyDynamicPart1 (collision);
			break;
		case ("sidePlane2"):
			DestroyDynamicPart1 (collision);
			break;
		case ("sidePlane3"):
			DestroyDynamicPart1 (collision);
			break;
		case ("sidePlane4"):
			DestroyDynamicPart1 (collision);
			break;
		default:
			Debug.LogWarning ("Unknown dynamic part " + collision.name + " was destroyed");
			break;
		}
	}
	
	static void DestroyDynamicPart1 (GameObject collision)
	{
		SwitchBox.Instantiate (dynamicPart1Explosion, collision.transform.position, collision.transform.rotation);
	}
	
	public static void DynamicObject (GameObject collision)
	{
		SwitchBox.Destroy (collision);
		
		switch (collision.name)
		{
		case ("Destructible 1"):
			DestroyDynamicObject1 (collision);
			break;
		case ("Destructible 1(Clone)"):
			DestroyDynamicObject1 (collision);
			break;
		default:
			Debug.LogWarning ("Unknown dynamic object " + collision.name + " was destroyed");
			break;
		}
	}
	
	static void DestroyDynamicObject1 (GameObject collision)
	{
		GameObject explosion_destObject1 = SwitchBox.Instantiate (dynamicObject1Explosion, new Vector3(collision.transform.position.x, 0.0f, collision.transform.position.z), collision.transform.rotation);
		
		PartsHolder partsHolder = explosion_destObject1.GetComponent <PartsHolder>();
		GameObject[] explosion_destObject1_parts = partsHolder.explosion_parts;
		
		for (int i = 0; i < explosion_destObject1_parts.Length; i++)
		{
			explosion_destObject1_parts[i].rigidbody.AddForce (new Vector3(Random.Range(-5,5), 0.0f, Random.Range(-5,5)), ForceMode.VelocityChange);
		}
	}
}

public class ShakeValues
{
	public static float playerTank_intensity = 0.02f;
	public static float playerTank_duration = 1.2f;
	public static float playerTank_fade = 1.0f;

	public static float npcTank_intensity = 0.02f;
	public static float npcTank_duration = 1.2f;
	public static float npcTank_fade = 1.0f;

	public static float mine_intensity = 0.05f;
	public static float mine_duration = 0.8f;
	public static float mine_fade = 1.0f;
}
