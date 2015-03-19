using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class socketController : MonoBehaviour
{
    #region singleton
    private static socketController _instance;
    //This is the public reference that other classes will use
    public static socketController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<socketController>();
            return _instance;
        }
    }
    #endregion
    #region variables
    private SocketIOComponent socket;
    private string playerCode;
    private string challengerCode;
    private string challengedCode;
    #endregion
    void Start()
    {
        //set socket reference
        socket = FindObjectOfType<SocketIOComponent>();
        //Register UI and other event listeners
        socket.On("playerCodeCreated", recieveCode);
        socket.On("invalidCode", invalidCode);
        socket.On("playerDisconnected", playerDisconnect);
        socket.On("challengedIsBusy", challengeRejected);
        socket.On("challengePosted", challengeRecieved);
        socket.On("challengeCanceled", challengeCancelled);
        socket.On("challengeAccepted", challengeAccepted);
        socket.On("challengeRejected", challengeRejected);
        socket.On("disconnectFromRoom", disconnectFromRoom);
        socket.On("beginGame", beginGame);
        socket.On("draw", draw);
        socket.On("endDraw", endDraw);
        socket.On("gameUpdate", gameUpdate);
        StartCoroutine(requestCode());
    }
    private IEnumerator requestCode()
    {
        yield return new WaitForSeconds(1);
        //request player code
        socket.Emit("requestPlayerCode");
    }
    #region socket listeners
    //recieve events from server and display approrpriate UI elements through UI controller
    private void recieveCode(SocketIOEvent e)
    {
        Debug.Log(e.data);
        //codeField.text = string.Format("{0}", e.data["gameCode"]);
        playerCode = string.Format("{0}", e.data["code"]).Substring(1, 4);
        //Ask UI Controller to display code
        uiController.instance.showCode(playerCode);
    }
    private void invalidCode(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.showInvalidCodePanel();

    }
    private void playerDisconnect(SocketIOEvent e)
    {
        Debug.Log(e.data);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = string.Format("{0}", e.data["channel"]).Substring(1, 4);
        socket.Emit("playerDisconnected", new JSONObject(data));
        //Ask UI Controller to display appropriate screen
        uiController.instance.hidePlayerLabel();
        uiController.instance.showDisconnectedPanel();
        gameController.instance.resetGameState();
    }
    private void disconnectFromRoom(SocketIOEvent e)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = string.Format("{0}", e.data["channel"]).Substring(1, 4);
        socket.Emit("playerDisconnected", new JSONObject(data));
    }

    private void challengeAccepted(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.showAcceptedPanel();

    }
    private void challengeRecieved(SocketIOEvent e)
    {
        challengerCode = string.Format("{0}", e.data["id"]).Substring(1, 4);
        Debug.Log("recieved challenge from " + challengerCode);
        //Ask UI Controller to display appropriate screen        
        uiController.instance.showChallengedPanel();
    }
    private void challengeCancelled(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.showCode(playerCode);
    }

    private void challengeRejected(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.showRejectedPanel();
        gameController.instance.resetGameState();
    }
    private void beginGame(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        if (challengedCode != null)
            gameController.instance.beginGame(true);
        else
            gameController.instance.beginGame(false);
        uiController.instance.hideMenuPanels();
        challengedCode = null;
    }
    private void draw(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.showDraw();
    }
    private void endDraw(SocketIOEvent e)
    {
        //Ask UI Controller to display appropriate screen
        uiController.instance.hideDraw();
    }
    private void gameUpdate(SocketIOEvent e)
    {
        Debug.Log("recieved game update");
        Debug.Log(e.data);
        int player1State = int.Parse(string.Format("{0}", e.data["player1state"]));
        int player2State = int.Parse(string.Format("{0}", e.data["player2state"]));
        int gameState = int.Parse(string.Format("{0}", e.data["gameState"]));
        gameController.instance.recieveGameState(gameState, player1State, player2State);
    }
    #endregion
    #region public methods
    //recieve inputs from UI/Gamecontroller and send them to server
    public void processInput()
    {
        socket.Emit("processInput");
    }
    public void cancelChallenge()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["code"] = challengedCode;
        socket.Emit("cancelChallenge", new JSONObject(data));
        challengedCode = null;
    }
    public void acceptChallenge()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["challengerId"] = challengerCode;
        Debug.Log(new JSONObject(data));
        socket.Emit("acceptChallenge", new JSONObject(data));
        challengedCode = null;
    }
    public void rejectChallenge()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["challengerId"] = challengerCode;
        Debug.Log(new JSONObject(data));
        socket.Emit("rejectChallenge", new JSONObject(data));
        challengedCode = null;
        gameController.instance.resetGameState();
    }
    public void challenge(string s)
    {
        challengedCode = s;
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["code"] = s;
        data["challengerId"] = playerCode;
        Debug.Log(new JSONObject(data));
        socket.Emit("challenge", new JSONObject(data));
        uiController.instance.showChallengingPanel();
        challengerCode = challengedCode;
    }
    public void challenge()
    {
        Debug.Log(challengerCode);
        Dictionary<string, string> data = new Dictionary<string, string>();
        if (challengedCode == null)
            challengedCode = challengerCode;
        data["code"] = challengedCode;
        data["challengerId"] = playerCode;
        Debug.Log(new JSONObject(data));
        socket.Emit("challenge", new JSONObject(data));
        uiController.instance.showChallengingPanel();
    }
    public void resetGame()
    {
        socket.Emit("reset");
        gameController.instance.resetGameState();
        uiController.instance.showCode(playerCode);
    }
    #endregion

}


