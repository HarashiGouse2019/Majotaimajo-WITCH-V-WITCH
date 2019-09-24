using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class PlayerPawn : MonoBehaviour
{
    public static PlayerPawn player;

    #region Public Members
    //Our movment speeds
    public PlayerController controller;

    public float movementSpeed;
    public float rotationSpeed;
    public float maxSpeed;

    public Transform originOfRotation;

    public float radius = 6f;
    readonly public float radiusSpeed = 5f;
    public bool isMoving;

    #endregion


    #region Private Members
    //We'll need our Transform
    Transform pawnTransform;

    //We'll add the important stuff
    Vector2 startPoint;
    public float g_angle = 0f;

    bool recoil = false;
    bool returnVal;
    bool hit;

    readonly Timer timer = new Timer(3, true);
    SpriteRenderer srenderer;
    Rigidbody2D rb;
    Vector2 move;
    bool isVisible;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = this;
        pawnTransform = GetComponent<Transform>();
        srenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isVisible = srenderer.isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (recoil == true) Wait(0.05f);
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing

        //For our blinking effect
        if (hit == true)
        {
            GetHurt(0.15f, 5f);
            srenderer.enabled = isVisible;
        }
        else
        {
            isVisible = true;
            srenderer.enabled = isVisible;
        }
    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    public void CalculateAngle()
    {
        g_angle = Mathf.Sin(transform.position.y);
    }

    public void MoveOnDiameter(float _speed)
    {
        //This will calculate the vector between Raven and Luu, and will move on that normalized vector
        radius += _speed * Time.deltaTime;
    }

    public void Shoot(int _index)
    {
        if (recoil == false)
        {
            Standard_Shoot.Instance = GetComponent<Standard_Shoot>();
            Standard_Shoot.Instance.SpawnBullets(1, _index);
            recoil = true;
        }
    }

    public bool CheckIfMoving()
    {
        return isMoving;
    }

    public void Flip(int _direction)
    {
        Vector3 xScale = transform.localScale;
        xScale.x = _direction;

        transform.localScale = xScale;
    }

    public void ActivateSpell(SequencerManager.Spell _spell)
    {
        _spell.sequence.enabled = true;
    }

    void Wait(float _duration)
    {
        timer.StartTimer(2);
        returnVal = timer.SetFor(_duration, 2);
        if (returnVal == true) recoil = false;
    }

    private void GetHurt(float _blinkRate, float _duration)
    {
        if (hit == true)
        {
            if (GameManager.Instance.GetLives() < 1)
            {
                Application.Quit();
            }

            timer.StartTimer(0);
            timer.StartTimer(1);
            //I want it so that the player is blinking on and off for a certain duration of time.
            //That would mean getting to the Sprite Renderer, and enabling it and disabling it after
            //certain intervals.

            //Since there's a timer in Pawn, and I've initialized 12, I'm going to use alarm 6
            //We'll pass the blink rate to our SetFor method.
            returnVal = timer.SetFor(_duration, 1, true);
            if (timer.SetFor(_blinkRate, 0))
            {
                if (isVisible) isVisible = false;
                else if (isVisible == false) isVisible = true;
            }

            if (returnVal)
            {
                hit = false;
                timer.SetToZero(0, true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.name)
        {
            case "testBullet(Clone)":
                if (hit == false)
                {
                    hit = true;
                    GameManager.Instance.DecrementLives();
                }
                break;
            case "Luu_Obj":
                if (hit == false)
                {
                    hit = true;
                    GameManager.Instance.DecrementLives();
                }
                break;
            default:

                break;
        }
    }



    public void Foward()
    {
        move = new Vector2(rb.velocity.x, movementSpeed);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;

    }
    public void Back()
    {
        move = new Vector2(rb.velocity.x, -movementSpeed);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }

    public void Left()
    {
        move = new Vector2(-movementSpeed, rb.velocity.y);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }

    public void Right()
    {
        move = new Vector2(movementSpeed, rb.velocity.y);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }
}
