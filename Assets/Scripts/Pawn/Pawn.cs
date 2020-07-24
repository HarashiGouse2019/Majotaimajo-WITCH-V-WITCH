using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;
using System;

public abstract class Pawn : MonoBehaviour
{
    public static Pawn Instance;

    #region Public Members
    //Our movment speeds
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

    protected readonly Timer timer = new Timer(3);
    protected Vector3 xScale;
    protected float xScaleVal;
    protected SpriteRenderer srenderer;
    protected Rigidbody2D rb;
    protected DanmakuSequencer sequencer;
    
    protected Vector2 move;
    protected bool isVisible;
    protected Color srendererColor;
    protected GetOrignatedSpawnPoint objectOrigin;
    public Animator Animator { get; private set; }

    #endregion
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Shoot a projectile based on its name.
    /// </summary>
    /// <param name="bulletName"></param>
    public virtual void Shoot(string bulletName)
    {

    }

    /// <summary>
    /// Check if the pawn is moving
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckIfMoving()
    {
        return false;
    }

    /// <summary>
    /// Flip the pawn to either -1 (left) or 1 (right)
    /// </summary>
    /// <param name="_direction"></param>
    public virtual void Flip(int _direction)
    {

    }

    /// <summary>
    /// Activate a spell from the pawn's Spell Library
    /// </summary>
    /// <param name="_name"></param>
    public virtual void ActivateSpell(string _name, bool cancelRunningSpell = false)
    {

    }

    /// <summary>
    /// Wait for a duration amount of time
    /// </summary>
    /// <param name="_duration"></param>
    public virtual void Wait(float _duration)
    {

    }

    /// <summary>
    /// Wait for a duration amount of time before executing a method
    /// </summary>
    /// <param name="_duration"></param>
    /// <param name="method"></param>
    public virtual void Wait(float _duration, Action method)
    {
        
    }

    /// <summary>
    /// Blink the sprite at a rate for a set duration of time
    /// </summary>
    /// <param name="_blinkRate"></param>
    /// <param name="_duration"></param>
    public virtual void Blink(float _duration)
    {

    }

    /// <summary>
    /// Move foward.
    /// </summary>
    public virtual void Foward()
    {

    }

    /// <summary>
    /// Move backwards
    /// </summary>
    public virtual void Back()
    {

    }

    /// <summary>
    /// Move left
    /// </summary>
    public virtual void Left()
    {

    }

    /// <summary>
    /// Move right
    /// </summary>
    public virtual void Right()
    {

    }

}
