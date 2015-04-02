using UnityEngine;
using System.Collections;
using NUnit.Framework; 

public class animTest{	
	[Test]
	public void testing(){
		GameObject duelFigure = GameObject.Find("DuelFigure");
		Animation duelAnim;
		bool clicked = false;
	    duelAnim = duelFigure.GetComponent<Animation> ();
		duelFigure = GameObject.Find("DuelFigure");
	    duelFigure.GetComponent<Animation> ().Play ("Idle");

		Assert.That (duelAnim.isPlaying == true);

		duelFigure.GetComponent<Animation> ().Play ("Shoot");

		Assert.That (duelAnim.isPlaying == true);

		duelFigure.GetComponent<Animation> ().Play ("Jam");

		Assert.That (duelAnim.isPlaying == true);

		duelFigure.GetComponent<Animation> ().Play ("Dead");

		Assert.That (duelAnim.isPlaying == true);

	}

}
