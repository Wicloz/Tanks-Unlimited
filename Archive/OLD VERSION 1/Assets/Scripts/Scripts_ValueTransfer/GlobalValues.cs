using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GlobalValues : MonoBehaviour
{
	public static GlobalValues acces;
	public static Properties properties;
	public static PlayerData playerData;
	
	// PlayerData
	public string userName = "";
	public float totalScore;

	// PlayerPrefs
	public float musicVolume;
	public bool firstPerson;

	// Level generation data
	public float chance;
	public int enemyTanks;
	public int difficulty;
	public string gameMode;
	
	// Camera position saving
	public Vector3 firstPersonCameraPosition;
	public Quaternion firstPersonCameraRotation;
	
	// Extra game states
	public bool coOp = false;
	public bool multiplayer = false;

	// Data Moving
	public int lives;
	public int playerAmount;

	void Awake ()
	{
		if (acces == null)
		{
			DontDestroyOnLoad(gameObject);
			acces = this;
			properties = new Properties ();
			playerData = new PlayerData ();
		}
		else if (acces != this)
		{
			Destroy (gameObject);
		}

		GetPrefs ();
		if (userName == "")
		{
			PlayerPrefs.SetFloat ("Music Volume", 0.6f);
			PlayerPrefs.SetString ("User Name", "User");
			GetPrefs ();
		}
		LoadGame ();
	}

	void OnDestroy ()
	{
		SaveGame ();
	}

	void GetPrefs ()
	{
		musicVolume = PlayerPrefs.GetFloat ("Music Volume");
		userName = PlayerPrefs.GetString ("User Name");
	}

	void SetPrefs ()
	{
		PlayerPrefs.SetFloat ("Music Volume", musicVolume);
		PlayerPrefs.SetString ("User Name", userName);
	}
	
	public void LoadGame ()
	{
		SetPrefs ();
		ResetData ();
		
		if (File.Exists(Application.persistentDataPath + "/" + userName + ".sav"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream saveFile = File.Open (Application.persistentDataPath + "/" + userName + ".sav", FileMode.Open);

			playerData = (PlayerData) bf.Deserialize (saveFile);
			saveFile.Close ();

			GetData ();
			SetTankProperties ();
		}
		else
		{
			SaveGame ();
		}
	}

	public void SaveGame ()
	{
		SetPrefs ();

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream saveFile = File.Create (Application.persistentDataPath + "/" + userName + ".sav");

		SetData ();

		bf.Serialize (saveFile, playerData);
		saveFile.Close ();
	}

	void GetData ()
	{
		totalScore = playerData.totalScore;

		firstPerson = playerData.firstPerson;

		firstPersonCameraPosition.x = playerData.firstPersonCameraPositionX;
		firstPersonCameraPosition.y = playerData.firstPersonCameraPositionY;
		firstPersonCameraPosition.z = playerData.firstPersonCameraPositionZ;

		firstPersonCameraRotation.x = playerData.firstPersonCameraRotationX;
		firstPersonCameraRotation.y = playerData.firstPersonCameraRotationY;
		firstPersonCameraRotation.z = playerData.firstPersonCameraRotationZ;
		firstPersonCameraRotation.w = playerData.firstPersonCameraRotationW;
	}

	void SetData ()
	{
		playerData.totalScore = totalScore;

		playerData.firstPerson = firstPerson;

		playerData.firstPersonCameraPositionX = firstPersonCameraPosition.x;
		playerData.firstPersonCameraPositionY = firstPersonCameraPosition.y;
		playerData.firstPersonCameraPositionZ = firstPersonCameraPosition.z;

		playerData.firstPersonCameraRotationX = firstPersonCameraRotation.x;
		playerData.firstPersonCameraRotationY = firstPersonCameraRotation.y;
		playerData.firstPersonCameraRotationZ = firstPersonCameraRotation.z;
		playerData.firstPersonCameraRotationW = firstPersonCameraRotation.w;
	}

	void ResetData ()
	{
		totalScore = 0;

		firstPerson = false;
		firstPersonCameraPosition = new Vector3 (0, 2.0f, 0.4f);
		firstPersonCameraRotation = Quaternion.Euler (10, 0, 0);

		playerData = new PlayerData ();
		SetData ();
	}

	public void SetTankProperties ()
	{
		properties.moveSpeed = 9000 + (playerData.applied_MoveSpeed * 700);
		properties.turnSpeed = 1500 + (playerData.applied_TurnSpeed * 100);
		
		properties.turretTurnSpeed = 2 + playerData.applied_TurretTurnSpeed;
		properties.ammo = 1 + playerData.applied_Ammo;
		properties.reload = 2 - (((float) playerData.applied_Reload) / 5);

//		float reloadSpeed = 4 - (((float) playerData.applied_Reload + 1) / 2);
//		properties.reload = reloadSpeed + (reloadSpeed * (((float) playerData.applied_Ammo) / 5));
		
		properties.shellSpeed = 5 + playerData.applied_ShellSpeed;
		properties.shellBounces = playerData.applied_Bounces;
		
		properties.mineAmount = 1 + playerData.applied_MineAmount;
		properties.mineRadius = 0.8f + ((float) playerData.applied_MineRadius / 10);
	}

	void Update ()
	{
		if (Application.loadedLevel == 0)
		{
			GameObject.Find ("text_points_1").GetComponent <TextMesh>().text = "Total Points: " + totalScore;
			GameObject.Find ("Points Text 2").GetComponent <TypogenicText>().Text = "Total Points: " + totalScore;
			
			GameObject.Find ("text_username").GetComponent <TextMesh>().text = "Welcome " + userName;
		}
	}
}

[Serializable]
public class PlayerData
{
	public float totalScore;

	// Preferences
	public bool firstPerson;

	public float firstPersonCameraPositionX;
	public float firstPersonCameraPositionY;
	public float firstPersonCameraPositionZ;

	public float firstPersonCameraRotationX;
	public float firstPersonCameraRotationY;
	public float firstPersonCameraRotationZ;
	public float firstPersonCameraRotationW;
	
	// Applied Upgrades
	public int applied_MoveSpeed = 0;
	public int applied_TurnSpeed = 0;
	
	public int applied_TurretTurnSpeed = 0;
	public int applied_Ammo = 0;
	public int applied_Reload = 0;
	
	public int applied_ShellSpeed = 0;
	public int applied_Bounces = 0;
	
	public int applied_MineAmount = 0;
	public int applied_MineRadius = 0;
	
	public bool applied_AimLaser = false;
	
	// Unlocked Upgrades
	public int purchased_MoveSpeed = 0;
	public int purchased_TurnSpeed = 0;
	
	public int purchased_TurretTurnSpeed = 0;
	public int purchased_Ammo = 0;
	public int purchased_Reload = 0;
	
	public int purchased_ShellSpeed = 0;
	public int purchased_Bounces = 0;
	
	public int purchased_MineAmount = 0;
	public int purchased_MineRadius = 0;
	
	public bool purchased_AimLaser = false;
}

public class Properties
{
	public float moveSpeed;
	public float turnSpeed;

	public float turretTurnSpeed;
	public int ammo;
	public float reload;

	public float shellSpeed;
	public int shellBounces;

	public int mineAmount;
	public float mineRadius;
}
