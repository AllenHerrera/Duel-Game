using UnityEngine;
using System.Collections;

public class aiPanel : menuPanel {
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);

        switch (btn)
        {
            case ButtonAction.aiMatch:
                socketController.instance.requestAIMatch();
                break;
        }
    }
    public void PlayVsAi()
    {
        ProcessButtonPress(ButtonAction.aiMatch);
    }
    public void No()
    {
        ProcessButtonPress(ButtonAction.challenge);
    }
}
