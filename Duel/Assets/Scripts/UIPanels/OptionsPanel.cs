using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsPanel : menuPanel {
	public Slider characterSlider;
	public Slider musicSlider;
	public Slider volumeSlider;
	public Scrollbar soundScrollbar;

	public float characterIndex;
	public float music;
	public float sound;
	public float volume;

	public string playerName;
	public Text volumeText;
	public Text soundText;

	public Sprite[] characterSprites;
	public Sprite currentSprite;

	void Awake()
	{

		characterSprites = Resources.LoadAll<Sprite> ("Sprites");


	}
	// Use this for initialization
	void Start () {
		
			base.Start();
		characterIndex = PlayerPrefs.GetInt("savedCharacter");
		if (characterIndex == null) characterIndex = 0;
		music= PlayerPrefs.GetInt("savedMusic");
			if (music == null) music = 0;
		sound= PlayerPrefs.GetInt("savedSound");
			if (sound == null) sound = 1;
		volume = PlayerPrefs.GetInt ("savedVolume");
			if (volume == null) volume = 50;

		currentSprite = characterSprites [(int)characterIndex];
		GameObject.FindWithTag ("CurrentSprite").GetComponent<SpriteRenderer> ().sprite = currentSprite;
	}
	

	public void _UpdateAll () {

		volumeText.text = volumeSlider.value.ToString ();
		characterIndex = characterSlider.value;
		music = musicSlider.value;
		volume = volumeSlider.value;
		sound = soundScrollbar.value;
		if (sound == 0)
			soundText.text = "Off";
		if (sound == 1)
			soundText.text = "On";
	
	}

	public void UpdateSprite ()
	{
		Debug.Log ("made it motherfucker");
		characterIndex = characterSlider.value;
		currentSprite = characterSprites [(int)characterIndex];
		GameObject.FindWithTag ("CurrentSprite").GetComponent<SpriteRenderer> ().sprite = currentSprite;

	}

	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn) {
		case ButtonAction.returnToMain:
		    {
			
			PlayerPrefs.SetFloat ("savedCharacter", characterIndex);
			PlayerPrefs.SetFloat ("savedMusic", music);
			PlayerPrefs.SetFloat ("savedSound", sound);
			PlayerPrefs.SetFloat ("savedVolume", volume);
			PlayerPrefs.SetString ("playerProfile", playerName);
				// transition to mainpanel
			Debug.Log(PlayerPrefs.GetFloat("savedVolume"));
			Debug.Log(PlayerPrefs.GetFloat("savedSound"));
			Debug.Log(PlayerPrefs.GetFloat("savedCharacter"));
			Debug.Log(PlayerPrefs.GetFloat("savedMusic"));
			uiController.instance.ShowPanel (uiController.instance.MainPanel);

			break;
			}
		}
	}
	
	public void returnToMain()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}



}
