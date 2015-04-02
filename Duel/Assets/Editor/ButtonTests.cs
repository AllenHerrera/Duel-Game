using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonTests {


	[Test]
	public void OptionsPanel()
	{
	
		Assert.That (GameObject.Find("ChangeNameBtn").GetComponent<Button>().isActiveAndEnabled);
		Assert.That (GameObject.Find("ReturnToMenuBtn").GetComponent<Button>().isActiveAndEnabled);

	}

	[Test]
	public void ChangeNamePanel()
	{
		Assert.That (GameObject.Find("SaveBtn").GetComponent<Button>().isActiveAndEnabled);
		Assert.That (GameObject.Find("CancelBtn").GetComponent<Button>().isActiveAndEnabled);
	}

	[Test]
	public void MainMenu()
	{
		Assert.That (GameObject.Find("ChallengeBtn").GetComponent<Button>().isActiveAndEnabled);
		Assert.That (GameObject.Find("MatchMakingBtn").GetComponent<Button>().isActiveAndEnabled);
		Assert.That (GameObject.Find("LeaderBoardBtn").GetComponent<Button>().isActiveAndEnabled);

		Assert.That (GameObject.Find("OptionsBtn").GetComponent<Button>().isActiveAndEnabled);
		Assert.That (GameObject.Find("QuitBtn").GetComponent<Button>().isActiveAndEnabled);
	}

	[Test]
	public void OnFirstLoad()
	{
		Assert.That (GameObject.Find("ContinueBtn").GetComponent<Button>().isActiveAndEnabled);
	}
}
