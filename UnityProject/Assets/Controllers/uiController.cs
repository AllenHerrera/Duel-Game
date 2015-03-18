using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uiController : MonoBehaviour
{
    public GameObject mainPanel, challengePanel, challengedPanel, rejectedPanel, preGamePanel, invalidCodePanel;
    public Text code;
    private static readonly uiController instance = new uiController();
    //allows one instance of object. Provides static reference to it.
    private uiController() { }

    public static uiController Instance
    {
        get
        {
            return instance;
        }
    }
    // Use this for initialization
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
    }

    public void showAcceptedPanel()
    {
        //shows panel when challenge is accepted
        preGamePanel.SetActive(true);
        mainPanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
    }

    public void showCode(string userCode)
    {
        code.text = userCode;
        //shows panel with personal code
        mainPanel.SetActive(true);
        preGamePanel.SetActive(false);
        challengePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
    }

    public void challenge()
    {
        //displays panel that enables challenging of other players
        challengePanel.SetActive(true);
        mainPanel.SetActive(false);
        preGamePanel.SetActive(false);
        challengedPanel.SetActive(false);
        rejectedPanel.SetActive(false);
    }
    public void challengeResponse()
    {
        //displays panel when challenged
        challengedPanel.SetActive(true);
        challengePanel.SetActive(false);
        mainPanel.SetActive(false);
        preGamePanel.SetActive(false);
        rejectedPanel.SetActive(false);

    }
}
