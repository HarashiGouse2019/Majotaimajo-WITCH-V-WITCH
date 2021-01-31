using UnityEngine;
using TMPro;
using System.Collections;
using Extensions;

public class PlayerPawn : Pawn
{
    public enum AnimationMotion { 
        Idle = 0,
        MoveLeft = 1,
        MoveRight = 2
    }


    public static new PlayerPawn Instance;

    public EnemyPawn target;

    public Animator characterAnimator;

    [SerializeField]
    private AutoOrbit[] autoOrbitObjs;

    public PlayerController controller;

    //Since you are the player, you'll have an object that handles the shooting for you.
    //For example, Maple's Star or Raven's Crystal Circle.
    //It will be a prefab
    public GameObject pawnEmitterPrefab;

    public float MovementSpeed { get; set; }
    public int Evasivness { get; set; }

    public float FocusSpeed { get; set; }
    public float DashSpeed { get; set; }
    public bool IsMoving { get; set; }

    public bool IsMagicActivelyUsed { get; set; } = false;
    public bool IsOnFocus { get; set; } = false;

    private const int ZERO = 0;
    private const int DOUBLE = 2;
    [SerializeField]
    LinearEmitter emitter;

    //Player Stats
    Stats PlayerStats;

    string[] motionNames =
    {
        "Idle",
        "MoveLeft",
        "MoveRight"
    };

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
        PlayerStats = GameManager.CharacterStats;

        AssignValuesFromStats();

        priority = basePriority;

        GameManager.Instance.tRenderer.check = true;
        GameManager.Instance.SetMaxMagic(PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.MAGIC) * 100);
        GameManager.Instance.IncrementMagic(PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.MAGIC) * 100);

        //Get Components
        rb = GetComponent<Rigidbody2D>();
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();

        isVisible = characterRenderer.isVisible;
        xScale = transform.localScale;
        xScaleVal = xScale.x;
        srendererColor = characterRenderer.color;

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
            for (int i = ZERO; i < library.spells.Count; i++)
            {
                GameManager.Instance.SLOTS[i].GetComponentInChildren<TextMeshProUGUI>().text = library.spells[i].name;
            }
        }
        catch { /*These hands!*/}

        ChangeMotion(AnimationMotion.Idle);
    }

    public void ChangeMotion(AnimationMotion newMotion)
    {
        characterAnimator.Play(motionNames[(int)newMotion]);
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
            characterRenderer.color = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
        }


    }

    #region Unique

    IEnumerator FocusCycle()
    {
        while (true)
        {
            for (int index = ZERO; index < autoOrbitObjs.Length; index++)
            {
                AutoOrbit orbitObj = autoOrbitObjs[index];
                orbitObj.ChangeRadius(IsOnFocus);
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
        MovementSpeed = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.SPEED);
        Evasivness = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.EVASIVENESS);
        FocusSpeed = (Evasivness * 10) / MovementSpeed;
        DashSpeed = MovementSpeed * Evasivness;
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
            if (!IsMagicActivelyUsed && GameManager.Instance.GetPlayerMagic() < GameManager.MaxMagic)
            {
                RecoverMagic(GameManager.MaxMagic * 0.0005f);
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
    #endregion

    public override void Shoot(string bulletName)
    {
        EventManager.Watch(recoil == false, () =>
        {
            emitter.SetPawnParent(this);
            emitter.SetBulletInitialSpeed(650);
            emitter.SpawnBullets(1, bulletName);
            AudioManager.Play("Shoot000", _oneShot: true);
            recoil = true;
        }, out recoil);
    }

    public override bool CheckIfMoving()
    {
        return IsMoving;
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

        EventManager.Watch(GameManager.Instance.GetPlayerMagic() > spell.magicConsumtion && library.spellInUse == null, () =>
        {
            library.spellInUse = spell;

            GameManager.Instance.DecrementMagic(spell.magicConsumtion);

            IsMagicActivelyUsed = true;

            //Increate pawn's priority!!!
            priority += spell.spellPriority;

            spell.Activate(this);
        }, out bool result);
    }

    public override void Wait(float _duration)
    {
        timer.StartTimer(2);
        returnVal = timer.SetFor(_duration, 2);

        EventManager.Watch(returnVal == true, () =>
        {
            recoil = false; timer.SetToZero(2, true);
        }, out returnVal);
    }

    public override void Blink(float _duration)
    {
        EventManager.Watch(hit, () =>
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

        }, out hit);
    }

    IEnumerator BlinkCycle(float _blinkRate)
    {
        while (true)
        {
            EventManager.Watch(hit, () =>
            {
                if (GameManager.Instance.GetPlayerLives() < 1 && !GameManager.Instance.NoDeaths)
                {
                    GameSceneManager.LoadScene("RECORDSANDHIGHSCORE", false);
                }

                if (isVisible)
                {
                    Color invisible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, (float)ZERO);
                    characterRenderer.color = invisible;
                    isVisible = false;
                }
                else
                {
                    Color visible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
                    characterRenderer.color = visible;
                    isVisible = true;
                }

            }, 
            out hit
            );

            yield return hit ? new WaitForSecondsRealtime(_blinkRate) : null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        GetOrignatedSpawnPoint objectOrigin = other.GetComponent<GetOrignatedSpawnPoint>();

        EventManager.Watch(objectOrigin != null && objectOrigin.pawn != this, () =>
        {
            if (hit == false)
            {
                hit = true;
                GameManager.Instance.DecrementLives();
            }
        },
        out bool result
        );
    }

    public override void Foward()
    {
        Steady();
        move.y++;
        Move();
    }
    public override void Back()
    {
        Steady();
        move.y--;
        Move();
    }

    public override void Left()
    {
        Steady();
        move.x--;
        Move();
    }

    public override void Right()
    {
        Steady();
        move.x++;
        Move();
    }

    void Move()
    {
        transform.Translate(move.normalized * (IsOnFocus ? FocusSpeed : MovementSpeed) * Time.deltaTime, Space.Self);
    }

    public void Steady()
    {
        move = Vector3.zero;
    }
}