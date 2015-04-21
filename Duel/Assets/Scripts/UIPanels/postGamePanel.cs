using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class postGamePanel : menuPanel
{	
	public int BasePointsWonLost;
    private Text OutcomeTitle, OutcomeText;
    protected override void Start()
    {
		BasePointsWonLost = 1;
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
	public void UpdateRating( int opponentRating)
	{
		int CPR = PlayerPrefs.GetInt("rating");
		int gain;
		double multiplier;
		int spread;
		int loss;

		if (CPR <= opponentRating) {

			if (gameController.instance.wonGame)
			{
				//case when you beat someone higher than you
				spread =(opponentRating - CPR); // larger spread = more rating gained
				multiplier = spread/CPR; // relatively how big is that spread
				gain =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR + gain));
			}
			else
			{
				//case when you lose to someone higher than you
				spread =(opponentRating - CPR);
				multiplier = spread/CPR;
				loss =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR - loss));

				if (PlayerPrefs.GetInt("rating")==0)
					PlayerPrefs.SetInt("rating", 100);
			}
		} 
		else 
		{
			if (gameController.instance.wonGame)
			{
				//case when you beat someone lower than you
				spread =(CPR - opponentRating);
				multiplier = spread/CPR;
				if (multiplier < .33)
				gain =(BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR + gain));

			}

		else{
				//case when you lose to someone lower than you
				spread =(CPR - opponentRating);
				multiplier = spread/CPR;
				loss =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR - loss));
			}
		}


	}
    public override void TransitionIn()
    {
        if (gameController.instance.wonGame)
        {
            OutcomeTitle.text = "You Won!";
            OutcomeText.text = "You're the best! :)";
			PlayerPrefs.SetInt("wins",(PlayerPrefs.GetInt("wins") + 1));
			PlayerPrefs.SetInt("gold",(PlayerPrefs.GetInt("gold") + 2));
			PlayerPrefs.SetInt("winStreak", PlayerPrefs.GetInt("winStreak")+1);
			if (PlayerPrefs.GetInt("winStreak") > PlayerPrefs.GetInt("maxWinStreak"))
			{ 
				PlayerPrefs.SetInt("maxWinStreak", PlayerPrefs.GetInt("winStreak"));
			}

        }
        else
        {
            OutcomeTitle.text = "You Lost!";
            OutcomeText.text = "You're the worst! :(";
			PlayerPrefs.SetInt("losses",(PlayerPrefs.GetInt("losses") + 1));
			PlayerPrefs.SetInt("gold",(PlayerPrefs.GetInt("gold") + 1));
			PlayerPrefs.SetInt("winStreak", 0);
        }
        base.TransitionIn();
    }
}