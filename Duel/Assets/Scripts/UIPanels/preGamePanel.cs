using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class preGamePanel : menuPanel {
    private Text beginText;
    private Text title;
    protected override void Start()
    {
        base.Start();
        title = GameObject.Find("TitleText").GetComponent<Text>();
        beginText = GameObject.Find("BeginText").GetComponent<Text>();
    }
    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(transitionDuration);
        panelTransform.gameObject.SetActive(false);
    }
    protected override void ProcessButtonPress(menuPanel.ButtonAction btn)
    {
        return;
    }
    public override void TransitionIn()
    {
        uiController.instance.HideTitle();
        panelTransform.gameObject.SetActive(true);
        panel.DOFade(1, transitionDuration).SetEase(Ease.InOutSine);
    }
    public override void TransitionOut()
    {
        panel.DOFade(0, transitionDuration).SetEase(Ease.InOutSine);
        StartCoroutine(Disable());        
    }
    public void CountDown()
    {
        Debug.Log("Counting down!");
        StartCoroutine(DisplayCountdown());
    }
    private IEnumerator DisplayCountdown()
    {
        title.DOFade(0, 2);
        string[] message = new string[4]{"3","2","1","Begin!"};
        for(int i = 0; i< message.Length; i++){
            beginText.text = message[i];
            beginText.DOFade(1, .6f);
            yield return new WaitForSeconds(.5f);
            beginText.DOFade(0, .4f);
            yield return new WaitForSeconds(.5f);
        }
        beginText.DOFade(0, .5f);
        inputController.instance.EnableInput();
        //beginText.DOText(message[index], 1).SetLoops(message.Length).OnComplete(() => { index++; Debug.Log(index); if (index > 5)uiController.instance.HidePanel(); }).SetDelay(.5f);
    }
   
}
