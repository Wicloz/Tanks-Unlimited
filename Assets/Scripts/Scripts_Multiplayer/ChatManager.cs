using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatManager : PersistentSingleton<ChatManager>
{
	private List<string> chatLines = new List<string>();
	private string messageBox = "";
	private Vector2 scrollPosition = new Vector2 (0, 0);
	public int chatHeight;

	private bool isEnabled = false;

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public void Enable ()
	{
		isEnabled = true;
	}

	public void Disable ()
	{
		isEnabled = false;
		chatLines = new List<string>();
	}

	[RPC] private void AddChatLine (string message)
	{
		string m = "[" + System.DateTime.Now.ToString("HH:mm:ss") + "] " + message;

		chatLines.Insert(0, m);
		scrollPosition.y += 22;

		if (chatLines.Count > 100)
		{
			chatLines.RemoveAt(100);
		}
	}

	private void SendPlayerMessage (string message)
	{
		string m = ProfileManager.acces.player.username + ": " + message;

		if (SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC ("AddChatLine", RPCMode.All, m);
		}
		else
		{
			AddChatLine (m);
		}
	}

	public void SendServerMessage (string message)
	{
		string m = "Info" + ": " + message;

		if (SwitchBox.isServerOn)
		{
			GetComponent<NetworkView>().RPC ("AddChatLine", RPCMode.All, m);
		}
		else
		{
			AddChatLine (m);
		}
	}

	void OnGUI ()
	{
		if (isEnabled)
		{
			int begin = 210;

			GUI.Box(new Rect(begin, Screen.height - chatHeight, Screen.width - begin - 10, chatHeight - 2), "");

			messageBox = GUI.TextField (new Rect (begin, Screen.height - 24, Screen.width - begin - 100 - 20, 22), messageBox);

			if ((GUI.Button(new Rect(Screen.width - 110, Screen.height - 24, 100, 22), "Send Message") || Event.current.Equals(Event.KeyboardEvent("None"))) && messageBox != "")
			{
				SendPlayerMessage (messageBox);
				messageBox = "";
			}

			scrollPosition = GUI.BeginScrollView (new Rect(begin, Screen.height - chatHeight, Screen.width - begin - 10, chatHeight - 30), scrollPosition, new Rect(0, 0, Screen.width, chatLines.Count * 22));

			for (int i = 0; i < chatLines.Count; i++)
			{
				GUI.Label (new Rect(0, (22 * chatLines.Count) - (22 + i * 22), chatLines[i].ToCharArray().Length * 10, 22), chatLines[i]);
			}

			GUI.EndScrollView ();
		}
	}
}
