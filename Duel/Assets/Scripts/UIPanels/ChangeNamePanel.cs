using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeNamePanel : menuPanel {
private InputField playerNameField;

	private string _playerName;
// Use this for initialization

public string playerName
{
	get
	{
		return _playerName;
	}
	set
	{
		_playerName=value;
		playerNameField.text = _playerName;
	}
}

protected override void Start()
{
	
	 //_playerName = PlayerPrefs.GetString("playerProfile");

	base.Start();
	playerNameField = GameObject.Find("EnterNewNameField").GetComponent<InputField>();
}

protected override void ProcessButtonPress(ButtonAction btn)
{
	switch (btn)
	{
	case ButtonAction.returnToMain:
			if (playerName != null && playerName != "" && playerName != "Invalid Name")
		{
			PlayerPrefs.SetString ("playerProfile", playerName);
				GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
				uiController.instance.MainPanel.playerName = PlayerPrefs.GetString ("playerProfile");//does this work?

				GameObject.Find("ProfileMenuPlayerName").GetComponent<Text>().text =PlayerPrefs.GetString ("playerProfile");
			// transition back to optionpanel
				GameObject.Find ("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = true;
//				GameObject.Find ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-645, -290), 0.5f, true);
			uiController.instance.ShowPanel(uiController.instance.OptionsPanel);
		}
		else
				GameObject.Find("EnterNewNameField").GetComponent<InputField>().text = "Invalid Name";
		break;
	case ButtonAction.quit:
			GameObject.Find("OptionsPanel").GetComponent<AudioSource>().Play();
			GameObject.Find ("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = true;
//			GameObject.Find ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-645, -290), 0.5f, true);
			// transition back to optionpanel without saving
			uiController.instance.ShowPanel(uiController.instance.OptionsPanel);
		
		break;
	}
}

public void continu()
{
	ProcessButtonPress(ButtonAction.returnToMain);
}

public void cancel()
{
	ProcessButtonPress(ButtonAction.quit);
}

public override void TransitionIn()
{	
		
		GameObject.Find ("EnterNewNameField").GetComponent<InputField> ().text = "";
		GameObject.Find ("PlaceholderNEWNAME").SetActive (true);
		
		base.TransitionIn();
}



}