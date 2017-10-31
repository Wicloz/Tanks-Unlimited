using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverText : MonoBehaviour
{
	public static GameOverText acces;
	List <GameObject> lineList = new List <GameObject> ();
	List <RequestTemplate> reqList = new List <RequestTemplate> ();

	public GameObject seperator;
	public GameObject lineText;
	public GameObject lineTank;

	public GameObject background;
	public GameObject text_GameOver;
	public GameObject text_Return;

	public int spacing;

	private int offset;
	private int generalWidth;
	public bool requestDone = false;
	private bool linesDone = false;

	void Awake ()
	{
		acces = this;
	}

	void OnEnable ()
	{
		generalWidth = (int) (Screen.width / 2);

		background.GetComponent<GUITexture>().pixelInset = new Rect (generalWidth / -2, -1000, generalWidth, 2000);
		text_GameOver.GetComponent<GUIText>().fontSize = generalWidth / 6;
		text_GameOver.GetComponent<GUIText>().pixelOffset = new Vector2 (0, (- generalWidth / 12) + 5);
		text_Return.GetComponent<GUIText>().fontSize = generalWidth / 33;

		offset = text_GameOver.GetComponent<GUIText>().fontSize - 10;

		text_Return.SetActive (false);

		StartCoroutine (AddLineFunction ());
		AddSeperatorLine ();
	}

	void Update ()
	{
		if (requestDone && linesDone)
		{
			text_Return.SetActive (true);
			text_Return.GetComponent<Fader>().originalAlpha = 1;
		}
		else
		{
			text_Return.SetActive (false);
		}
	}

	IEnumerator AddLineFunction ()
	{
		int currentIndex = 0;
		while (true)
		{
			if (currentIndex < reqList.Count)
			{
				linesDone = false;

				switch (reqList[currentIndex].type)
				{
				case "seperator":
					AddSeperatorLine ();
					break;
				case "text":
					AddTextLine (reqList[currentIndex].text, reqList[currentIndex].fontSize);
					break;
				case "tank":
					AddTankLine (reqList[currentIndex].amount, reqList[currentIndex].text, reqList[currentIndex].fontSize);
					break;
				}

			 	currentIndex ++;
				yield return new WaitForSeconds (0.6f);
			}

			else
			{
				linesDone = true;
			}

			yield return null;
		}
	}

	public void RequestSeperatorLine ()
	{
		RequestTemplate request = new RequestTemplate ("seperator");
		reqList.Add (request);
	}

	public void RequestTextLine (string content, int fontSize)
	{
		RequestTemplate request = new RequestTemplate ("text", fontSize, content);
		reqList.Add (request);
	}

	public void RequestTankLine (int amount, string name, int fontSize)
	{
		RequestTemplate request = new RequestTemplate ("tank", fontSize, name, amount);
		reqList.Add (request);
	}

	public class RequestTemplate
	{
		public string type;
		
		public int fontSize;
		public string text;
		public int amount;
		
		public RequestTemplate (string setType)
		{
			type = setType;
		}
		
		public RequestTemplate (string setType, int setFontSize, string setText)
		{
			type = setType;
			fontSize = setFontSize;
			text = setText;
		}
		
		public RequestTemplate (string setType, int setFontSize, string setText, int setAmount)
		{
			type = setType;
			fontSize = setFontSize;
			text = setText;
			amount = setAmount;
		}
	}

	void AddSeperatorLine ()
	{
		GameObject newObject = (GameObject) Instantiate (seperator, new Vector3 (0.5f, 1, 0), Quaternion.identity);
		newObject.transform.parent = gameObject.transform;

		newObject.GetComponent<GUITexture>().pixelInset = new Rect ((generalWidth - 20) / -2, 0, generalWidth - 20, 2);

		FinaliseAddLine (2 + spacing, newObject);
	}

	void AddTextLine (string content, int fontSize)
	{
		GameObject newObject = (GameObject) Instantiate (lineText, new Vector3 (0.5f, 1, 0), Quaternion.identity);
		newObject.transform.parent = gameObject.transform;

		newObject.GetComponent<GUIText>().fontSize = fontSize;
		newObject.GetComponent<GUIText>().text = content;

		FinaliseAddLine (fontSize + spacing, newObject);
	}

	void AddTankLine (int amount, string name, int fontSize)
	{
		GameObject newObject = (GameObject) Instantiate (lineTank, new Vector3 (0.5f, 1, 0), Quaternion.identity);
		newObject.transform.parent = gameObject.transform;


		
		FinaliseAddLine (fontSize + spacing, newObject);
	}

	void FinaliseAddLine (int height, GameObject newObject)
	{
		offset += height / 2;
		
		if (newObject.GetComponent<GUIText>() != null)
		{
			newObject.GetComponent<GUIText>().pixelOffset = new Vector2 (newObject.GetComponent<GUIText>().pixelOffset.x, -offset);
		}
		if (newObject.GetComponent<GUITexture>() != null)
		{
			newObject.GetComponent<GUITexture>().pixelInset = new Rect (newObject.GetComponent<GUITexture>().pixelInset.x, -offset, newObject.GetComponent<GUITexture>().pixelInset.width, newObject.GetComponent<GUITexture>().pixelInset.height);
		}
		
		offset += height / 2;
		
		lineList.Add (newObject);
	}
}
