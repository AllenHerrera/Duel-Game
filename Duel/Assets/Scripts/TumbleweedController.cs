﻿using UnityEngine;
using System.Collections;

public class TumbleweedController : MonoBehaviour {
	
	#region Singleton
	private static TumbleweedController _instance;
	//This is the public reference that other classes will use
	public static TumbleweedController instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<TumbleweedController>();
			return _instance;
		}
	}
	#endregion
	public GameObject tumbleweed; 
	private GameObject tw;
	float count= 0; 

	// Use this for initialization
	void Start () {

		tw = Instantiate(tumbleweed, new Vector3(15, -2.5f, 0), Quaternion.identity) as GameObject;
        var rigid = tw.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(Random.Range(-3,-5), 1);

	}
	void FixedUpdate(){
		var rigid = tw.GetComponent<Rigidbody2D> ();
		count = count + 1; 
		if (tw.transform.position.x < -11) {
			tw.transform.position = new Vector2 (15, -2.5f);
            rigid.velocity = new Vector2(-5, 1);
		}

		if (count>40 &&count <80) {
			rigid.AddForce (new Vector2 (0, 7));
		}

		if (count>120 &&count <160) {
			rigid.AddForce (new Vector2 (0, 7));

		}

		if (count==180) {
			count=0; 
		
		}
    }

}