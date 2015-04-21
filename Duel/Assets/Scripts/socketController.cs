using System;
using System.Net.Sockets;
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
    public bool vsAi { get; private set; }
    public int playerPing { get; private set; }
    private int drawTime;
    private bool pingWarning = false;
    public JSONObject leaderboard { get; private set; }
    private JSONObject player1Appearance, player2Appearance;
    private int player1Rating, player2Rating;
    void Start()
    {
        //set socket reference
        socket = FindObjectOfType<SocketIOComponent>();
        socket.url="ws://52.0.195.188:3001/socket.io/?EIO=4&transport=websocket";
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
        socket.On("suggestAIMatch", suggestAIMatch);
        socket.On("ping", ping);
        socket.On("pingResult", pingResult);
        socket.On("sendLeaderboard", receiveLeaderboard);
        socket.On("sendAppearances", setAppearances);
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
        updateAppearance();
    }
    private void ping(SocketIOEvent e)
    {
        socket.Emit("clientPing");

       // StartCoroutine(pingtest());
    }
    private void setAppearances(SocketIOEvent e)
    {
        Debug.Log(e.data);
        string data = string.Format("{0}", e.data["player1"]);
        player1Appearance = new JSONObject(data);
        data = string.Format("{0}", e.data["player2"]);
        player2Appearance = new JSONObject(data);
        var s = string.Format("{0}", e.data["player1Rating"]);
        player1Rating = int.Parse(string.Format("{0}", s).Substring(1,s.Length-2));
        s = string.Format("{0}", e.data["player2Rating"]);
        player2Rating = int.Parse(string.Format("{0}", s).Substring(1, s.Length - 2));

    }

    private IEnumerator pingtest()//TEST PURPOSES
    {
        yield return new WaitForSeconds(1);
        socket.Emit("clientPing");
    }
    private void receiveLeaderboard(SocketIOEvent e)
    {
        string data = string.Format("{0}", e.data["leaderboard"]);
        JSONObject lb = new JSONObject(data);
        leaderboard = lb;
        uiController.instance.ShowPanel(uiController.instance.LeaderboardPanel);
    }
    private void pingResult(SocketIOEvent e)
    {
       playerPing= int.Parse(string.Format("{0}", e.data["ping"]));
       uiController.instance.Ping = ""+playerPing;
    }
    private void receiveError(SocketIOEvent e)
    {
        inputController.instance.DisableInput();
        errorMessage = string.Format("{0}",e.data["message"]).Substring(1, string.Format("{0}",e.data["message"]).Length-2);
        uiController.instance.ShowPanel(uiController.instance.FailPanel);
    }
    private void playerDisconnect(SocketIOEvent e)
    {
        gameController.instance.StopAllCoroutines();
        inputController.instance.DisableInput();
        errorMessage = "Your opponent has disconnected from the match.";
        uiController.instance.ShowPanel(uiController.instance.FailPanel);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = string.Format("{0}", e.data["channel"]).Substring(1, 4);
        socket.Emit("playerDisconnected", new JSONObject(data));
        gameController.instance.resetGameState();
        
    }
    private void disconnectFromRoom(SocketIOEvent e)
    {
        vsAi = false;
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = string.Format("{0}", e.data["channel"]).Substring(1, 4);
        socket.Emit("disconnectFromRoom", new JSONObject(data));
    }
    private void challengeAccepted(SocketIOEvent e)
    {
        Debug.Log("show stuff");
        uiController.instance.ShowPanel(uiController.instance.PreGamePanel);
    }
    private void suggestAIMatch(SocketIOEvent e)
    {
        uiController.instance.ShowPanel(uiController.instance.AIPanel);
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
        distractionMessage = null;
        isChallenger = (string.Format("{0}",e.data["player1"]).Substring(1,4)==playerCode);
        //Set enemy outfit
        string gun, hat, vest, pants;
        if (isChallenger) 
        {
            gun = string.Format("{0}", player2Appearance["gun"]).Substring(1, 1);
            hat = string.Format("{0}", player2Appearance["hat"]).Substring(1, 1);
            vest = string.Format("{0}", player2Appearance["vest"]).Substring(1, 1);
            pants = string.Format("{0}", player2Appearance["pants"]).Substring(1, 1);  
            
        }
        else
        {
            gun = string.Format("{0}", player1Appearance["gun"]).Substring(1,1);
            hat = string.Format("{0}", player1Appearance["hat"]).Substring(1, 1);
            vest = string.Format("{0}", player1Appearance["vest"]).Substring(1, 1);
            pants = string.Format("{0}", player1Appearance["pants"]).Substring(1, 1);
            //pass player 1 rating
        }
        gameController.instance.player2.gameObject.SetActive(true);
        gameController.instance.player2.setGuns(int.Parse(gun));
        gameController.instance.player2.setHat(int.Parse(hat));
        gameController.instance.player2.setShirt(int.Parse(vest));
        gameController.instance.player2.setLegs(int.Parse(pants));
        uiController.instance.ShowPanel(uiController.instance.PreGamePanel);
        uiController.instance.PreGamePanel.CountDown();
        gameController.instance.beginGame();
        challengedCode = null;
    }
    private void draw(SocketIOEvent e)
    {
        distractionMessage = null;
        drawTime = GetTimestamp();
        uiController.instance.ShowPanel(uiController.instance.DrawPanel);
        if (vsAi)
            gameController.instance.promptAI(true);
            
    }

    private void showDistraction(SocketIOEvent e)
    {
        string[] message = {"Dude!", "D'oh!", "Dang!", "Darn!", "Drat!", "Derp!"};
        int index = UnityEngine.Random.Range(0, 5);
        distractionMessage = message[index];
        uiController.instance.ShowPanel(uiController.instance.DrawPanel);
        if (vsAi)
            gameController.instance.promptAI(false);
    }
    private void endDraw(SocketIOEvent e)
    {
        uiController.instance.HidePanel();
    }
    private void gameUpdate(SocketIOEvent e)
    {
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
    public void updateAppearance()
    {
        int[] settings = CustomizeAvatarPanel.GetSpriteArray();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["hat"] = settings[0].ToString();
        data["vest"] = settings[1].ToString();
        data["gun"] = settings[2].ToString();
        data["pants"] = settings[3].ToString();
        socket.Emit("updateAppearance", new JSONObject(data));

    }
    public void processInput()
    {
        var delta = GetTimestamp() - drawTime;
        Dictionary<string, string> data = new Dictionary<string, string>();
        Debug.Log("delta was "  + delta);
        data["delta"] = delta.ToString();
        socket.Emit("processInput", new JSONObject(data));
    }
    public void processAIInput()
    {
        socket.Emit("processAIInput");
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
    public void requestAIMatch()
    {
        vsAi = true;
        inMatchmaking = true;
        socket.Emit("challengeAI");
    }
    public void challenge(string s)
    {
        if (playerPing >= 500 && !pingWarning)
        {
            uiController.instance.ShowPanel(uiController.instance.PingPanel);
            pingWarning = true;
            return;
        }
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
        if (playerPing >= 500 && !pingWarning)
        {
            uiController.instance.ShowPanel(uiController.instance.PingPanel);
            pingWarning = true;
            return;
        }
        var name = PlayerPrefs.GetString("playerProfile");
        var rating = PlayerPrefs.GetInt("rating");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = name;
        data["rating"] = rating.ToString();
        inMatchmaking = true;
        socket.Emit("findMatch", new JSONObject(data));
        uiController.instance.ShowPanel(uiController.instance.ChallengingPanel);
    }

    public void requestLeaderboard()
    {
        socket.Emit("requestLeaderboard");
    }
    #endregion
    #region utility
    public static int GetTimestamp()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int ts = (int)t.TotalMilliseconds;
        return ts;
    }

    #endregion
}


