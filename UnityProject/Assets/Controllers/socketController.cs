using UnityEngine;
using System.Collections;
using SocketIO;

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
   private string userId;
   private string gameCode;
   private string challengeGameRoom;
   #endregion
   void Start () {
       //set socket reference
       socket = GetComponent<SocketIOComponent>();
        //Register UI and other event listeners
        socket.On("gameCodeCreated", recieveCode);
        socket.On("challengedIsBusy", challengeRejected);
        socket.On("invalidCode", invalidCode);
        socket.On("challengePosted", challengeRecieved);
        socket.On("challengeAccepted", challengeAccepted);
        socket.On("challengeRejected", challengeRejected);    
        StartCoroutine(GetCode());
	}
    IEnumerator GetCode()//request code from server
    {
        yield return new WaitForSeconds(1);
        socket.Emit("requestCode");
    }
    #region socket listeners
    //recieve events from server and display approrpriate UI elements through UI controller
    private void invalidCode(SocketIOEvent e)
    {

    }
    private void recieveCode(SocketIOEvent e)
    {
       
    }

    private void challengeAccepted(SocketIOEvent e)
    {
        
    }
    private void challengeRecieved(SocketIOEvent e)
    {
        
    }

    private void challengeRejected(SocketIOEvent e)
    {
        
    }
    #endregion
    #region public methods
    //recieve inputs from UI/Gamecontroller and send them to server
    public void acceptChallenge()
    {
        
    }
    public void rejectChallenge()
    {
        
    }
    public void challenge(string s)
    {
        
    }
#endregion
}


