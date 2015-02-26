using UnityEngine;
using System.Collections;

public class challengeButton : MonoBehaviour {
    public string challengeCode{get;set;}
    private sockettest socket;
    private void Start(){
        socket = FindObjectOfType<sockettest>();
    }
    public void SubmitChallenge()
    {
        socket.challenge(challengeCode);
    }
}
