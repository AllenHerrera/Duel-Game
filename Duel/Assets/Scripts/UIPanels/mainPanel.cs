using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class mainPanel : menuPanel {
    private Text playerCode;
    private InputField challengeCodeField;
	public Text PlayerNameText;
	//private Text PlayerNameText = GameObject.Find ("PlayerNameText");
	public string playerName{ 
		get{ 
			return PlayerNameText.text;
		}
		set{
			PlayerNameText.text = value;
		} 
	}
    private string _challengeCode = "";
    public string challengeCode
    {
        get
        {
            return _challengeCode;
        }
        set
        {
            _challengeCode = value.ToUpper();
            challengeCodeField.text = _challengeCode;
        }
    }
    protected override void Start()
    {


        base.Start();
		PlayerNameText = GameObject.Find ("PlayerNameText").GetComponent<Text>();
		playerName = PlayerPrefs.GetString ("playerProfile");
        playerCode = GameObject.Find("PlayerCode").GetComponent<Text>();
        challengeCodeField = GameObject.Find("ChallengeCodeField").GetComponent<InputField>();

    }
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        switch (btn)
        {
            case ButtonAction.challenge:
                if (challengeCode.Length == 4)
                {
                    socketController.instance.challenge(challengeCode);
                    uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);
                    challengeCodeField.text = "";
                }
                break;
            case ButtonAction.matchmaking:
                socketController.instance.findMatch();
                break;
            case ButtonAction.leaderboard:
                socketController.instance.requestLeaderboard();
                break;
			case ButtonAction.options:
		    {		
				Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
//				GameObject.Find ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-645, -290), 0.5f, true);
				uiController.instance.ShowPanel(uiController.instance.OptionsPanel);
				break;
			}
            case ButtonAction.quit:
               Debug.Log("should quit!");
                Application.Quit();
                break;
        }
    }
    public override void TransitionIn()
    {	

        playerCode.text = socketController.instance.playerCode;
        base.TransitionIn();
    }
    public override void TransitionOut()
    {	
		PlayerNameText.gameObject.SetActive (false);
        base.TransitionOut();
    }
    public void Challenge()
    {
        ProcessButtonPress(ButtonAction.challenge);
    }
    public void Matchmaking()
    {
        ProcessButtonPress(ButtonAction.matchmaking);
    }
    public void Leaderboard()
    {
        ProcessButtonPress(ButtonAction.leaderboard);
    }
	public void Options()
	{
		ProcessButtonPress(ButtonAction.options);
	}
    public void Quit()
    {
        ProcessButtonPress(ButtonAction.quit);
    }
}