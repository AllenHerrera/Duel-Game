using UnityEngine;
using UnityEngine.UI;

public class pingPanel : menuPanel {
    private Text PingText;
    protected override void Start()
    {
        base.Start();
        PingText = GameObject.Find("PingText").GetComponent<Text>();
    }
    protected override void ProcessButtonPress(ButtonAction btn)
    {
        switch (btn)
        {
            case ButtonAction.returnToMain:
                uiController.instance.ShowPanel(uiController.instance.MainPanel);
                break;
        }
    }
    public void Okay()
    {
        ProcessButtonPress(ButtonAction.returnToMain);
    }
    public override void TransitionIn()
    {
        base.TransitionIn();
        PingText.text = string.Format("Your ping is {0}ms! Your playing experience may be poor. Consider connecting to a faster network.", socketController.instance.playerPing);
    }
}
