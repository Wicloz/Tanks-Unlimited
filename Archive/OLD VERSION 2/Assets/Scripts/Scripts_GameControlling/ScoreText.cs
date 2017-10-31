using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreText : MonoBehaviour
{
	public class UserElements
	{
		public string username;
		public List<GameObject> textFields = new List<GameObject>();

		public UserElements (string username)
		{
			this.username = username;
		}
	}

	public static ScoreText acces;
	UserElements thisUser = new UserElements ("");
	List<UserElements> otherUsers = new List<UserElements> ();

	public GameObject guiObject;

	void Awake ()
	{
		acces = this;
	}

	public void Clear ()
	{
		RemoveUser (thisUser);

		foreach (UserElements user in otherUsers)
		{
			RemoveUser (user);
		}
		
		thisUser = new UserElements ("");
		otherUsers = new List<UserElements> ();
	}

	void Update ()
	{
		for (int i = 0; i < otherUsers.Count; i++)
		{
			bool userExists = false;
			foreach (UserInfo user in UserManager.acces.userList)
			{
				if (user.username == otherUsers[i].username && user.isOnline)
				{
					userExists = true;
					break;
				}
			}

			if (!userExists)
			{
				RemoveUser (otherUsers[i]);
				otherUsers.RemoveAt (i);
				i--;
				continue;
			}

			for (int j = 0; j < otherUsers[i].textFields.Count; j++)
			{
				float lastUserSize = 0;

				if (i != 0)
				{
					lastUserSize = (otherUsers[i-1].textFields.Count + 1);
				}

				MoveLine (otherUsers[i].textFields[j], -20 * (lastUserSize + j), false);
			}
		}
	}

	private void RemoveUser (UserElements user)
	{
		foreach (GameObject line in user.textFields)
		{
			Destroy (line);
		}
	}

	public void EditLineSelf (int line, string value)
	{
		if (line <= thisUser.textFields.Count - 1)
		{
			thisUser.textFields[line].GetComponent<GUIText>().text = value;
		}
		else
		{
			thisUser.textFields.Add (CreateLine (line * -20, true));
		}
	}

	public void EditLineOther (string username, int line, string value)
	{
		bool userExists = false;
		foreach (UserElements user in otherUsers)
		{
			if (user.username == username)
			{
				userExists = true;
				break;
			}
		}

		if (!userExists)
		{
			otherUsers.Add (new UserElements (username));
		}

		for (int i = 0; i < otherUsers.Count; i++)
		{
			if (otherUsers[i].username == username)
			{
				if (line <= otherUsers[i].textFields.Count - 1)
				{
					otherUsers[i].textFields[line].GetComponent<GUIText>().text = value;
				}
				else
				{
					otherUsers[i].textFields.Add (CreateLine (0, false));
				}

				break;
			}
		}
	}

	private GameObject CreateLine (float offset, bool thisUser)
	{
		GameObject newObject = (GameObject) Instantiate (guiObject, new Vector3 (0, 1, 0), Quaternion.identity);
		newObject.transform.SetParent (gameObject.transform, true);
		MoveLine (newObject, offset, thisUser);
		return newObject;
	}

	private void MoveLine (GameObject line, float newOffset, bool thisUser)
	{
		float sideOffset = 10;
		
		if (!thisUser)
		{
			line.GetComponent<GUIText>().anchor = TextAnchor.UpperRight;
			sideOffset = Screen.width - sideOffset;
		}

		line.GetComponent<GUIText>().pixelOffset = new Vector2 (sideOffset, -10 + newOffset);
	}
}
