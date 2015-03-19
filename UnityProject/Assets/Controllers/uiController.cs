using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uiController : MonoBehaviour
{
    public GameObject mainPanel, challengePanel, challengedPanel, rejectedPanel, preGamePanel, invalidCodePanel, disconnectedPanel, Draw, player1Label, player2Label, wonPanel, lostPanel;
    public Text code;
    private static uiController _instance;
    //This is the public reference that other classes will use
    public static uiController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<uiController>();
            return _instance;
        }
    }
    public void panelUpdate()
    {
        //takes the different events such as rejecting or accepting panels and sets the approriate panel to active.
    }
    public void showRejectedPanel()
    {
        //shows panel when a challenge is rejected
        rejectedPanel.SetActive(true);
        preGamePanel.SetActive(false);
        mainPanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);        
    }
    public void showDisconnectedPanel()
    {
        //shows panel when a challenge is rejected
        rejectedPanel.SetActive(false);
        preGamePanel.SetActive(false);
        mainPanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        wonPanel.SetActive(false);
        lostPanel.SetActive(false);
        disconnectedPanel.SetActive(true);
    }
    public void showAcceptedPanel()
    {
        //shows panel when challenge is accepted
        preGamePanel.SetActive(true);
        mainPanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);

    }
    public void showCode(string userCode)
    {
        code.text = userCode;
        //shows panel with personal code
        mainPanel.SetActive(true);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);
    }
    public void showInvalidCodePanel()
    {
        mainPanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
        preGamePanel.SetActive(false);
        invalidCodePanel.SetActive(true);
        disconnectedPanel.SetActive(false);

    }
    public void showChallengingPanel()
    {
        //displays panel that enables challenging of other players
        challengePanel.SetActive(true);
        mainPanel.SetActive(false);
        preGamePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);
        wonPanel.SetActive(false);
        lostPanel.SetActive(false);
    }
    public void showChallengedPanel()
    {
        //displays panel when challenged
        challengedPanel.SetActive(true);
        challengePanel.SetActive(false);
        mainPanel.SetActive(false);
        preGamePanel.SetActive(false);
        rejectedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);
        wonPanel.SetActive(false);
        lostPanel.SetActive(false);

    }
    public void hideMenuPanels()
    {
        challengedPanel.SetActive(false);
        challengePanel.SetActive(false);
        mainPanel.SetActive(false);
        preGamePanel.SetActive(false);
        rejectedPanel.SetActive(false);
        disconnectedPanel.SetActive(false);
    }
    public void showPlayer1Label()
    {
        player1Label.SetActive(true);
        player2Label.SetActive(false);
    }
    public void showPlayer2Label()
    {
        player1Label.SetActive(false);
        player2Label.SetActive(true);
    }
    public void hidePlayerLabel()
    {
        player1Label.SetActive(false);
        player2Label.SetActive(false);
    }
    public void showDraw()
    {
        Draw.SetActive(true);
    }
    public void hideDraw()
    {
        Draw.SetActive(false);
    }
    public void showWonPanel()
    {
        wonPanel.SetActive(true);
    }
    public void showLostPanel()
    {
        lostPanel.SetActive(true);
    }
}
