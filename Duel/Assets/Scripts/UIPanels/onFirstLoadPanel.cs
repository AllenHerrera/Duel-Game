using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class onFirstLoadPanel : menuPanel {
	private InputField playerNameField;
	private string _playerName = null;
    private GameObject TutorialPanel;
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
		base.Start();
	    TutorialPanel = GameObject.Find("TutorialPanel");
		playerNameField = GameObject.Find("EnterNameField").GetComponent<InputField>();
	}

	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn)
		{
		case ButtonAction.returnToMain:
			if (_playerName != null && _playerName.Length>1)
			{
				PlayerPrefs.SetString ("playerProfile", playerName);
				PlayerPrefs.SetInt ("wins", 0);
				PlayerPrefs.SetInt ("losses", 0);
				PlayerPrefs.SetInt ("rating", 0);
				// transition to mainpanel

				uiController.instance.ShowPanel(uiController.instance.MainPanel);
				uiController.instance.MainPanel.playerName = PlayerPrefs.GetString ("playerProfile");
			}
			break;
		}
	}

	public void continu()
	{
		ProcessButtonPress(ButtonAction.returnToMain);
	}

	public override void TransitionIn(){
		//Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
		//Debug.Log ("og firstload: " + PlayerPrefs.GetInt ("firstLoad"));
		if (PlayerPrefs.GetInt ("firstLoad")!= null)
			PlayerPrefs.SetInt ("firstLoad", (PlayerPrefs.GetInt ("firstLoad")+1));
		else
			PlayerPrefs.SetInt ("firstLoad", 0);

	    if (PlayerPrefs.GetString("playerProfile") != "")
	    {
	        uiController.instance.ShowPanel(uiController.instance.MainPanel);
	        return;
	    }
	    base.TransitionIn ();
	}

    public override void TransitionOut()
    {
        base.TransitionOut();
        TutorialPanel.SetActive(false);
    }
}