﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class drawPanel : menuPanel
{
    private Text DrawText;
    private AudioSource sound;
    protected override void Start()
    {
        base.Start();
        sound = GetComponent<AudioSource>();
        //override transition duration to be quicker
        transitionDuration = .3f;
        DrawText = GameObject.Find("DrawText").GetComponent<Text>();
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
        if (socketController.instance.distractionMessage != null)
        {
            DrawText.text = socketController.instance.distractionMessage;
            sound.Play();
        }
        else
        {
            DrawText.text = "Draw!";
        }
        panelTransform.gameObject.SetActive(true);
        panel.DOFade(1, transitionDuration).SetEase(Ease.InOutSine);
    }
    public override void TransitionOut()
    {
        panel.DOFade(0, transitionDuration).SetEase(Ease.InOutSine);
        StartCoroutine(Disable());        
    }
}
