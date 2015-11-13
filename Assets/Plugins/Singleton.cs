using UnityEngine;
using System.Collections;

public class PersistentSingleton <T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;
	public bool assinged = false;

	protected T _acces
	{
		set
		{
			instance = value;
			assinged = true;
		}
	}

	public static T acces
	{
		get
		{
			return instance;
		}
	}

	public void Start ()
	{
		if (assinged)
		{
			_Start();
		}
	}

	public virtual void _Start ()
	{}

	public void OnDestroy ()
	{
		if (assinged)
		{
			_OnDestroy();
		}
	}

	public virtual void _OnDestroy ()
	{}
}