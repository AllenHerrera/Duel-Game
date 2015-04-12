using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour {
	GameObject duelFigure;
	Animation duelAnim;
	bool clicked = false;
	int hatSetting;
	private SpriteRenderer torso, legs, hat;
	public Sprite[] torsoOptions = new Sprite[4];
	public Sprite[] legsOptions = new Sprite[4];
	public Sprite[] hatsOptions = new Sprite[4];

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


private void Start(){
		torso = GameObject.FindGameObjectWithTag ("shirt").GetComponent<SpriteRenderer>();
		hat = GameObject.FindGameObjectWithTag ("hats").GetComponent<SpriteRenderer>();
		legs = GameObject.FindGameObjectWithTag ("legs").GetComponent<SpriteRenderer>();
	}
public void setHat(int option){
		hat.sprite = hatsOptions [option];
	}
public void setShirt(int option){
		torso.sprite = torsoOptions [option];
	}
public void setLegs(int option){
		legs.sprite = legsOptions [option];
	}
}