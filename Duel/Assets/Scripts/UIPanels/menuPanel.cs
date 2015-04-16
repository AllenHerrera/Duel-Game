using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public abstract class menuPanel : MonoBehaviour
{
    #region variables & initialization
	public enum ButtonAction { challenge, matchmaking, leaderboard, options, quit, cancelChallenge, returnToMain, acceptChallenge, declineChallenge, playAgain, aiMatch}; //hat0,hat1,vest0,vest1,gun0,gun1,pants0,pants1
    protected CanvasGroup panel;
    protected RectTransform panelTransform;
    protected Vector2 originalPosition = new Vector2(275, 0), activePosition = new Vector2(-275, 0);
    protected float transitionDuration = .5f;
    protected virtual void Start () {
        panel = GetComponent<CanvasGroup>();
        panelTransform = GetComponent<RectTransform>();
	}
    #endregion
    #region public methods
    //Animate Panel coming into view. Sets default animation that can be overridden if needed
    public virtual void TransitionIn()
    {
        panelTransform.DOAnchorPos(activePosition, transitionDuration,true).SetEase(Ease.InOutSine);
        panel.DOFade(1, transitionDuration).SetEase(Ease.InOutSine);
    }
    //Animate Panel leaving view. Sets default animation that can be overridden if needed
	public virtual void TransitionOut()
    {
        panelTransform.DOAnchorPos(originalPosition, transitionDuration, true).SetEase(Ease.InOutSine);
        panel.DOFade(0, transitionDuration).SetEase(Ease.InOutSine);
    }
    #endregion
    #region private methods
    protected abstract void ProcessButtonPress(ButtonAction btn);
    #endregion
}
