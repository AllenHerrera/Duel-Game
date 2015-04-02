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
	[Test]
	public void challengedPanelTest(){
		uiController.instance.ShowPanel (uiController.instance.ChallengedPanel);
		Assert.That (uiController.instance.ChallengedPanel.enabled == true);
		
	}
	[Test]
	public void drawPanelTest(){
		uiController.instance.ShowPanel (uiController.instance.DrawPanel);
		Assert.That (uiController.instance.DrawPanel.enabled == true);
		
	}
	[Test]
	public void preGamePanelTest(){
		uiController.instance.ShowPanel (uiController.instance.PreGamePanel);
		Assert.That (uiController.instance.PreGamePanel.enabled == true);
		
	}
	[Test]
	public void failPanelTest(){
		uiController.instance.ShowPanel (uiController.instance.FailPanel);
		Assert.That (uiController.instance.FailPanel.enabled == true);
		
	}
	[Test]
	public void optionsPanelTest(){
		uiController.instance.ShowPanel (uiController.instance.OptionsPanel);
		Assert.That (uiController.instance.OptionsPanel.enabled == true);
		
	}
	[Test]
	public void firstLoadPanelTest(){
		uiController.instance.ShowPanel (uiController.instance.OnFirstLoadPanel);
		Assert.That (uiController.instance.OnFirstLoadPanel.enabled == true);
		
	}
	[Test]
	public void changeNamePanelTest(){
		uiController.instance.ShowPanel (uiController.instance.ChangeNamePanel);
		Assert.That (uiController.instance.ChangeNamePanel.enabled == true);
		
	}
}	

