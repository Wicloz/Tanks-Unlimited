using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreText : MonoBehaviour
{
	public static ScoreText acces;
	List <GameObject> textFields = new List <GameObject> ();

	public GameObject guiObject;

	void Awake ()
	{
		acces = this;
	}

	public void EditLine (int index, string value)
	{
		if (index <= textFields.Count - 1)
		{
			textFields[index].guiText.text = value;
		}
		else
		{
			AddLine (index);
		}
	}

	void AddLine (int index)
	{
		GameObject newObject = (GameObject) Instantiate (guiObject, new Vector3 (0, 1, 0), Quaternion.identity);
		newObject.transform.parent = gameObject.transform;

		float offset = index * -20;
		newObject.guiText.pixelOffset = new Vector2 (10, -10 + offset);

		textFields.Add (newObject);
	}
}
