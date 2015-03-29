using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class challengedPanel : menuPanel {
    private Text challengedMessage;
    protected override void Start()
    {
        base.Start();
        challengedMessage = GameObject.Find("ChallengedText").GetComponent<Text>();
    }
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        switch (btn)
        {
            case ButtonAction.declineChallenge:
                socketController.instance.rejectChallenge();
                uiController.instance.ShowPanel(uiController.instance.MainPanel);
                break;
            case ButtonAction.acceptChallenge:
                socketController.instance.acceptChallenge();
                //uiController.instance.ShowPanel
                break;
        }
    }
    public void Accept()
    {
        ProcessButtonPress(ButtonAction.acceptChallenge);
    }        
    public void Decline()
    {
        ProcessButtonPress(ButtonAction.declineChallenge);
    }
    public override void TransitionIn()
    {
        base.TransitionIn();
        challengedMessage.text = string.Format("You have been challenged by code {0}", socketController.instance.challengerCode);
    }
}