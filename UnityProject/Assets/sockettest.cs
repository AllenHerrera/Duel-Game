using UnityEngine;
using System.Collections;
using SocketIO;

public class sockettest : MonoBehaviour {
    private SocketIOComponent socket;
	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("roomCodeCreated", recieveCode);
	}
    private void OnSocketOpen(SocketIOEvent e)
    {
        socket.Emit("request Code");
    }
    private void recieveCode(SocketIOEvent e){
        Debug.Log("recieved code!");
        Debug.Log(e.data);
    }
}
