using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class mainPanel : menuPanel {
    private Text playerCode;
    private InputField challengeCodeField;
	public GameObject Skeleton;
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
		//Skeleton = GameObject.Find ("Skeleton");
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
                throw new System.NotImplementedException();
            case ButtonAction.leaderboard:
                throw new System.NotImplementedException();
          
			case ButtonAction.options:
		    {		
				Skeleton.SetActive(false);
				Debug.Log ("Playername is: " + PlayerPrefs.GetString ("playerProfile"));
				GameObject.FindGameObjectWithTag ("CurrentSprite").GetComponent<SpriteRenderer> ().enabled = true;
				GameObject.FindGameObjectWithTag ("CharacterSlider").GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-645, -290), 0.5f, true);
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
		Skeleton.SetActive(true);
        playerCode.text = socketController.instance.playerCode;
        base.TransitionIn();
    }
    public override void TransitionOut()
    {
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