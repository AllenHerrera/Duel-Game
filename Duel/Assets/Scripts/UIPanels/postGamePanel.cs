using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class postGamePanel : menuPanel
{
    private Text OutcomeTitle, OutcomeText;
    protected override void Start()
    {
        base.Start();
        OutcomeTitle = GameObject.Find("OutcomeTitle").GetComponent<Text>();
        OutcomeText = GameObject.Find("OutcomeText").GetComponent<Text>();
    }
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        switch (btn)
        {
            case ButtonAction.returnToMain:
                if(socketController.instance.inMatchmaking)
                    socketController.instance.resetGame();
                else
                    socketController.instance.rejectChallenge();
                uiController.instance.ShowPanel(uiController.instance.MainPanel);
                break;
            case ButtonAction.playAgain:
                if(socketController.instance.inMatchmaking)
                    socketController.instance.findMatch();
                else
                    socketController.instance.challenge();
                uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);
                break;
        }
    }
    public void PlayAgain()
    {
        ProcessButtonPress(ButtonAction.playAgain);
    }
    public void Quit()
    {
        ProcessButtonPress(ButtonAction.returnToMain);
    }
    public override void TransitionIn()
    {
        if (gameController.instance.wonGame)
        {
            OutcomeTitle.text = "You Won!";
            OutcomeText.text = "You're the best! :)";
			PlayerPrefs.SetInt("wins",(PlayerPrefs.GetInt("wins") + 1));
			PlayerPrefs.SetInt("gold",(PlayerPrefs.GetInt("gold") + 2));
        }
        else
        {
            OutcomeTitle.text = "You Lost!";
            OutcomeText.text = "You're the worst! :(";
			PlayerPrefs.SetInt("losses",(PlayerPrefs.GetInt("losses") + 1));
			PlayerPrefs.SetInt("gold",(PlayerPrefs.GetInt("gold") + 1));
        }
        base.TransitionIn();
    }
}