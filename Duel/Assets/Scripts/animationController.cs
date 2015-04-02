using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour {
	GameObject duelFigure;
	Animation duelAnim;
	bool clicked = false;

	public enum chooseAnimations {Idle, Shoot, Dead, Jam};


	public void animationChoice(chooseAnimations anim){

		switch (anim) {
		case chooseAnimations.Idle:
			this.gameObject.GetComponent<Animation> ().Play ("Idle");
			break;
		case chooseAnimations.Jam:
			this.gameObject.GetComponent<Animation> ().Play ("Jam");
			break;
		case chooseAnimations.Shoot:
			this.gameObject.GetComponent<Animation> ().Play ("Shoot");
			break;
		case chooseAnimations.Dead:
			this.gameObject.GetComponent<Animation> ().Play ("Dead");
			break;
	
		}
	}


	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
	
		}

	}
