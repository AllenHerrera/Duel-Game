using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {
	
	#region Singleton
	private static CloudController _instance;
	//This is the public reference that other classes will use
	public static CloudController instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<CloudController>();
			return _instance;
		}
	}
	#endregion

	public GameObject cloud; 
	public Transform cloud1, cloud2;
	bool waiting= false; 
	
	// Use this for initialization
	void Start () {
		GameObject object1= Instantiate(cloud, new Vector3(15, 4, 5), Quaternion.identity) as GameObject;
		cloud1 = object1.GetComponent<Transform>();
		StartCoroutine(Wait(4));
	}
	
	IEnumerator Wait(float waitTime)
		{
		yield return new WaitForSeconds(waitTime);  
		GameObject object2= Instantiate(cloud, new Vector3(15, 1, 5), Quaternion.identity) as GameObject;
		cloud2 = object2.GetComponent<Transform>(); 
		waiting = true; 
	}


	// Update is called once per frame
	void Update () {
		if (cloud1.position.x < -11) 
			cloud1.position= new Vector3(15,4,5);

		if (waiting) 
			if (cloud2.position.x < -11) 
				cloud2.position= new Vector3(15,1,5);

		cloud1.position += Vector3.left * (Time.deltaTime*3)/2;
		if (waiting)
			cloud2.position += Vector3.left*Time.deltaTime;


}



}

