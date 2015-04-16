using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour
{
    GameObject duelFigure;
    Animation duelAnim;
    private Vector3 origin;
    public int playerNumber;
    bool clicked = false;
    int hatSetting;
    private SpriteRenderer torso, legs, hat, gun;
    public Sprite[] torsoOptions = new Sprite[4];
    public Sprite[] legsOptions = new Sprite[4];
    public Sprite[] hatsOptions = new Sprite[4];
    public Sprite[] gunsOptions = new Sprite[4];

    public enum chooseAnimations { Idle, Shoot, Dead, Jam };


    public void animationChoice(chooseAnimations anim)
    {

        switch (anim)
        {
            case chooseAnimations.Idle:
                this.gameObject.GetComponent<Animation>().Play("Idle");
                break;
            case chooseAnimations.Jam:
                this.gameObject.GetComponent<Animation>().Play("Jam");
                break;
            case chooseAnimations.Shoot:
                this.gameObject.GetComponent<Animation>().Play("Shoot");
                break;
            case chooseAnimations.Dead:
                if (playerNumber == 1)
                    this.gameObject.GetComponent<Animation>().Play("Dead");
                else
                    this.gameObject.GetComponent<Animation>().Play("Dead(Mirror)");
                break;

        }
    }

    private void Start()
    {
        origin = transform.position;
        torso = GameObject.Find("shirt" + playerNumber).GetComponent<SpriteRenderer>();
        hat = GameObject.Find("hat" + playerNumber).GetComponent<SpriteRenderer>();
        legs = GameObject.Find("legs" + playerNumber).GetComponent<SpriteRenderer>();
        gun = GameObject.Find("gun" + playerNumber).GetComponent<SpriteRenderer>();
        torso.sprite = torsoOptions[0];
        hat.sprite = hatsOptions[0];
        legs.sprite = legsOptions[0];
        gun.sprite = gunsOptions[0];
    }
    public void setHat(int option)
    {
        hat.sprite = hatsOptions[option];
    }
    public void setShirt(int option)
    {
        torso.sprite = torsoOptions[option];
    }
    public void setLegs(int option)
    {
        legs.sprite = legsOptions[option];
    }
    public void setGuns(int option)
    {
        gun.sprite = gunsOptions[option];
    }
    public void reset()
    {
        transform.position = origin;
        animationChoice(chooseAnimations.Idle);
    }
}