using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class failPanel : menuPanel {
    private Text errorText;
    protected override void Start()
    {
        base.Start();
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
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
        errorText.text = socketController.instance.errorMessage;
    }
}
