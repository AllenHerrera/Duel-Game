using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uiController : MonoBehaviour
{
    public GameObject mainPanel, challengePanel, challengedPanel, rejectedPanel, preGamePanel, invalidCodePanel, disconnectedPanel;
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
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

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
}
