using System;
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
    #region variables & initialiation
    private SocketIOComponent socket;
    public string challengerCode { get; private set; }
    public string challengedCode { get; private set;}
    public string playerCode{ get; private set;}
    public string errorMessage { get; private set;}
    public bool isChallenger { get; private set; }
    public bool inMatchmaking { get; private set; }
    public string distractionMessage { get; private set; }
    void Start()
    {
        //set socket reference
        socket = FindObjectOfType<SocketIOComponent>();
        //Register UI and other event listeners
        socket.On("playerCodeCreated", receiveCode);
        socket.On("connectionError", receiveError);
        socket.On("playerDisconnected", playerDisconnect);
        socket.On("challengePosted", challengeRecieved);
        socket.On("challengeCanceled", challengeCancelled);
        socket.On("challengeAccepted", challengeAccepted);
        socket.On("challengeRejected", challengeRejected);
        socket.On("disconnectFromRoom", disconnectFromRoom);
        socket.On("beginGame", beginGame);
        socket.On("draw", draw);
        socket.On("distraction", showDistraction);
        socket.On("endDraw", endDraw);
        socket.On("gameUpdate", gameUpdate);
        StartCoroutine(requestCode());
    }
    private IEnumerator requestCode()
    {
        yield return new WaitForSeconds(1);
        //request player code
        socket.Emit("requestPlayerCode");
        yield return new WaitForSeconds(5);
        if (playerCode == null)
            Application.Quit();
    }
    #endregion
    #region socket listeners
    //recieve events from server and display approrpriate UI elements through UI controller
    private void receiveCode(SocketIOEvent e)
    {
        playerCode = string.Format("{0}", e.data["code"]).Substring(1, 4);
        uiController.instance.ShowPanel(uiController.instance.OnFirstLoadPanel);
    }
    private void receiveError(SocketIOEvent e)
    {
        errorMessage = string.Format("{0}",e.data["message"]).Substring(1, string.Format("{0}",e.data["message"]).Length-2);
        uiController.instance.ShowPanel(uiController.instance.FailPanel);
    }
    private void playerDisconnect(SocketIOEvent e)
    {
        Debug.Log("Player disconnected!");
        uiController.instance.ShowPanel(uiController.instance.FailPanel);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = string.Format("{0}", e.data["channel"]).Substring(1, 4);
        socket.Emit("playerDisconnected", new JSONObject(data));
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
        Debug.Log("show stuff");
        uiController.instance.ShowPanel(uiController.instance.PreGamePanel);
    }
    private void challengeRecieved(SocketIOEvent e)
    {
        challengerCode = string.Format("{0}", e.data["id"]).Substring(1, 4);
        uiController.instance.ShowPanel(uiController.instance.ChallengedPanel);
    }
    private void challengeCancelled(SocketIOEvent e)
    {
        uiController.instance.ShowPanel(uiController.instance.MainPanel);
    }
    private void challengeRejected(SocketIOEvent e)
    {
        errorMessage = string.Format("{0}", e.data["message"]).Substring(1);
        uiController.instance.ShowPanel(uiController.instance.FailPanel);
        gameController.instance.resetGameState();
    }
    private void beginGame(SocketIOEvent e)
    {
        isChallenger = (string.Format("{0}",e.data["player1"]).Substring(1,4)==playerCode);
        uiController.instance.ShowPanel(uiController.instance.PreGamePanel);
        uiController.instance.PreGamePanel.CountDown();
        gameController.instance.beginGame();
        challengedCode = null;
    }
    private void draw(SocketIOEvent e)
    {
        distractionMessage = null;
        uiController.instance.ShowPanel(uiController.instance.DrawPanel);
    }

    private void showDistraction(SocketIOEvent e)
    {
        float proc = float.Parse(string.Format("{0}", e.data["value"]).Substring(1,3));
        if(proc <.1f)
            distractionMessage = "Dude!";
        if(proc>=.1f && proc <.2f)
            distractionMessage = "D'oh!";
        if (proc >= .2f && proc < .3f)
            distractionMessage = "Dang!";
        if (proc >= .4f && proc < .5f)
            distractionMessage = "Darn!";
        if (proc >= .5f && proc < .6f)
            distractionMessage = "Damn!";
        if (proc >= .6f && proc < .7f)
            distractionMessage = "Derp!";
        if (proc >= .7f)
            distractionMessage = "Drat!";
        uiController.instance.ShowPanel(uiController.instance.DrawPanel);
    }
    private void endDraw(SocketIOEvent e)
    {
        uiController.instance.HidePanel();
    }
    private void gameUpdate(SocketIOEvent e)
    {
        Debug.Log(e.data);
        int playerState;
        int opponentState;
        bool wonGame = false;
        if (isChallenger)
        {
            playerState = int.Parse(string.Format("{0}", e.data["player1state"]));
            opponentState = int.Parse(string.Format("{0}", e.data["player2state"]));
        }
        else
        {
            playerState = int.Parse(string.Format("{0}", e.data["player2state"]));
            opponentState = int.Parse(string.Format("{0}", e.data["player1state"]));
        }
        int gameState = int.Parse(string.Format("{0}", e.data["gameState"]));
        if (playerState == 3 && gameState == 2)
        {
            wonGame = true;
        }
        gameController.instance.recieveGameState(gameState, playerState, opponentState, wonGame);
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
        uiController.instance.ShowPanel(uiController.instance.PreGamePanel);
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
        uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);
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
    }
    public void resetGame()
    {
        if (!inMatchmaking)
        {
            socket.Emit("reset");
            gameController.instance.resetGameState();
        }
        uiController.instance.ShowPanel(uiController.instance.MainPanel);

    }

    public void findMatch()
    {
        inMatchmaking = true;
        socket.Emit("findMatch");
        uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);
    }
    #endregion
}


