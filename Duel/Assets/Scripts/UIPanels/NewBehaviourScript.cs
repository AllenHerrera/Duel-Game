using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	
	// Use this for initialization
	
	void Start () {
		string playername;
		
		//get playerprefs - if null, set to zero
		int savedsprite = PlayerPrefs.GetInt("savedsprite");
		if (savedsprite == null) savedsprite = 0;
		int savedmusic = PlayerPrefs.GetInt("savedmusic");
		if (savedmusic == null) savedmusic = 0;
		int savedsound = PlayerPrefs.GetInt("savedsound");
		if (savedsound == null) savedsound = 0;
		
		int firstload = PlayerPrefs.GetInt ("firstload");
		//if first load is anything but null
		if (firstload == null) {
			PlayerPrefs.SetInt ("firstLoad", 1);
			
			//still need text field GUI
			//link this to GUI - idk how
			//put this in void onGUI()
			if (GUI.Button (new Rect (10, 70, 50, 30), "Save Profile")) {
				Debug.Log ("USER SAVED PROFILE");
			//	playername = textFieldVariableforplayerName;
			//	PlayerPrefs.SetString ("playerProfile", textFieldVariableforplayerName);
			}
		} 
		else {
			playername = PlayerPrefs.GetString ("playerProfile");
			PlayerPrefs.SetInt ("firstload", firstload + 1);
			
		}
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
