using System.Net.Sockets;
using System.Xml;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class leaderboardPanel : menuPanel {
    private Text LeaderboardNames;
    private Text LeaderboardStreaks;
    protected override void Start()
    {
        base.Start();
        LeaderboardNames = GameObject.Find("LeaderboardNames").GetComponent<Text>();
        LeaderboardStreaks = GameObject.Find("LeaderboardStreaks").GetComponent<Text>();

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
        string names = "";
        string streaks = "";
        foreach (var entries in socketController.instance.leaderboard.list)
        {
            var length = string.Format("{0}", entries["name"]).Length;
            names += string.Format("{0}",entries["name"]).Substring(1,length-2)+"\n";
            streaks += entries["streak"] + "\n";
        }
        LeaderboardNames.text = names;
        LeaderboardStreaks.text = streaks;
        base.TransitionIn();
    }
}

