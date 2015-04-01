using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour {
	GameObject duelFigure;
	Animation duelAnim;
	bool clicked = false;


	// Use this for initialization
	void Start () {
		duelAnim = duelFigure.GetComponent<Animation> ();
		this.gameObject.GetComponent<Animation> ().Play ("Idle");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("s")){

			this.gameObject.GetComponent<Animation>().Play("Shoot");

		}
		if(Input.GetKey("d")){
			this.gameObject.GetComponent<Animation>().Play("Dead");
			
		}
		if (Input.GetKeyDown ("j")) {
			this.gameObject.GetComponent<Animation> ().Play ("Jam");
			
		} 

		if (Input.GetKeyDown ("i")) {
			this.gameObject.GetComponent<Animation> ().Play ("Idle");
			
		}
	
	}
}
