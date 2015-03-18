using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

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
	public enum gameState { inMenu, inGame ,inGameDraw }; 
	public enum duelistAction { waiting, firing , jamming };
	//public enum distractionType { 1 , 2 , 3 , 4 , 5 };
	
	//Variables
	//initialize the currentstate as Menu
	public gameState currentState = gameState.inMenu;
	private float elapsedTime;
	//container of player and opponent sprites and animations
	private GameObject player, opponent;
	
	//Public Methods
	//enables input controller, hides UI elemtents, begins game loop
	public void beginGame() 
	{
        Debug.Log("Game is beginning!");
	}
	
	//recieve opponent input from server and show result on screen
	public void processOpponentAction(duelistAction state )
	{	
	}
	
	// receieve input from touch controller, check if valid, display result on screen, send result to sever
	public void processPlayerAction ()
	{
	}
	//call a distraction
	/*public void spawnDistraction(distractionType type)
	{
	}*/
	
	//private methods
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
