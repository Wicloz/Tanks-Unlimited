using UnityEngine;
using System.Collections;

public class JoinGameMenu : MenuLayoutHelper
{
	public static JoinGameMenu acces;

	public bool isEnabled = false;

	private HostData[] hostList;
	private HostData[] filteredList;
	private HostData selectedGame;
	private int listLength = 0;

	HostGameMenu.CommentData commentData = new HostGameMenu.CommentData ();

	private string passWord_required = "";
	private string passWord_entered = "";

	private string filter_name = "";
	private bool filter_infinimode = true;
	private bool filter_defensemode = true;
	private bool filter_password = true;
	private bool filter_full = false;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		RefreshHostList ();
	}

	public void SetFilters (bool infinimode, bool defensemode)
	{
		filter_infinimode = infinimode;
		filter_defensemode = defensemode;
	}

	public void OnButtonClicked ()
	{
		GlobalValues.acces.lives = 0;
		RefreshHostList ();
	}

	private void RefreshHostList()
	{
		selectedGame = null;
		MasterServer.RequestHostList(HostGameMenu.gameName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
			FilterList ();
		}
	}

	void FilterList ()
	{
		listLength = 0;

		for (int i = 0; i < hostList.Length; i++)
		{
			if (FilterCheck (hostList[i]))
			{
				listLength ++;
			}
		}

		filteredList = new HostData[listLength];
		int number = 0;

		for (int i = 0; i < hostList.Length; i++)
		{
			if (FilterCheck (hostList[i]))
			{
				filteredList[number] = hostList[i];
				number ++;
			}
		}
	}

	bool FilterCheck (HostData hostData)
	{
		bool filter = false;
		HostGameMenu.CommentData localCommentData = SerializerHelper.DeserializeFromString<HostGameMenu.CommentData> (hostData.comment);

		if (!filter && filter_infinimode && localCommentData.gameMode == "Play_InfiniMode")
		{
			filter = true;
		}
		
		if (!filter && filter_defensemode && localCommentData.gameMode == "Play_DefenseMode")
		{
			filter = true;
		}

		if (filter && !filter_password && localCommentData.passWord != "")
		{
			filter = false;
		}

		if (filter && hostData.connectedPlayers == hostData.playerLimit)
		{
			filter = false;
		}

		if (filter && filter_name != "")
		{
			filter = hostData.gameName.Contains (filter_name);
		}
		
		return filter;
	}

	private Vector2 scrollPosition = Vector2.zero;

	void OnGUI()
	{
		if (isEnabled)
		{
			// Boxes
			GUI.Box(new Rect(boxStartX1, boxStartY, boxWidth, boxEndY - boxStartY), "Filters");
			GUI.Box(new Rect(boxStartX2, boxStartY, boxWidth, boxEndY - boxStartY), "Room List");
			GUI.Box(new Rect(boxStartX3, boxStartY, boxWidth, boxEndY - boxStartY), "Room Details");

			// First Section
			if (GUI.Button(new Rect(boxStartX1 + halfBox - 100, endY - 40, 200, 40), "Refresh"))
			{
				RefreshHostList ();
			}

			filter_name = GUI.TextField(new Rect(startX1, startY, width, 22), filter_name);
			filter_infinimode = GUI.Toggle(new Rect(startX1, startY + 30, 200, 22), filter_infinimode, "Battle Arena");
			filter_defensemode = GUI.Toggle(new Rect(startX1, startY + 55, 200, 22), filter_defensemode, "Defend the Flag");

			filter_password = GUI.Toggle(new Rect(startX1, startY + 100, 200, 22), filter_password, "Password Protected");
			filter_full = GUI.Toggle(new Rect(startX1, startY + 125, 200, 22), filter_full, "Show Full Games");
			
			// Second Section
			scrollPosition = GUI.BeginScrollView(new Rect(startX2, startY, width, boxEndY - startY - itemSpacing), scrollPosition, new Rect(0, 0, width - itemSpacing, listLength * 30));

			if (filteredList != null)
			{
				for (int i = 0; i < filteredList.Length; i++)
				{
					if (GUI.Button(new Rect(0, (30 * i), width - itemSpacing, 22), filteredList[i].gameName))
					{
						GetCommentData (filteredList[i]);
						selectedGame = filteredList[i];
					}
				}
			}

			GUI.EndScrollView();

			// Third Section
			if (selectedGame != null)
			{
				GUI.Label (new Rect(startX3, startY, 200, 22), "Host Name: " + commentData.hostName);
				GUI.Label (new Rect(startX3, startY + 25, 200, 22), "Gamemode: " + FilterName(commentData.gameMode));
				GUI.Label (new Rect(startX3, startY + 50, 200, 22), "Difficulty: " + commentData.difficulty);

				if (passWord_required != "")
				{
					GUI.Label (new Rect(startX3, endY - 102, 200, 22), "Password:");
					passWord_entered = GUI.TextField(new Rect(startX3, endY - 80, width, 22), passWord_entered);
				}
				
				if (GUI.Button(new Rect(boxStartX3 + halfBox - 100, endY - 40, 200, 40), "Join Game"))
				{
					if (passWord_entered == passWord_required)
					{
						JoinServer (selectedGame);
					}
					else
					{
						passWord_entered = "Invalid Password";
					}
				}
			}
		}
	}

	void GetCommentData (HostData hostData)
	{
		commentData = SerializerHelper.DeserializeFromString<HostGameMenu.CommentData> (hostData.comment);
		passWord_required = commentData.passWord;
	}
	
	// Joining a server
	private void JoinServer (HostData hostData)
	{
		MainMenuBehaviour.acces.disableJoinMenu = true;

		if (commentData.gameMode == "Play_DefenseMode")
		{
			GameModeList.DefenseMode (true, hostData);
		}
		else if (commentData.gameMode == "Play_InfiniMode")
		{
			GameModeList.InfiniMode (true, hostData);
		}
	}

	// On Server Joined
	void OnConnectedToServer()
	{
		Debug.LogError  ("Server Joined");
		MainMenuBehaviour.screenFader.ChangeScene (GlobalValues.acces.gameMode);
	}
}
