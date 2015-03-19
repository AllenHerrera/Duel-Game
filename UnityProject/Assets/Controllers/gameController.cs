using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{

    private static gameController _instance;
    //This is the public reference that other classes will use
    public static gameController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<gameController>();
            return _instance;
        }
    }

    //create possible game states
    public enum gameState { inMenu = 0, inGame = 1, inGameDraw = 2 };
    public enum playerState { idle = 0, firing = 1, jammed = 2, dead = 3 };
    public Sprite[] cowboySprites = new Sprite[4];
    //public enum distractionType { 1 , 2 , 3 , 4 , 5 };

    //Variables
    //initialize the currentstate as Menu
    private gameState currentState = gameState.inMenu;
    private bool drawActive = false;
    //container of player and opponent sprites and animations
    private SpriteRenderer player1, player2;
    private playerState player1State, player2State;


    //Public Methods
    //enables input controller, hides UI elemtents, begins game loop
    public void recieveGameState(gameState newState, playerState newPlayer1State, playerState newPlayer2State)
    {
        currentState = newState;
        player1State = newPlayer1State;
        player2State = newPlayer2State;
        player1.sprite = cowboySprites[(int)player1State];
        player2.sprite = cowboySprites[(int)player2State];
    }

    public void beginGame()
    {
        Debug.Log("Game is beginning!");
    }

    // receieve input from touch controller, check if valid, display result on screen, send result to sever
    public void processPlayerAction()
    {
        socketController.instance.processInput();
    }
    //call a distraction
    /*public void spawnDistraction(distractionType type)
    {
    }*/

    //private methods



    // Use this for initialization
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("player1").GetComponent<SpriteRenderer>();
        player2 = GameObject.FindGameObjectWithTag("player2").GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {

    }
}