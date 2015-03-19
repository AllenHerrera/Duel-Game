using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class uiTester : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[TestFixture]
public class uiControllerTest{
	public GameObject mainPanel, challengePanel, challengedPanel, rejectedPanel, preGamePanel, invalidCodePanel, disconnectedPanel;
	public Text code;
	
	[Test]
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
	[Test]
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
	[Test]
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
	[Test]
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
	[Test]
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
	[Test]
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
	[Test]
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
	[Test]
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