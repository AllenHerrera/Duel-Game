using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mainPanel : menuPanel {
    private Text playerCode;
    private InputField challengeCodeField;
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
				uiController.instance.ShowPanel(uiController.instance.OptionsPanel);
				break;
					}
            case ButtonAction.quit:
                throw new System.NotImplementedException();
        }
    }
    public override void TransitionIn()
    {
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
}