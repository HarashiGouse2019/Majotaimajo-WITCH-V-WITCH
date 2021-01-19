﻿using UnityEngine;
using TMPro;
using System.Collections;
using Extensions;

public class PlayerPawn : Pawn
{
    public static new PlayerPawn Instance;

    public EnemyPawn target;

    [SerializeField]
    private AutoOrbit[] autoOrbitObjs;

    public PlayerController controller;

    //Since you are the player, you'll have an object that handles the shooting for you.
    //For example, Maple's Star or Raven's Crystal Circle.
    //It will be a prefab
    public GameObject pawnEmitterPrefab;

    public float movementSpeed;
    public float speedReduction;
    public float rotationSpeed;

    public Transform originOfRotation;

    public float radius = 6f;
    readonly public float radiusSpeed = 5f;
    public bool isMoving;

    public bool isMagicActivelyUsed = false;
    public bool isSneaking = false;

    const int ZERO = 0;

    [SerializeField]
    LinearEmitter emitter;

    //Player Stats
    Stats PlayerStats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    private void Start()
    {
        PlayerStats = Stats.New();

        AssignValuesFromStats();

        priority = basePriority;

        GameManager.Instance.tRenderer.check = true;
        GameManager.Instance.SetMaxMagic(PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.MAGIC));
        GameManager.Instance.IncrementMagic(PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.MAGIC));

        //Get Components
        srenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();

        isVisible = srenderer.isVisible;
        xScale = transform.localScale;
        xScaleVal = xScale.x;
        srendererColor = srenderer.color;

        library.SetCaster(this);

        //Start Recovery Sequence
        PassiveRecoveryCycle().Start();

        //Start Hit Cycle Coroutine
        BlinkCycle(0.05f).Start();

        //Start Focus Cycle Coroutine
        FocusCycle().Start();

        //Read your SpellLibrary, and override GameUi spell text
        try
        {
            for (int i = 0; i < library.spells.Count; i++)
            {
                GameManager.Instance.SLOTS[i].GetComponentInChildren<TextMeshProUGUI>().text = library.spells[i].name;
            }
        }
        catch { /*These hands!*/}
    }

    public T GetEmitter<T>() where T : Emitter => (T)System.Convert.ChangeType(emitter, typeof(T));

    private void Update()
    {
        if (recoil == true) Wait(0.05f);

        if (hit == true)
            Blink(5f);
        else
        {
            isVisible = true;
            srenderer.color = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
        }
    }

    #region Unique

    IEnumerator FocusCycle()
    {
        while (true)
        { 
            for (int index = 0; index < autoOrbitObjs.Length; index++)
            {
                AutoOrbit orbitObj = autoOrbitObjs[index];
                orbitObj.ChangeRadius(isSneaking);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Assign Game Values to PlayerPawn
    /// </summary>
    void AssignValuesFromStats()
    {
        basePriority = (uint)PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.BASEPRIORITY);
        movementSpeed = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.SPEED);
        speedReduction = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.EVASIVENESS);
    }

    /// <summary>
    /// Get stats from player spawn
    /// </summary>
    /// <returns></returns>
    public Stats GetStats() => PlayerStats;

    /// <summary>
    /// Recover certain amount of value
    /// </summary>
    /// <param name="value"></param>
    void RecoverMagic(float value)
    {
        GameManager.Instance.IncrementMagic(value);
    }

    /// <summary>
    /// Recover Raven's magic slowly when not actively using spells or shooting
    /// </summary>
    /// <returns></returns>
    IEnumerator PassiveRecoveryCycle()
    {
        while (true)
        {
            if (!isMagicActivelyUsed && GameManager.Instance.GetPlayerMagic() < 100f)
            {
                RecoverMagic(0.05f);
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
    #endregion

    public override void Shoot(string bulletName)
    {
        if (recoil == false)
        {
            emitter.SetPawnParent(this);
            emitter.SetBulletInitialSpeed(650);
            emitter.SpawnBullets(1, bulletName);
            AudioManager.Play("Shoot000", _oneShot: true);
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

    public override void ActivateSpell(string _name, bool cancelRunningSpell = false)
    {
        Spell spell = library.FindSpell(_name);

        GameManager.Instance.ActivateSlot((int)library.GetSpellIndex(_name), true);

        if (GameManager.Instance.GetPlayerMagic() > spell.magicConsumtion && library.spellInUse == null)
        {
            library.spellInUse = spell;

            GameManager.Instance.DecrementMagic(spell.magicConsumtion);

            isMagicActivelyUsed = true;

            //Increate pawn's priority!!!
            priority += spell.spellPriority;

            spell.Activate(this);
        }
    }

    public override void Wait(float _duration)
    {
        timer.StartTimer(2);
        returnVal = timer.SetFor(_duration, 2);
        if (returnVal == true) { recoil = false; timer.SetToZero(2, true); }
    }

    public override void Blink(float _duration)
    {
        if (hit == true)
        {

            //Timer for duration
            timer.StartTimer(1);

            //Since there's a timer in Pawn, and I've initialized 12, I'm going to use alarm 6
            //We'll pass the blink rate to our SetFor method.
            returnVal = timer.SetFor(_duration, 1, true);

            if (returnVal)
            {
                hit = false;
                timer.SetToZero(1, true);
            }
        }
    }

    IEnumerator BlinkCycle(float _blinkRate)
    {
        while (true)
        {
            if (hit)
            {
                if (GameManager.Instance.GetPlayerLives() < 1 && !GameManager.Instance.NoDeaths)
                {
                    GameSceneManager.Instance.LoadScene("RECORDSANDHIGHSCORE");
                }

                if (isVisible)
                {
                    Color invisible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 0f);
                    srenderer.color = invisible;
                    isVisible = false;
                }
                else
                {
                    Color visible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
                    srenderer.color = visible;
                    isVisible = true;
                }

                yield return new WaitForSecondsRealtime(_blinkRate);
            }

            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        GetOrignatedSpawnPoint objectOrigin = other.GetComponent<GetOrignatedSpawnPoint>();

        if (objectOrigin != null && objectOrigin.pawn != this)
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
        move = new Vector2(rb.velocity.x, (movementSpeed - (isSneaking ? speedReduction / 2 : ZERO)) * 10) * Time.fixedDeltaTime;
        rb.AddForce(move);

    }
    public override void Back()
    {
        move = new Vector2(rb.velocity.x, -(movementSpeed - (isSneaking ? speedReduction / 2 : ZERO)) * 10) * Time.fixedDeltaTime;
        rb.AddForce(move);
    }

    public override void Left()
    {
        move = new Vector2(-(movementSpeed - (isSneaking ? speedReduction / 2 : ZERO)) * 10, rb.velocity.y) * Time.fixedDeltaTime;
        rb.AddForce(move);
    }

    public override void Right()
    {
        move = new Vector2((movementSpeed - (isSneaking ? speedReduction / 2 : ZERO)) * 10, rb.velocity.y) * Time.fixedDeltaTime;
        rb.AddForce(move);
    }
}