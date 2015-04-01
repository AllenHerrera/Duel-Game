using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class CameraController : MonoBehaviour
{
    #region singleton
    private static CameraController _instance;
    //This is the public reference that other classes will use
    public static CameraController instance
    {
        get { return _instance ?? (_instance = GameObject.FindObjectOfType<CameraController>()); }
    }
    #endregion
    #region variables & initialization
    private Vector3 originalPosition= new Vector3(0,0,-10), activePosition = new Vector3(0,-2,-10);
    private float originalZoom = 3.5f, activeZoom = 5f, transitionTime = 2.5f;
    private Transform cameraTransform;
    private Camera mainCamera;
    void Start()
    {
        
        mainCamera = FindObjectOfType<Camera>();
        cameraTransform = mainCamera.GetComponent<Transform>();
    }
    #endregion

    public void TransitionToGame()
    {
        uiController.instance.HideTitle();
        cameraTransform.DOMove(activePosition, transitionTime).SetEase(Ease.InOutSine);
        DOTween.To(x => mainCamera.orthographicSize = x, originalZoom, activeZoom, transitionTime)
            .SetEase(Ease.InOutSine);
        uiController.instance.Title.DOFade(0, transitionTime);
    }
    public void TransitionToMenu()
    {
        uiController.instance.ShowTitle();
        cameraTransform.DOMove(originalPosition, transitionTime).SetEase(Ease.InOutSine);
        DOTween.To(x => mainCamera.orthographicSize = x, activeZoom, originalZoom, transitionTime).SetEase(Ease.InOutSine);
        uiController.instance.Title.DOFade(0, transitionTime);
    }
}
