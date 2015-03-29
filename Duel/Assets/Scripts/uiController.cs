using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uiController : MonoBehaviour
{

    #region singleton
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
    #endregion
    #region variables & initialization
    public mainPanel MainPanel { get; private set; }
    public challengingPanel ChallengingPanel { get; private set; }
    public failPanel FailPanel { get; private set; }
    public challengedPanel ChallengedPanel { get; private set; }
    public preGamePanel PreGamePanel { get; private set; }
    public postGamePanel PostGamePanel { get; private set; }
    public drawPanel DrawPanel { get; private set; }
    private menuPanel currentPanel;
    private void Start()
    {
        MainPanel = FindObjectOfType<mainPanel>();
        ChallengingPanel = FindObjectOfType<challengingPanel>();
        FailPanel = FindObjectOfType<failPanel>();
        ChallengedPanel = FindObjectOfType<challengedPanel>();
        PreGamePanel = FindObjectOfType<preGamePanel>();
        PostGamePanel = FindObjectOfType<postGamePanel>();
        DrawPanel = FindObjectOfType<drawPanel>();
        //set pregame, ingameUi & draw panels to inactive so it doesn't block other touch events
        PreGamePanel.gameObject.SetActive(false);
        DrawPanel.gameObject.SetActive(false);
    }
    #endregion
    #region public methods
    public void ShowPanel(menuPanel panel)
    {
        Debug.Log("changing panel");
        if (currentPanel != null)
        {
            currentPanel.TransitionOut();
        }
        currentPanel = panel;
        //check if desired panel is pre game panel. Reenable if it is
        if (panel == PreGamePanel)
        {
            panel.gameObject.SetActive(true);
        }
        panel.TransitionIn();
    }
    public void HidePanel()
    {
        Debug.Log("Hiding panel");
        currentPanel.TransitionOut();
        currentPanel = null;       
    }
    #endregion
}