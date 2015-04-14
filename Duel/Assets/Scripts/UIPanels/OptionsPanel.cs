using UnityEngine;
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
	public bool updateScrollBar = false;
	public bool PreviousScrollerState = false;
	public float PreviousValue = 1;
	public int count = 0;
	
	public string playerName;
	public Text PlayerName;
	public Text volumeText;
	public Text soundText;

	public Text goldText;
	public Text ratingText;
	public Text winsText;
	public Text lossesText;
	public Text winRatioText;
	
	public Sprite[] characterSprites;
	public Sprite currentSprite;
	
	public AudioClip[] audioClips;
	public AudioClip currentAudioClip;
	
	// Use this for initialization
	void Start () {
		
		Debug.Log("savedVolume: " + PlayerPrefs.GetFloat("savedVolume"));
		Debug.Log("savedSound: " + PlayerPrefs.GetFloat("savedSound"));
		Debug.Log("savedCharacter: " + PlayerPrefs.GetFloat("savedCharacter"));
		Debug.Log("savedMusic: " + PlayerPrefs.GetFloat("savedMusic"));
		Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
		Debug.Log ("gold is: " + PlayerPrefs.GetInt ("gold"));
		Debug.Log ("wins is: " + PlayerPrefs.GetInt ("wins"));
		Debug.Log ("losses is: " + PlayerPrefs.GetInt ("losses"));
		Debug.Log ("firstloadvalue : " + PlayerPrefs.GetInt ("firstLoad"));

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
		CheckNullPlayerPrefs ();

		playerName = PlayerPrefs.GetString ("playerProfile");
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
		if (characterIndex == 0) 
		{
			characterIndex = 1;
		}
		characterSlider.value = characterIndex;
		
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
		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().volume = (volume / 100);
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
		
		GameObject.FindGameObjectWithTag ("BackgroundMusic").GetComponent<AudioSource> ().mute = soundmute;
	}
	public void UpdateMusic()
	{	GameObject.FindGameObjectWithTag("OptionBtnSound").GetComponent<AudioSource>().Play();
		
		music = musicSlider.value;
		currentAudioClip = audioClips [(int)music];
		GameObject gameObject = GameObject.FindGameObjectWithTag ("BackgroundMusic");
		gameObject.GetComponent<AudioSource> ().clip = currentAudioClip;
		gameObject.GetComponent<AudioSource> ().Play();
		
	}
	
	public void UpdateSprite ()
	{	GameObject.FindGameObjectWithTag("OptionBtnSound").GetComponent<AudioSource>().Play();
		
		characterIndex = characterSlider.value;
		currentSprite = characterSprites [(int)characterIndex];
		GameObject.FindWithTag ("CurrentSprite").GetComponent<SpriteRenderer> ().sprite = currentSprite;
		
	}
	
	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn) {
		case ButtonAction.returnToMain:
		{
			GameObject.FindGameObjectWithTag("OptionBtnSound").GetComponent<AudioSource>().Play();
			
			GameObject.FindWithTag("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = false;
			GameObject.FindGameObjectWithTag ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-745, -420), 0.6f, true);
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);
			Debug.Log ("Playername going into main is: " + PlayerPrefs.GetString ("playerProfile"));
			// transition to mainpanel
			
			uiController.instance.ShowPanel (uiController.instance.MainPanel);
			
			
		}
			break;
		case ButtonAction.options:
		{
			GameObject.FindGameObjectWithTag("OptionBtnSound").GetComponent<AudioSource>().Play();
			
		
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedVolume", volume);
			//transition to changenamePanel
			Debug.Log ("Playername going into changename: " + PlayerPrefs.GetString ("playerProfile"));
			Debug.Log("savedVolume: " +PlayerPrefs.GetFloat("savedVolume"));
			Debug.Log("savedSound: " +PlayerPrefs.GetFloat("savedSound"));
			Debug.Log("savedcharacter: " +PlayerPrefs.GetFloat("savedCharacter"));
			Debug.Log("savedMusic: " +PlayerPrefs.GetFloat("savedMusic"));
			
			uiController.instance.ShowPanel (uiController.instance.ChangeNamePanel);
			
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
	
	public void returnToMain()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}
	
	public void ToChangeNamePanel()
	{
		ProcessButtonPress(ButtonAction.options);
	}
	void Update()
	{ 
		
		if (count > 0) {
			if (soundScrollbar.value > 0.5) {
				
				soundScrollbar.value = (soundScrollbar.value + ((1 - soundScrollbar.value) / 12));
			} else {
				soundScrollbar.value = soundScrollbar.value - (soundScrollbar.value) / 12;
				
			}
			if(CheckIfChange(updateScrollBar,soundScrollbar.value)==true)
				GameObject.FindGameObjectWithTag("OptionBtnSound").GetComponent<AudioSource>().Play();
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
		goldText.text = PlayerPrefs.GetInt ("gold").ToString();
		//ratingText.text = "N/A";
		ratingText.text = PlayerPrefs.GetInt ("rating").ToString();
		winsText.text = PlayerPrefs.GetInt ("wins").ToString();
		lossesText.text = PlayerPrefs.GetInt ("losses").ToString();

		if (PlayerPrefs.GetInt ("losses") + PlayerPrefs.GetInt ("wins") != 0)
			winRatioText.text = ((((double)PlayerPrefs.GetInt ("wins") / (double)(PlayerPrefs.GetInt ("losses") + PlayerPrefs.GetInt ("wins"))) * 100).ToString () + "%");
		else
			winRatioText.text = "N/A";


		base.TransitionIn();
	}

	
	
}
