using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class Pawn : MonoBehaviour
{
    public static Pawn Instance;


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

    public uint priority = 1;
    public uint basePriority;

    public SpellLibrary library;
    #endregion

    #region Protected Members

    //We'll add the important stuff
    protected Vector2 startPoint;
    public float g_angle = 0f;

    protected bool recoil = false;
    protected bool returnVal;
    protected bool hit;

    protected readonly Timer timer = new Timer(3, true);
    protected Vector3 xScale;
    protected float xScaleVal;
    protected SpriteRenderer srenderer;
    protected Rigidbody2D rb;
    protected DanmakuSequencer sequencer;
    
    protected Vector2 move;
    protected bool isVisible;
    protected Color srendererColor;

    #endregion
    private void Awake()
    {
        Instance = this;
    }


    public virtual void Shoot(int _index)
    {

    }

    public virtual bool CheckIfMoving()
    {
        return false;
    }

    public virtual void Flip(int _direction)
    {

    }

    public virtual void ActivateSpell(string _name)
    {
        Spell spell = library.FindSpell(_name);

        if (SpellLibrary.library.spellInUse == null)
        {
            library.spellInUse = spell;

            //Increate pawn's priority!!!
            priority = spell.spellPriority;
            //We give all values to our Sequencer
            sequencer.stepSpeed = spell.stepSpeed;
            //We have to loop each routine, and add them the list
            for (int routinePos = 0; routinePos < spell.routine.Count; routinePos++)
            {
                sequencer.routine.Add(spell.routine[routinePos]);

                //And then we check if we enable looping
                sequencer.enableSequenceLooping = spell.enableSequenceLooping;
            }

            //Now that all value have passed in, we enable
            sequencer.enabled = true;
        }
    }

    public virtual void Wait(float _duration)
    {

    }

    public virtual void GetHurt(float _blinkRate, float _duration)
    {

    }

    public virtual void Foward()
    {

    }

    public virtual void Back()
    {

    }

    public virtual void Left()
    {

    }

    public virtual void Right()
    {

    }

}
