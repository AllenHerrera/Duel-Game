using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class onFirstLoadPanel : menuPanel {
	private InputField playerNameField;
	private string _playerName = null;
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
		PlayerPrefs.SetInt ("firstLoad", 0);
		base.Start();
		playerNameField = GameObject.Find("EnterNameField").GetComponent<InputField>();
	}

	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn)
		{
		case ButtonAction.returnToMain:
			if (playerName != "")
			{
				PlayerPrefs.SetString ("playerProfile", playerName);
				// transition to mainpanel
				uiController.instance.ShowPanel(uiController.instance.MainPanel);
			}
			break;
		}
	}

	public void continu()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}

	public override void TransitionIn(){
		Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
		if ( PlayerPrefs.GetString ("playerProfile") != "" ) {
			Debug.Log ("why");
			uiController.instance.ShowPanel (uiController.instance.MainPanel);
			return;
	}
		base.TransitionIn ();
	}
}