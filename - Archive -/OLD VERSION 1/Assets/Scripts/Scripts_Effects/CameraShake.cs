using UnityEngine;
using System.Collections;
using Wicloz.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake acces;
	private Transform camTransform;

	private float shakeAmplitude = 0;
	private float setShakeAmplitude = 0;

	private SimpleDict <string, float> shakeList = new SimpleDict <string, float> ();

	Vector3 originalPos;

	void Awake ()
	{
		acces = this;
	}

	void Start ()
	{
		shakeList.Add ("default", 0);
		camTransform = GetComponent(typeof(Transform)) as Transform;
		originalPos = camTransform.localPosition;
	}
	
	void Update ()
	{
		setShakeAmplitude = shakeList.Acces (0);
		for (int i = 0; i < shakeList.Count; i ++)
		{
			if (shakeList.Acces (i) > setShakeAmplitude)
			{
				setShakeAmplitude = shakeList.Acces (i);
			}
		}

		shakeAmplitude = Mathf.Lerp (shakeAmplitude, setShakeAmplitude, Time.deltaTime * 5);

		if (FirstPerson.acces.inFirstPerson)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmplitude / 10;
		}
		else
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmplitude;
		}
	}

	public void SetShake (string className, float amplitue, float duration, float fadeTime)
	{
		StartCoroutine (SetShakeI (className, amplitue, duration, fadeTime));
	}

	IEnumerator SetShakeI (string className, float amplitue, float duration, float fadeTime)
	{
		SetAmplitude (className, amplitue);
		yield return new WaitForSeconds (duration);

		if (fadeTime == 0)
		{
			Unsubscribe (className);
		}

		else
		{
			float decreasePS = amplitue / fadeTime;

			while (amplitue > 0f)
			{
				amplitue -= decreasePS * Time.deltaTime;
				CameraShake.acces.SetAmplitude (className, amplitue);
				yield return null;
			}

			Unsubscribe (className);
		}
	}

	public void SetAmplitude (string className, float amplitue)
	{
		if (!shakeList.ContainsKey (className))
		{
			shakeList.Add (className, amplitue);
		}
		else
		{
			shakeList.Edit (className, amplitue);
		}
	}

	public void Unsubscribe (string className)
	{
		if (shakeList.ContainsKey (className))
		{
			shakeList.Remove (className);
		}
	}
}
