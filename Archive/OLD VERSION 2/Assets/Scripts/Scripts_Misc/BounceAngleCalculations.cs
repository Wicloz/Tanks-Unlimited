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
