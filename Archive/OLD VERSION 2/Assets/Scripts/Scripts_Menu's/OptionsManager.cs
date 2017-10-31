using UnityEngine;
using System.Collections;
using System.IO;

public enum OptionsTab {None, Video, Audio, Cards, Profile};

public class OptionsManager : PersistentSingleton<OptionsManager>
{
	private string lastState = "";
	public InstantGuiElement closeButton;

	private string settingsFile
	{
		get
		{
			if (Application.dataPath != null)
			{
				return Application.dataPath + "\\..\\options.ini";
			}
			else
			{
				return Application.persistentDataPath + "\\options.ini";
			}
		}
	}
	public GameObject optionsParent;
	public InstantGuiTabs tabsParent;

	public bool isOpened
	{
		get
		{
			return optionsParent.activeSelf;
		}
	}

	public InstantGuiPopup resolutionsBox;
	public InstantGuiPopup screenmodeBox;
	public InstantGuiToggle borderlessToggle;
	public InstantGuiPopup qualityBox;

	public InstantGuiSlider musicSlider;
	public InstantGuiSlider effectSlider;

	public InstantGuiElement profileTab;
	public InstantGuiInputText nameField;
	public InstantGuiButton profileLoadButton;
	public InstantGuiToggle SaveProfileLocalToggle;

	private bool loadingProfile = false;
	private string[] lastVideoSettings = new string[3];

	public float volumeSpeed;
	
	public string resolution
	{
		get
		{
			return resolutionsBox.list.labels[resolutionsBox.list.selected];
		}
		set
		{
			for (int i = 0; i < resolutionsBox.list.labels.Length; i++)
			{
				string label = resolutionsBox.list.labels[i];

				if (label == value)
				{
					resolutionsBox.list.selected = i;
					break;
				}
			}
		}
	}
	public string screenMode
	{
		get
		{
			return screenmodeBox.list.labels[screenmodeBox.list.selected];
		}
		set
		{
			for (int i = 0; i < screenmodeBox.list.labels.Length; i++)
			{
				string label = screenmodeBox.list.labels[i];
				
				if (label == value)
				{
					screenmodeBox.list.selected = i;
					break;
				}
			}
		}
	}
	public bool borderless
	{
		get
		{
			return borderlessToggle.check;
		}
		set
		{
			borderlessToggle.check = value;
		}
	}
	public string quality
	{
		get
		{
			return qualityBox.list.labels[qualityBox.list.selected];
		}
		set
		{
			for (int i = 0; i < qualityBox.list.labels.Length; i++)
			{
				string label = qualityBox.list.labels[i];
				
				if (label == value)
				{
					qualityBox.list.selected = i;
					break;
				}
			}
		}
	}

	public float musicVolume
	{
		get
		{
			return musicSlider.value;
		}
		set
		{
			musicSlider.value = value;
		}
	}
	public float effectsVolume
	{
		get
		{
			return effectSlider.value;
		}
		set
		{
			effectSlider.value = value;
		}
	}

	public string currentUsername
	{
		get
		{
			return nameField.text;
		}
		set
		{
			nameField.text = value;
		}
	}
	public bool saveProfileLocal
	{
		get
		{
			return SaveProfileLocalToggle.check;
		}
		set
		{
			SaveProfileLocalToggle.check = value;
		}
	}

	void Awake ()
	{
		if (acces == null)
		{
			_acces = this;
		}
	}

	public override void _Start ()
	{
		LoadOptions ();
		SetVideoSettings ();
		WriteLastSettings ();
		ProfileManager.acces.LoadProfile (currentUsername);
	}

	public override void _OnDestroy ()
	{
		SaveOptions ();
	}

	void Update ()
	{
		if (MultiplayerManager.acces.gameState == GameState.Stopped)
		{
			profileTab.disabled = false;
		}
		else
		{
			profileTab.disabled = true;
		}

		if (lastVideoSettings [0] != this.resolution || lastVideoSettings [1] != this.screenMode || lastVideoSettings [2] != this.quality)
		{
			Debug.Log ("Video Settings Changed, setting new data ...");
			SetVideoSettings ();
		}

		WriteLastSettings ();

		if (profileLoadButton.pressed && !loadingProfile)
		{
			loadingProfile = true;
			ProfileManager.acces.LoadProfile (currentUsername);
		}
		else if (!profileLoadButton.pressed)
		{
			loadingProfile = false;
		}

		if (closeButton.pressed || Input.GetButtonDown ("Cancel"))
		{
			closeButton.pressed = false;
			CloseWindow ();
		}
	}

	public void OpenWindow (string fromState)
	{
		optionsParent.SetActive (true);
		lastState = fromState;
		tabsParent.selected = 0;
	}

	public void CloseWindow ()
	{
		optionsParent.SetActive (false);
		SaveOptions ();

		if (lastState == "EscapeMenu")
		{
			EscapeMenu.acces.OpenWindow();
		}
	}

	private void WriteLastSettings ()
	{
		lastVideoSettings [0] = this.resolution;
		lastVideoSettings [1] = this.screenMode;
		lastVideoSettings [2] = this.quality;
	}

	public void SetVideoSettings ()
	{
		char[] delim = new char[] {'x'};

		int resWidth = System.Convert.ToInt32 (this.resolution.Split(delim)[0].Trim());
		int resHeight = System.Convert.ToInt32 (this.resolution.Split(delim)[1].Trim());
		bool fullscreen = false;
		
		switch (this.quality)
		{
		case "Fastest":
			QualitySettings.SetQualityLevel (0);
			break;
		case "Fast":
			QualitySettings.SetQualityLevel (1);
			break;
		case "Simple":
			QualitySettings.SetQualityLevel (2);
			break;
		case "Good":
			QualitySettings.SetQualityLevel (3);
			break;
		case "Beautiful":
			QualitySettings.SetQualityLevel (4);
			break;
		case "Fantastic":
			QualitySettings.SetQualityLevel (5);
			break;
		}

		if (this.screenMode == "Fullscreen")
		{
			fullscreen = true;
		}
		//if (borderless)
		//{
		//	fullscreen = false;
		//}

		Screen.SetResolution (resWidth, resHeight, fullscreen);
	}

	public void SaveOptions ()
	{
		Settings set = new Settings ();

		set.resolution = this.resolution;
		set.screenMode = this.screenMode;
		set.borderless = this.borderless;
		set.quality = this.quality;

		set.musicVolume = this.musicVolume;
		set.effectsVolume = this.effectsVolume;

		set.lastUsername = this.currentUsername;
		set.saveProfileLocal = this.saveProfileLocal;

		SerializerHelper.SaveFileXml (set, settingsFile);

		if (Application.dataPath != null)
		{
			string shortcut = Application.dataPath + "\\..\\TanksUnlimited.bat";
			string content = "@ECHO OFF\nstart \"Tanks Unlimited.exe\"\nexit";
			if (borderless)
			{
				content = "@ECHO OFF\nstart \"Tanks Unlimited.exe\" -popupwindow\nexit";
			}
			File.WriteAllText (shortcut, content);
		}
		Debug.Log ("Options Saved");
	}

	public void LoadOptions ()
	{
		if (File.Exists (settingsFile))
		{
			Settings set = SerializerHelper.LoadFileXml<Settings> (settingsFile);

			this.resolution = set.resolution;
			this.screenMode = set.screenMode;
			this.borderless = set.borderless;
			this.quality = set.quality;

			this.musicVolume = set.musicVolume;
			this.effectsVolume = set.effectsVolume;

			this.currentUsername = set.lastUsername;
			this.saveProfileLocal = set.saveProfileLocal;
		}

		else
		{
			currentUsername = "USER_" + Random.Range (0, 1000);
			SaveOptions ();
		}
	}
}

[System.Serializable]
public class Settings
{
	// Video
	public string resolution;
	public string screenMode;
	public bool borderless;
	public string quality;

	// Audio
	public float musicVolume;
	public float effectsVolume;

	// Profile
	public string lastUsername;
	public bool saveProfileLocal;

	public Settings ()
	{}
}
