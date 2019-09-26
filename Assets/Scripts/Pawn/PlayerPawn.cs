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

    //We'll add the important stuff
    Vector2 startPoint;
    public float g_angle = 0f;

    bool recoil = false;
    bool returnVal;
    bool hit;

    readonly Timer timer = new Timer(3, true);
    Vector3 xScale;
    float xScaleVal;
    SpriteRenderer srenderer;
    Rigidbody2D rb;
    DanmakuSequencer sequencer;
    SpellLibrary library;
    Vector2 move;
    bool isVisible;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        srenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();

        player = this;
        isVisible = srenderer.isVisible;
        xScale = transform.localScale;
        xScaleVal = xScale.x;
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

    public void ActivateSpell(string _name)
    {
        GameManager manager = GameManager.Instance;

        Spell spell = library.FindSpell(_name);

        //What index is the spell; This is for UI purposes;
        manager.ActivateSlot(manager.SLOTS[library.GetSpellIndex(_name)], _on:true);
        
        //We give all values to our Sequencer
        sequencer.stepSpeed = spell.stepSpeed;

        //We have to loop each routine, and add them the list
        for (int routinePos = 0; routinePos < spell.routine.Count; routinePos++)
        {
            sequencer.routine.Add(spell.routine[routinePos]);
        }

        //And then we check if we enable looping
        sequencer.enableSequenceLooping = spell.enableSequenceLooping;

        //Now that all value have passed in, we enable
        sequencer.enabled = true;
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
                if (other.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint.name == "Luu_Obj")
                {
                    if (hit == false)
                    {
                        hit = true;
                        GameManager.Instance.DecrementLives();
                    }
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
