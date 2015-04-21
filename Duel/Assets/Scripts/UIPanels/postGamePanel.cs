using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class postGamePanel : menuPanel
{	
	public int BasePointsWonLost;
	public int OppRating;
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
		OppRating = opponentRating;
	}
	public void ActuallyUpdateRating( int opponentRating)
	{
		int CPR = PlayerPrefs.GetInt("rating");
		int gain=0;
		double multiplier =1.0;
		int spread =1;
		int loss = 0;
		if (PlayerPrefs.GetInt("rating")<=0)
			PlayerPrefs.SetInt("rating", 100);

		if (CPR <= opponentRating) {

			if (gameController.instance.wonGame)
			{
				//case when you beat someone higher than you
				spread =(opponentRating - CPR); // larger spread = more rating gained
				multiplier = spread/CPR; // relatively how big is that spread
				gain =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR + gain));
				OutcomeText.text += "\nCurrent Streak: " + PlayerPrefs.GetInt("winStreak").ToString() ;
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n+2 Gold ";
				OutcomeText.text += "\n+ " + gain.ToString() + " Rating";
			}
			else
			{
				//case when you lose to someone higher than you
				spread =(opponentRating - CPR);
				multiplier = spread/CPR;
				loss =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR - loss));

				OutcomeText.text += "\nCurrent Streak: " + PlayerPrefs.GetInt("winStreak").ToString() ;
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n+1 Gold ";
				OutcomeText.text += "\n- " + gain.ToString() + " Rating";
			}
		} 
		else 
		{
			if (gameController.instance.wonGame)
			{
				//case when you beat someone lower than you
				spread =(CPR - opponentRating);
				multiplier = spread/CPR;

				gain =(BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR + gain));
				OutcomeText.text += "\nCurrent Streak: " + PlayerPrefs.GetInt("winStreak").ToString() ;
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n+2 Gold ";
				OutcomeText.text += "\n+ " + gain.ToString() + " Rating";


			}

			else{
				//case when you lose to someone lower than you
				if (opponentRating != 0)//who isnt a bot
				{
				spread =(CPR - opponentRating);
				multiplier = spread/CPR;
				loss =(int)((multiplier*spread)+ BasePointsWonLost);
				PlayerPrefs.SetInt("rating", (CPR - loss));
				}
				else
				{
					loss =(int)(BasePointsWonLost);
					PlayerPrefs.SetInt("rating", (CPR - loss));
				
				}
				OutcomeText.text += "\nCurrent Streak: " + PlayerPrefs.GetInt("winStreak").ToString() ;
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n ";
				OutcomeText.text += "\n+1 Gold ";
				OutcomeText.text += "\n- " + loss.ToString() + " Rating";
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
			ActuallyUpdateRating(OppRating);

        }
        else
        {
            OutcomeTitle.text = "You Lost!";
            OutcomeText.text = "You're the worst! :(";
			PlayerPrefs.SetInt("losses",(PlayerPrefs.GetInt("losses") + 1));
			PlayerPrefs.SetInt("gold",(PlayerPrefs.GetInt("gold") + 1));
			PlayerPrefs.SetInt("winStreak", 0);
			ActuallyUpdateRating(OppRating);
        }
        base.TransitionIn();
    }
}