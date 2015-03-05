using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	
	private static readonly gameController instance = new gameController();
	private gameController(){}
	
	//allows only one instance of this object and provides a static reference to it
	public static gameController Instance
	{
		get 
		{
			return instance; 
		}
	}
	
	//create possible game states
	public enum gameState { inMenu, inGame ,inGameDraw }; 
	public enum duelistAction { waiting, firing , jamming };
	public enum distractionType { 1 , 2 , 3 , 4 , 5 };
	
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
	public void spawnDistraction(distractionType type)
	{
	}
	
	//private methods
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
