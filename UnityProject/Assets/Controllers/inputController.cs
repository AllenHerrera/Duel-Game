using UnityEngine;
using System.Collections;

public class inputController : MonoBehaviour {
	#region Singleton
	private static readonly inputController instance = new inputController();
	
	private inputController(){}
	
	public static inputController Instance
	{
		get 
		{
			return instance; 
		}
	}
	#endregion

	private bool isTouching= false; 

	public void DisableTouchInput(){
		this.gameObject.SetActive (false);
	}

	public void EnableTouchInput(){
		this.gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update() {
		if (isTouching==false && Input.touchCount > 0 ){
			isTouching=true; 
			//run method that processes input
		}

		if (isTouching==true && Input.touchCount==0 ) {
			isTouching= false; 

	
	}


}
}
