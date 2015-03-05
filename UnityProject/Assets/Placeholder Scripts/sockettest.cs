using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;

public class sockettest : MonoBehaviour {
    private SocketIOComponent socket;
    private string userId;
    public Text codeField;
    public GameObject mainPanel, challengePanel, challengedPanel, rejectedPanel, gamePanel, invalidCodePanel;
	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("gameCodeCreated", recieveCode);
        socket.On("challengedIsBusy", rejectChallenge);
        StartCoroutine(GetCode());
	}
    private void recieveCode(SocketIOEvent e){
        Debug.Log("recieved code!");
        Debug.Log(e.data);
        codeField.text = string.Format("{0}", e.data["gameCode"]);
        userId = string.Format("{0}", e.data["userId"]);
        mainPanel.SetActive(true);
    }
    private void rejectChallenge(SocketIOEvent e)
    {
        Debug.Log("Challenge Rejected!");
        mainPanel.SetActive(false);
        rejectedPanel.SetActive(true);
    }
    IEnumerator GetCode()
    {
        yield return new WaitForSeconds(2);
        socket.Emit("requestCode");
    }
    public void challenge(string s)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["code"] = s;
        data["challengerId"] = userId;
        socket.Emit("challenge",new JSONObject(data));
        Debug.Log("sent Challenge!");
    }
}
