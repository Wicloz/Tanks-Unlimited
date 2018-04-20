using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommentManager : MonoBehaviour
{
	[System.Serializable]
	public class CommentData
	{
		public string passWord;
		public string hostName;
		public List<string> users;
		public string gamemode;
		public string protocol;
		
		public CommentData (string passWord, string hostName, List<string> users, string gamemode, string protocol)
		{
			this.passWord = passWord;
			this.hostName = hostName;
			this.users = users;
			this.gamemode = gamemode;
			this.protocol = protocol;
		}
		
		public CommentData ()
		{
			users = new List<string>();
		}
	}

	public static string SerialiseComment (string passWord, string hostName, List<string> users, string gamemode, string protocol)
	{
		CommentData commentData = new CommentData (passWord, hostName, users, gamemode, protocol);
		return SerializerHelper.SerializeToString (commentData);
	}

	public static CommentData GetCommentData (string comment)
	{
		return SerializerHelper.DeserializeFromString<CommentData> (comment);
	}
}
