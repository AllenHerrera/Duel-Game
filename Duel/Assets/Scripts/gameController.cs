using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{
    #region singleton
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
    #endregion
    #region variables & initialization
    public enum gameState { inactive = 0, active = 1, over = 2 };
    public enum playerState { idle = 0, firing = 1, jammed = 2, dead = 3 };
    public bool wonGame { get; private set; }
    public gameState currentState { get; private set; }
    //To be replaced by resource.load
    public AudioClip[] sounds = new AudioClip[3];
    public Sprite[] cowboySprites = new Sprite[4];
    public SpriteRenderer player1, player2;
    private AudioSource audio;
    //container of player and opponent sprites and animations
    private playerState _playerState, opponentState;
    void Start()
    {
        player1.sprite = null;
        player2.sprite = null;
        audio = GetComponent<AudioSource>();
        currentState = gameState.inactive;
    }
    #endregion
    #region private methods
    private IEnumerator gameOver()
    {
        audio.clip = sounds[0];
        audio.Play();
        yield return new WaitForSeconds(.5f);
        audio.clip = sounds[1];
        audio.Play();
        yield return new WaitForSeconds(2.5f);
        uiController.instance.ShowPanel(uiController.instance.PostGamePanel);
        
    }
    #endregion
    #region public methods
    public void beginGame()
    {
        currentState = gameState.active;
        player1.sprite = cowboySprites[0];
        player2.sprite = cowboySprites[0];
        _playerState = playerState.idle;
        opponentState = playerState.idle;
        inputController.instance.EnableInput();
    }
    public void recieveGameState(int newState, int newPlayerState, int newOpponentState)
    {
        bool wasJammed = ((_playerState == playerState.jammed && (playerState)newPlayerState != playerState.jammed) || (opponentState == playerState.jammed && (playerState)newOpponentState != playerState.jammed));
        currentState = (gameState)newState;
        _playerState = (playerState)newPlayerState;
        opponentState = (playerState)newOpponentState;
        if ((_playerState == playerState.jammed) || (opponentState == playerState.jammed) || wasJammed)
        {
            audio.clip = sounds[2];
            audio.Play();
        }
        player1.sprite = cowboySprites[(int)_playerState];
        player2.sprite = cowboySprites[(int)opponentState];
        if (currentState == gameState.over)
        {
            inputController.instance.DisableInput();
            wonGame = ((socketController.instance.isChallenger && _playerState == playerState.firing) || (!socketController.instance.isChallenger && opponentState == playerState.firing));
            StartCoroutine(gameOver());

        }
    }
    public void processPlayerAction()
    {
        socketController.instance.processInput();
    }
    public void resetGameState()
    {
        currentState = gameState.inactive;    
    }
    #endregion    
}