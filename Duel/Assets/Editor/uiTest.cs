using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class uiTest{
	[Test]
	public void postGamePanelTest(){
		uiController.instance.ShowPanel (uiController.instance.PostGamePanel);
		Assert.That (uiController.instance.PostGamePanel.enabled == true);
	}
	[Test]
	public void challengingGamePanelTest(){
		uiController.instance.ShowPanel (uiController.instance.PostGamePanel);
		Assert.That (uiController.instance.ChallengingPanel.enabled == true);
	}
	[Test]
	public void mainGamePanelTest(){
		uiController.instance.ShowPanel (uiController.instance.MainPanel);
		Assert.That (uiController.instance.MainPanel.enabled == true);
	
	}
}	
