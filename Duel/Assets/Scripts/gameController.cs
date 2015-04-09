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
    public animationController Player;
    //container of player and opponent sprites and animations
    private playerState _playerState, opponentState;
    void Start()
    {
        player1.sprite = cowboySprites[0];
        player2.sprite = cowboySprites[0];
        audio = GetComponent<AudioSource>();
        currentState = gameState.inactive;
    }
    #endregion
    #region private methods
    private IEnumerator aiResponse(float responseTime)
    {
        //After x delay, respond
        yield return new WaitForSeconds(responseTime/1000);
        socketController.instance.processAIInput();

    }
    private IEnumerator gameOver()
    {
        audio.clip = sounds[0];
        audio.Play();
        yield return new WaitForSeconds(.5f);
        audio.clip = sounds[1];
        audio.Play();
        yield return new WaitForSeconds(2.5f);
        uiController.instance.ShowPanel(uiController.instance.PostGamePanel);
        CameraController.instance.TransitionToMenu();
    }
    #endregion
    #region public methods
    public void promptAI(bool isDraw)
	{
		var responseTime = Random.Range(350,1000);
		//if isdraw determine delay and always respond
		if (isDraw) 
		{
			StartCoroutine (aiResponse (responseTime));
		}
        //else determine delay and determine if respond
		else 
		{
			var ResponseChoice = Random.value;

			if (ResponseChoice < .6)
				StartCoroutine (aiResponse (responseTime));  
  		}
    }
    public void beginGame()
    {
        currentState = gameState.active;
        player1.sprite = cowboySprites[0];
        player2.sprite = cowboySprites[0];
        _playerState = playerState.idle;
        opponentState = playerState.idle;
        Player.animationChoice(animationController.chooseAnimations.Idle);
        CameraController.instance.TransitionToGame();
    }
    public void recieveGameState(int newState, int newPlayerState, int newOpponentState, bool won)
    {
        wonGame = !won;
        bool wasJammed = ((_playerState == playerState.jammed && (playerState)newPlayerState != playerState.jammed) || (opponentState == playerState.jammed && (playerState)newOpponentState != playerState.jammed));
        currentState = (gameState)newState;
        _playerState = (playerState)newPlayerState;
        opponentState = (playerState)newOpponentState;
        switch (_playerState)
        {
            case playerState.idle:
                Player.animationChoice(animationController.chooseAnimations.Idle);
                break;
            case playerState.firing:
                Player.animationChoice(animationController.chooseAnimations.Shoot);
                break;
            case playerState.dead:
                Player.animationChoice(animationController.chooseAnimations.Dead);
                break;
            case playerState.jammed:
                Player.animationChoice(animationController.chooseAnimations.Jam);
                break;


        }
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
            StartCoroutine(gameOver());
        }
    }
    public void processPlayerAction()
    {
        socketController.instance.processInput();
    }
    public void resetGameState()
    {
        CameraController.instance.TransitionToMenu();
        player1.sprite = cowboySprites[0];
        player2.sprite = null;
        currentState = gameState.inactive;    
    }
    #endregion    
}