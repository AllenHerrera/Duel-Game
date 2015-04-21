using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class OptionsPanel : menuPanel {
    private animationController player;
	public Slider characterSlider;
	public Slider musicSlider;
	public Slider volumeSlider;
	public Scrollbar soundScrollbar;
	
	public float characterIndex;
	public float music;
	public float volume;
	
	public bool soundmute;
	public bool updateScrollBar = false;
	public bool updateCharacterBar = false;
	public float CharacterPreviousValue = 1;
	public int count2 = 0;
	public bool PreviousScrollerState = false;
	public float PreviousValue = 1;
	public int count = 0;
	

	public Text volumeText;
	public Text soundText;

	public Text goldText;
	public Text ratingText;
	public Text winsText;
	public Text lossesText;
	public Text winRatioText;
	public Text winStreakText;
	
	public Sprite[] characterSprites;
	public Sprite currentSprite;
	
	public AudioClip[] audioClips;
	public AudioClip currentAudioClip;
	
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player1").GetComponent<animationController>();
		//Debug.Log("savedVolume: " + PlayerPrefs.GetFloat("savedVolume"));
		//Debug.Log("savedSound: " + PlayerPrefs.GetFloat("savedSound"));
		//Debug.Log("savedCharacter: " + PlayerPrefs.GetFloat("savedCharacter"));
		//Debug.Log("savedMusic: " + PlayerPrefs.GetFloat("savedMusic"));
		//Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
		//Debug.Log ("gold is: " + PlayerPrefs.GetInt ("gold"));
		//Debug.Log ("wins is: " + PlayerPrefs.GetInt ("wins"));
		//Debug.Log ("losses is: " + PlayerPrefs.GetInt ("losses"));
		//Debug.Log ("firstloadvalue : " + PlayerPrefs.GetInt ("firstLoad"));
		goldText =GameObject.Find("GoldNumber").GetComponent<Text>();
		ratingText =GameObject.Find("RatingNumber").GetComponent<Text>();
		winsText =GameObject.Find("WinsNumber").GetComponent<Text>();
		lossesText =GameObject.Find("LossesNumber").GetComponent<Text>();
		winRatioText =GameObject.Find("WinRateNumber").GetComponent<Text>();
		winStreakText = GameObject.Find ("StreakNumber").GetComponent<Text> ();

		volumeText =GameObject.Find("VolumeTextLabel").GetComponent<Text>();
		soundText =GameObject.Find("SoundTextLabel").GetComponent<Text>();

//		characterSlider =GameObject.Find("CharacterSlider").GetComponent<Slider>();
		musicSlider =GameObject.Find("MusicSlider").GetComponent<Slider>();
		volumeSlider =GameObject.Find("VolumeSlider").GetComponent<Slider>();
		soundScrollbar =GameObject.Find("SoundScrollbar").GetComponent<Scrollbar>();


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
		winStreakText.text = PlayerPrefs.GetInt ("maxWinStreak").ToString ();;
		CheckNullPlayerPrefs ();
		GameObject.Find("ProfileMenuPlayerName").GetComponent<Text>().text =PlayerPrefs.GetString ("playerProfile");
		//get saved music
		music= PlayerPrefs.GetFloat("savedMusic");
		if (music == null) 
		{
			music = 0;
		}
		
		musicSlider.value = music;
		
		//get saved mute value
		if (PlayerPrefs.GetFloat ("savedSound") == 0 || PlayerPrefs.GetFloat ("savedSound") > 1) {
			soundmute = false;
			soundScrollbar.value = 1;
		} else {
			soundmute = true;
			soundScrollbar.value = 0;
		}
		
		//get saved volume
		volume = PlayerPrefs.GetFloat ("savedVolume");
		if (volume == 0) 
		{
			volume = 50;
		}
		volumeSlider.value = volume;
		
		//get saved character
		characterIndex = PlayerPrefs.GetFloat("savedCharacter");
		if (characterIndex == null) 
		{
			characterIndex = 1;
		}
		characterSlider.value = characterIndex;
		CharacterPreviousValue = PlayerPrefs.GetFloat("savedCharacter");
		
	}
	public void CheckNullPlayerPrefs()
	{
		if (PlayerPrefs.GetInt("gold") == null) 
		{
			PlayerPrefs.SetInt("gold", 0);
		}
		if (PlayerPrefs.GetInt("wins") == null) 
		{
			PlayerPrefs.SetInt("wins", 0);
		}
		if (PlayerPrefs.GetInt("losses") == null) 
		{
			PlayerPrefs.SetInt("losses", 0);
		}
		if (PlayerPrefs.GetInt("rating") == null) 
		{
			PlayerPrefs.SetInt("rating", 0);
			PlayerPrefs.GetInt("rating");
		}
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
		GameObject.Find ("BG").GetComponent<AudioSource> ().volume = (volume / 100);
	}
	
	public void UpdateSound()
	{ 	updateScrollBar = true;
		count = 10;
		
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
		
		GameObject.Find ("BG").GetComponent<AudioSource> ().mute = soundmute;
	}
	public void UpdateMusic()
	{	GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
		
		music = musicSlider.value;
		currentAudioClip = audioClips [(int)music];
		GameObject gameObject = GameObject.Find ("BG");
		gameObject.GetComponent<AudioSource> ().clip = currentAudioClip;
		gameObject.GetComponent<AudioSource> ().Play();
		
	}

    public void UpdateSprite()
    {
        
        {
            if (updateCharacterBar == false)
            {

                count2 = 12;
            }
            updateCharacterBar = true;
            characterIndex = characterSlider.value;
        }
    }
	
	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn) {
		case ButtonAction.returnToMain:
		{
			GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
			
			GameObject.Find("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = false;
//			GameObject.Find("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-745, -420), 0.6f, true);
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);

			// transition to mainpanel
			
			uiController.instance.ShowPanel (uiController.instance.MainPanel);
			
			
		}
			break;
		case ButtonAction.options:
		{
			GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
			
		
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);
			//transition to changenamePanel

			
			uiController.instance.ShowPanel (uiController.instance.ChangeNamePanel);
			
		}
			break;

		case ButtonAction.quit:
		{ //transition to customize avatar
			GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();

			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);

			
			uiController.instance.ShowPanel (uiController.instance.CustomizeAvatarPanel);
			
		}
			break;
		}
	}
	public bool CheckIfChange (bool ScrollerState, float value)
	{
		if (PreviousScrollerState == ScrollerState)
			return false;
		else {
			// change from 1 to 0 or from 0 to 1
			if (PreviousValue > 0.5 && value < 0.5 || PreviousValue < 0.5 && value >= 0.5)
			{
				
				PreviousScrollerState = ScrollerState;
				PreviousValue = value;
				return true;
				
			}
			else // no change in direction
			{return false;
			}
		}
		
	}
	//add a buy button that changes a value from player prefs
	// have scroll bar scroll away if invalid skin
	public void returnToMain()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}
	
	public void ToChangeNamePanel()
	{
		ProcessButtonPress(ButtonAction.options);
	}
	public void ToCustomizeAvatarPanel()
	{
		ProcessButtonPress(ButtonAction.quit);
	}
	void Update()
	{ 	
		//move characterscrollbar      still need to check that prev isnt the same so you dont double beep
		if (count2 > 0) {

			if (characterSlider.value % 1 < 0.5)
				characterSlider.value = characterSlider.value - ((characterSlider.value % 1)/12);
			else
				characterSlider.value = characterSlider.value + (( 1-(characterSlider.value%1))/12);

			count2--;
			if (count2 == 0)
				if (characterSlider.value%1 < 0.5) {

				characterSlider.value = (int)characterSlider.value;
			
				updateCharacterBar = false;
				if(characterSlider.value != CharacterPreviousValue){
				GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
					CharacterPreviousValue = characterSlider.value;
				}
			}
				else 
			{
				characterSlider.value= (int)characterSlider.value+1;
				updateCharacterBar = false;
				if(characterSlider.value != CharacterPreviousValue){
					GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
					CharacterPreviousValue = characterSlider.value;
				}
			}
		}

		//movemute scrollbar
		if (count > 0) {
			if (soundScrollbar.value > 0.5) {
				
				soundScrollbar.value = (soundScrollbar.value + ((1 - soundScrollbar.value) / 15));
			} else {
				soundScrollbar.value = soundScrollbar.value - (soundScrollbar.value) / 15;
				
			}
			if(CheckIfChange(updateScrollBar,soundScrollbar.value)==true)
				GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
			count --;
			if (count == 0)
			{
				if (soundScrollbar.value > 0.5) 
					
					soundScrollbar.value = 1;
				else 
					soundScrollbar.value = 0;
			}	
			
			updateScrollBar = false;
		}
	}

	public override void TransitionIn()
	{	

		uiController.instance.MainPanel.PlayerNameText.gameObject.SetActive (false);

		uiController.instance.MainPanel.playerName = PlayerPrefs.GetString ("playerProfile");
		GameObject.Find("ProfileMenuPlayerName").GetComponent<Text>().text =PlayerPrefs.GetString ("playerProfile");

		winStreakText.text = PlayerPrefs.GetInt ("maxWinStreak").ToString();
		goldText.text = PlayerPrefs.GetInt ("gold").ToString();
		//ratingText.text = "N/A";
		ratingText.text = PlayerPrefs.GetInt ("rating").ToString();
		winsText.text = ((float)PlayerPrefs.GetInt ("wins")).ToString();
		lossesText.text = PlayerPrefs.GetInt ("losses").ToString();

		if (PlayerPrefs.GetInt ("losses") + PlayerPrefs.GetInt ("wins") != 0) {

			int temp= (int)(((double)PlayerPrefs.GetInt ("wins") / (double)(PlayerPrefs.GetInt ("losses") + PlayerPrefs.GetInt ("wins"))) * 100);
			      
			winRatioText.text = (temp.ToString()+ "%"); 
		}
		else
			winRatioText.text = "N/A";


		base.TransitionIn();
	}

	
	
}