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
    public enum gameState { inactive = 0, active = 1, over = 2 };
    public enum playerState { idle = 0, firing = 1, jammed = 2, dead = 3 };
    public AudioClip[] sounds = new AudioClip[3];
    public Sprite[] cowboySprites = new Sprite[4];

    //Variables
    private gameState currentState = gameState.inactive;
    private AudioSource audio;
    private bool isPlayer1;
    //container of player and opponent sprites and animations
    private SpriteRenderer player1, player2;
    private playerState player1State, player2State;

    #region private methods
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("player1").GetComponent<SpriteRenderer>();
        player2 = GameObject.FindGameObjectWithTag("player2").GetComponent<SpriteRenderer>();
        player1.sprite = null;
        player2.sprite = null;
        audio = GetComponent<AudioSource>();
    }
    private IEnumerator gameOver(bool isWinner)
    {
        audio.clip = sounds[0];
        audio.Play();
        yield return new WaitForSeconds(.5f);
        audio.clip = sounds[1];
        audio.Play();
        yield return new WaitForSeconds(2.5f);
        if (isWinner)
            uiController.instance.showWonPanel();
        else
            uiController.instance.showLostPanel();
        
    }
    #endregion
    #region public methods
    public void beginGame(bool playerOne)
    {
        isPlayer1 = playerOne;
        if (isPlayer1)
        {
            uiController.instance.showPlayer1Label();
        }
        else
        {
            uiController.instance.showPlayer2Label();
        }
        currentState = gameState.active;
        player1.sprite = cowboySprites[0];
        player2.sprite = cowboySprites[0];
        player1State = playerState.idle;
        player2State = playerState.idle;
    }
    public void recieveGameState(int newState, int newPlayer1State, int newPlayer2State)
    {
        currentState = (gameState)newState;
        player1State = (playerState)newPlayer1State;
        player2State = (playerState)newPlayer2State;
        if ((player1State == playerState.jammed && isPlayer1) || (player2State == playerState.jammed && !isPlayer1))
        {
            audio.clip = sounds[2];
            audio.Play();
        }
        player1.sprite = cowboySprites[(int)player1State];
        player2.sprite = cowboySprites[(int)player2State];
        if (currentState == gameState.over)
        {
            uiController.instance.hideDraw();
            uiController.instance.hidePlayerLabel();
            bool wonGame = (isPlayer1 && player1State == playerState.firing) || (!isPlayer1 && player2State == playerState.firing);
            StartCoroutine(gameOver(wonGame));


        }
    }
    public void processPlayerAction()
    {
        socketController.instance.processInput();
    }
    public void resetGameState()
    {
        currentState = gameState.inactive;
        player1.sprite = null;
        player2.sprite = null;
    }
    #endregion    
}