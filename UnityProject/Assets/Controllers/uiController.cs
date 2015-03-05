using UnityEngine;
using System.Collections;

using UnityEngine; 
using System.Collections; 
public class uiController : MonoBehaviour {       
	
	private static readonly uiController instance = new uiController();
	//allows one instance of object. Provides static reference to it.
	private uiController() { }

	public static uiController Instance   {
		get{
			 return instance;
		}
	} 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void panelUpdate(){
	//takes the different events such as rejecting or accepting panels and sets the approriate panel to active.

	}

	public void showRejectedPanel(){
		//shows panel when a challenge is rejected

	}

	public void showAcceptedPanel(){
		//shows panel when challenge is accepted
	
	}

	public void showCode(){
		//shows panel with personal code
	}

	public void challenge(){
		//displays panel that enables challenging of other players
	}

}

