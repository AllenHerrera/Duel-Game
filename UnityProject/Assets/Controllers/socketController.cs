using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class socketController : MonoBehaviour
{
    #region singleton
    private static readonly socketController instance = new socketController();
   private socketController() { }
   public static socketController Instance
   {
      get 
      {
         return instance; 
      }
   }
   #endregion
   #region variables
   private SocketIOComponent socket;
   private string playerCode;
   private string challengerCode;
   #endregion
   void Start () {
        //set socket reference
        socket = GetComponent<SocketIOComponent>();
        //Register UI and other event listeners
        socket.On("playerCodeCreated", recieveCode);
        socket.On("invalidCode", invalidCode);
        socket.On("challengedIsBusy", challengeRejected);
        socket.On("challengePosted", challengeRecieved);
        socket.On("challengeAccepted", challengeAccepted);
        socket.On("challengeRejected", challengeRejected);
        //request player code
        socket.Emit("requestPlayerCode");
	}
    #region socket listeners
    //recieve events from server and display approrpriate UI elements through UI controller
    private void recieveCode(SocketIOEvent e)
    {
        Debug.Log(e.data);
        //codeField.text = string.Format("{0}", e.data["gameCode"]);
        playerCode= string.Format("{0}", e.data["userId"]).Substring(1,4);
        //Ask UI Controller to display code
        uiController.Instance.showCode(playerCode);
    }
    private void invalidCode(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen

    }

    private void challengeAccepted(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
       
    }
    private void challengeRecieved(SocketIOEvent e)
    {
        challengerCode = string.Format("{0}",e.data["challengerId"]);
        //Ask UI Controller to display appropriate screen        
    }

    private void challengeRejected(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        
    }
    #endregion
    #region public methods
    //recieve inputs from UI/Gamecontroller and send them to server
    public void acceptChallenge()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["challengerId"] = challengerCode;
        socket.Emit("acceptChallenge", new JSONObject(data));  
    }
    public void rejectChallenge()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["challengerId"] = challengerCode;
        socket.Emit("rejectChallenge", new JSONObject(data));  
    }
    public void challenge(string s)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["code"] = s;
        data["challengerId"] = playerCode;
        socket.Emit("challenge", new JSONObject(data));      
    }
#endregion
}


