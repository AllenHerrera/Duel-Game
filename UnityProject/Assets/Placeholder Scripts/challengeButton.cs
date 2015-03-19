using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class challengeButton : MonoBehaviour {
    public InputField code;
    private string _challengeCode ="";
    public string challengeCode
    {
        get
        {
            return _challengeCode;
        }
        set
        {
            _challengeCode = value.ToUpper();
            code.text = _challengeCode;

        }
    }
    void Start(){
        challengeCode ="";
    }
    public void SubmitChallenge()
    {
        if (challengeCode.Length == 4)
        {
            socketController.instance.challenge(challengeCode);
            code.text = "";
        }
    }
}
