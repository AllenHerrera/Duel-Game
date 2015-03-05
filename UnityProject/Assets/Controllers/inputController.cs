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

	//private boolean isTouching= False
	// in update loop, if not Touching, and there is a touchcount, then run the method that processes input
	// if isTouching and TouchCount=0, set touchCount to false 

	// Update is called once per frame
	void Update () {
	
	}


}
