using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class PlayerPawn : Pawn
{
    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    private void Start()
    {
        //Get Components
        srenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();

        isVisible = srenderer.isVisible;
        xScale = transform.localScale;
        xScaleVal = xScale.x;
        srendererColor = srenderer.color;

        //Set base priority at start
        basePriority = priority;
    }

    private void Update()
    {
        if (recoil == true) Wait(0.05f);
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing

        //For our blinking effect
        if (hit == true)
        {
            GetHurt(0.15f, 5f);
        }
        else
        {

            isVisible = true;
            srenderer.color = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
        }
    }

    public override void Shoot(int _index)
    {
        if (recoil == false)
        {
            Standard_Shoot.Instance = GetComponent<Standard_Shoot>();
            Standard_Shoot.Instance.SpawnBullets(1, _index);
            AudioManager.audio.Play("Shoot000", 100f);
            recoil = true;
        }
    }

    public override bool CheckIfMoving()
    {
        return isMoving;
    }

    public override void Flip(int _direction)
    {


        switch (_direction)
        {
            case 1:
                xScale.x = xScaleVal;
                break;
            case -1:
                xScale.x = -xScaleVal;
                break;
        }


        transform.localScale = xScale;
    }

    public override void ActivateSpell(string _name)
    {
       
        base.ActivateSpell(_name);
    }

    public override void Wait(float _duration)
    {
        timer.StartTimer(2);
        returnVal = timer.SetFor(_duration, 2);
        if (returnVal == true) recoil = false;
    }

    public override void GetHurt(float _blinkRate, float _duration)
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
                if (isVisible)
                {
                    Color invisible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 0f);
                    srenderer.color = invisible;
                    isVisible = false;
                }
                else if (isVisible == false)
                {
                    Color visible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
                    srenderer.color = visible;
                    isVisible = true;
                }
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

        if (other.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint.name == "Luu_Obj")
        {
            if (hit == false)
            {
                hit = true;
                GameManager.Instance.DecrementLives();
            }
        }
    }

    public override void Foward()
    {
        move = new Vector2(rb.velocity.x, movementSpeed);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;

    }
    public override void Back()
    {
        move = new Vector2(rb.velocity.x, -movementSpeed);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }

    public override void Left()
    {
        move = new Vector2(-movementSpeed, rb.velocity.y);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }

    public override void Right()
    {
        move = new Vector2(movementSpeed, rb.velocity.y);
        if (rb.velocity.magnitude < maxSpeed)
            rb.velocity += move * Time.fixedDeltaTime;
    }

    public void FindRadius()
    {
        radius = Vector2.Distance(originOfRotation.position, transform.position);
    }
}