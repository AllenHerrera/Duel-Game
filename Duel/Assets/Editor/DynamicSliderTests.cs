using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class DynamicSliderInitalizationTest {


	/*[Test]
	public void FirstLoadTest()
	{

		Assert.That (GameObject.FindWithTag ("test1").GetComponent<OptionsPanel> ().music == null);
	}
	*/

	[Test]
	public void CharacterSliderLength()
	{
		Sprite[] characterSprites;
		characterSprites = Resources.LoadAll<Sprite> ("Sprites");

		GameObject.Find ("OptionsPanel").GetComponent<OptionsPanel> ().setDynamicSliders ();

		//Debug.Log (characterSprites.Length-1);
		//Debug.Log (GameObject.FindWithTag ("CharacterSlider").GetComponent<Slider>().maxValue);
		Assert.That (GameObject.FindWithTag ("CharacterSlider").GetComponent<Slider>().maxValue == characterSprites.Length-1);

	}

	[Test]
	public void MusicSliderLength()
	{
		AudioClip[] audioClips;
		audioClips = Resources.LoadAll<AudioClip> ("Audio");
		
		GameObject.Find ("OptionsPanel").GetComponent<OptionsPanel> ().setDynamicSliders ();
		
		//Debug.Log (audioClips.Length-1);
		//Debug.Log (GameObject.Find ("MusicSlider").GetComponent<Slider>().maxValue);
		Assert.That (GameObject.Find ("MusicSlider").GetComponent<Slider>().maxValue == audioClips.Length-1);
		
	}


}
