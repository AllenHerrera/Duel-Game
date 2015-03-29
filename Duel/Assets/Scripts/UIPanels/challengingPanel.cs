using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class challengingPanel : menuPanel
{
    private Text challengingText, challengingTitle;
    protected override void Start()
    {
        base.Start();
        challengingTitle = GameObject.Find("ChallengingTitle").GetComponent<Text>();
        challengingText = GameObject.Find("ChallengingText").GetComponent<Text>();
        //animate challenging title
        challengingTitle.DOText("Challenging...", 1.5f, false).SetLoops(-1, LoopType.Yoyo);
    }
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        switch (btn)
        {
            case ButtonAction.cancelChallenge:
                socketController.instance.cancelChallenge();
                uiController.instance.ShowPanel(uiController.instance.MainPanel);
                break;
        }
    }
    public void Cancel()
    {
        ProcessButtonPress(ButtonAction.cancelChallenge);
    }
    public override void TransitionIn()
    {
        base.TransitionIn();
        challengingText.text = string.Format("{0} has been challenged. Awaiting response.", socketController.instance.challengedCode);
    }
}