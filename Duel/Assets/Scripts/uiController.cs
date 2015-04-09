using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
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
	public onFirstLoadPanel OnFirstLoadPanel { get; private set; }
	public OptionsPanel OptionsPanel { get; private set; }
	public ChangeNamePanel ChangeNamePanel { get; private set; }
    public aiPanel AIPanel { get; private set; }
	public Text Title { get; private set; }

    private menuPanel currentPanel;
    private void Start()
    {
        Title = GameObject.Find("Title").GetComponent<Text>();
        MainPanel = FindObjectOfType<mainPanel>();
        ChallengingPanel = FindObjectOfType<challengingPanel>();
        FailPanel = FindObjectOfType<failPanel>();
        ChallengedPanel = FindObjectOfType<challengedPanel>();
        PreGamePanel = FindObjectOfType<preGamePanel>();
        PostGamePanel = FindObjectOfType<postGamePanel>();
        DrawPanel = FindObjectOfType<drawPanel>();
		OnFirstLoadPanel = FindObjectOfType<onFirstLoadPanel>();
		OptionsPanel = FindObjectOfType<OptionsPanel> ();
		ChangeNamePanel = FindObjectOfType<ChangeNamePanel> ();
        AIPanel = FindObjectOfType<aiPanel>();
        //set pregame, ingameUi & draw panels to inactive so it doesn't block other touch events
        PreGamePanel.gameObject.SetActive(false);
        DrawPanel.gameObject.SetActive(false);
        //Tween in Title
    }
    #endregion
    #region public methods
    public void ShowPanel(menuPanel panel)
    {
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
        if (panel == MainPanel)
        {
            gameController.instance.resetGameState();
        }
        panel.TransitionIn();
    }
    public void HidePanel()
    {
        currentPanel.TransitionOut();
        currentPanel = null;       
    }

    public void ShowTitle()
    {
        Title.DOText("Duel", .5f);
    }

    public void HideTitle()
    {
        Title.DOText("", .5f);
    }
    #endregion
}