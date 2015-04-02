﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class OptionsPanel : menuPanel {
	public Slider characterSlider;
	public Slider musicSlider;
	public Slider volumeSlider;
	public Scrollbar soundScrollbar;

	public float characterIndex;
	public float music;
	public float volume;

	public bool soundmute;

	public string playerName;
	public Text PlayerName;
	public Text volumeText;
	public Text soundText;

	public Sprite[] characterSprites;
	public Sprite currentSprite;

	public AudioClip[] audioClips;
	public AudioClip currentAudioClip;
	
	// Use this for initialization
	void Start () {

		Debug.Log(PlayerPrefs.GetFloat("savedVolume"));
		Debug.Log(PlayerPrefs.GetFloat("savedSound"));
		Debug.Log(PlayerPrefs.GetFloat("savedCharacter"));
		Debug.Log(PlayerPrefs.GetFloat("savedMusic"));
		Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));

		GameObject.FindWithTag("PlayerName").GetComponent<Text>().text = PlayerPrefs.GetString("playerProfile");
		setDynamicSliders ();

		base.Start();

		loadSavedPlayerPrefs ();
		_UpdateALL ();
	}

	public void setDynamicSliders ()
	{

		audioClips = Resources.LoadAll<AudioClip> ("Audio");
		characterSprites = Resources.LoadAll<Sprite> ("Sprites");

		musicSlider.GetComponent<Slider> ().maxValue = audioClips.Length -1;
		characterSlider.GetComponent<Slider> ().maxValue = characterSprites.Length-1;
	}

	public void loadSavedPlayerPrefs ()
	{
		playerName = PlayerPrefs.GetString ("playerProfile");
		//get saved music
		music= PlayerPrefs.GetInt("savedMusic");
		if (music == 0) 
			{
				music = 50;
			}
		Debug.Log ("here is the value of music:  " + music);
		musicSlider.value = music;

		//get saved mute value
		if (PlayerPrefs.GetInt ("savedSound") == 0 || PlayerPrefs.GetInt ("savedSound") > 1) {
			soundmute = false;
			soundScrollbar.value = 1;
		} else {
			soundmute = true;
			soundScrollbar.value = 0;
		}

		//get saved volume
		volume = PlayerPrefs.GetInt ("savedVolume");
		if (volume == 0) 
			{
				volume = 50;
			}
		volumeSlider.value = volume;

		//get saved character
		characterIndex = PlayerPrefs.GetInt("savedCharacter");
		if (characterIndex == 0) 
			{
				characterIndex = 1;
			}
		characterSlider.value = characterIndex;

	}
	public void _UpdateALL()
	{
		UpdateMusic ();
		UpdateSound ();
		UpdateVolume ();
		UpdateSprite ();
	}

	public void UpdateVolume()
	{
		volume = volumeSlider.value;
		volumeText.text = " Volume: " +volumeSlider.value.ToString ();
		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().volume = (volume / 100);
	}

	public void UpdateSound()
	{
		if (soundScrollbar.value <= 0.5) 
		{	
			PlayerPrefs.SetFloat ("savedSound", 1);
			soundmute = true;
			soundText.text = "Music: Off";
		}
		if (soundScrollbar.value >= 0.5) 
		{
			PlayerPrefs.SetFloat ("savedSound", 2);
			soundmute = false;
			soundText.text = "Music: On";
		}

		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().mute = soundmute;
	}
	
	public void UpdateMusic()
	{
		music = musicSlider.value;
		currentAudioClip = audioClips [(int)music];
		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().clip = currentAudioClip;
		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().Play();

	}

	public void UpdateSprite ()
	{
		characterIndex = characterSlider.value;
		currentSprite = characterSprites [(int)characterIndex];
		GameObject.FindWithTag ("CurrentSprite").GetComponent<SpriteRenderer> ().sprite = currentSprite;

	}

	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn) {
		case ButtonAction.returnToMain:
		    {
			GameObject.FindWithTag("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = false;
			GameObject.FindGameObjectWithTag ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-745, -420), 0.6f, true);
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);
			Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
				// transition to mainpanel
			Debug.Log(PlayerPrefs.GetFloat("savedVolume"));
			Debug.Log(PlayerPrefs.GetFloat("savedSound"));
			Debug.Log(PlayerPrefs.GetFloat("savedCharacter"));
			Debug.Log(PlayerPrefs.GetFloat("savedMusic"));
			uiController.instance.ShowPanel (uiController.instance.MainPanel);


			}
			break;
		case ButtonAction.options:
		{
			GameObject.FindWithTag("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = false;
			GameObject.FindGameObjectWithTag ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-745, -420), 0.6f, true);
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);


			uiController.instance.ShowPanel (uiController.instance.ChangeNamePanel);
		
		}
			break;
		}
	}
	
	public void returnToMain()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}

	public void ToChangeNamePanel()
	{
		ProcessButtonPress(ButtonAction.options);
	}



}
