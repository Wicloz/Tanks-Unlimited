using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class SerializerHelper : MonoBehaviour
{
	public static string SerializeToString<T> (T classObject)
	{
		XmlSerializer xs = new XmlSerializer (typeof(T));
		StringWriter textWriter = new StringWriter ();
		
		xs.Serialize (textWriter, classObject);
		
		return textWriter.ToString();
	}
	
	public static T DeserializeFromString<T> (string comment)
	{
		XmlSerializer xs = new XmlSerializer (typeof(T));
		StringReader textReader = new StringReader (comment);
		
		T data = (T) xs.Deserialize (textReader);
		
		textReader.Close();
		return data;
	}
}
