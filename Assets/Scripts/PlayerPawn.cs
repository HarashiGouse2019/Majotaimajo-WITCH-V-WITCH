using System.Collections;
using UnityEngine;
using BulletPro;
using Extensions;

using static InGameBounds;

public abstract class PlayerPawn : Pawn
{
    protected enum AnimationMotion
    {
        Idle = 0,
        MoveLeft = 1,
        MoveRight = 2
    }

    public new static PlayerPawn Instance;

    public EnemyPawn target;

    public Animator characterAnimator;

    public PlayerController controller;

    //Since you are the player, you'll have an object that handles the shooting for you.
    //For example, Maple's Star or Raven's Crystal Circle.
    //It will be a prefab
    public GameObject pawnEmitterPrefab;

    public float MovementSpeed { get; set; }
    public float Evasiveness { get; set; }

    public float FocusSpeed { get; set; }
    public float DashSpeed { get; set; }
    public bool IsMoving { get; set; }

    public bool IsMagicActivelyUsed { get; set; } = false;
    public bool IsOnFocus { get; set; } = false;
    float AddedSpeed { get; set; }

    protected const int ZERO = 0;
    protected const int DOUBLE = 2;

    //Player Stats
    Stats PlayerStats;

    string[] motionNames =
    {
        "Idle",
        "MoveLeft",
        "MoveRight"
    };

    // Start is called before the first frame update
    protected virtual void Start()
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

        //Dash Mechanic Cycle Coroutine Start
        DashCycle(MovementSpeed / 100f).Start();

        ChangeMotion(AnimationMotion.Idle);
    }

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

    protected virtual void ChangeMotion(AnimationMotion newMotion)
    {
        if (characterAnimator != null)
            characterAnimator.Play(motionNames[(int)newMotion]);
    }

    /// <summary>
    /// Assign Game Values to PlayerPawn
    /// </summary>
    void AssignValuesFromStats()
    {
        basePriority = (uint)PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.BASEPRIORITY);
        MovementSpeed = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.SPEED);
        Evasiveness = PlayerStats.GetCurrentAttributeValue(Stats.StatsAttribute.EVASIVENESS);

        float statPercentage = Evasiveness / Stats.HIGH_RANK_VALUE;
        float reducePercentage = (Stats.SPEED_REDUCTION_CAP * statPercentage);

        FocusSpeed = MovementSpeed - (MovementSpeed * reducePercentage);
        DashSpeed = (IsOnFocus ? FocusSpeed * Evasiveness : MovementSpeed * Evasiveness) - DOUBLE;
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
            if (IsMagicActivelyUsed == false && GameManager.Instance.GetPlayerMagic() < GameManager.MaxMagic)
            {
                RecoverMagic(GameManager.MaxMagic * Stats.MAGIC_RECOVERY_RATE);
            }

            yield return new WaitForSeconds(0.025f);
        }
    }

    public override void Shoot()
    {
        for (int index = 0; index < emitterCollection.shotTypes.Length; index++)
        {
            if (PowerGradeSystem.CurrentLevel + 1 >= emitterCollection.shotTypes[index].emitter.enableAtPowerLevel)
                emitterCollection.shotTypes[index].emitter.Play();
            else
                emitterCollection.shotTypes[index].emitter.Stop();
        }
    }

    public override void CeaseShoot()
    {
        for (int index = 0; index < emitterCollection.shotTypes.Length; index++)
        {
            emitterCollection.shotTypes[index].emitter.Stop(PlayOptions.RootOnly);
        }
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
        Debug.Log("Activating Spell!!!");
        Spell spell = library.FindSpell(_name);

        EventManager.Watch(GameManager.Instance.GetPlayerMagic() > spell.magicConsumtion && library.spellInUse == null, () =>
        {
            library.spellInUse = spell;

            GameManager.Instance.DecrementMagic(spell.magicConsumtion);

            IsMagicActivelyUsed = true;

            //Increate pawn's priority!!!
            priority += spell.spellPriority;

            spell.Activate(this);
        }, null);
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
        if (move.x < 0 && transform.position.x - 0.05f < LeftBound.position.x) move.x = 0;
        if (move.x > 0 && transform.position.x + 0.05f > RightBound.position.x) move.x = 0;
        if (move.y < 0 && transform.position.y - 0.05f < BottomBound.position.y) move.y = 0;
        if (move.y > 0 && transform.position.y + 0.05f > TopBound.position.y) move.y = 0;

        if(controller.IsDashing)
            transform.Translate(move.normalized * AddedSpeed * Time.deltaTime, Space.Self);
        else
        transform.Translate(move.normalized * (IsOnFocus ? FocusSpeed : MovementSpeed) * Time.deltaTime, Space.Self);
    }

    public void Steady()
    {
        move = Vector3.zero;
    }

    protected virtual IEnumerator FocusCycle()
    {
        while (true)
        {


            yield return null;
        }
    }

    private IEnumerator DashCycle(float rate)
    {
        float interpolationVal = 0;

        while (true)
        {
            if (controller.IsDashing)
            {
                interpolationVal += IsOnFocus ? rate + (DOUBLE / 200f) : rate / DOUBLE;

                AddedSpeed = Mathf.SmoothStep(DashSpeed / 8f, IsOnFocus ? FocusSpeed / (DOUBLE*DOUBLE) : MovementSpeed / (DOUBLE * DOUBLE), interpolationVal);

                if (interpolationVal >= 0.99f)
                {
                    controller.IsDashing = false;
                    interpolationVal = ZERO;
                }

            }
            yield return new WaitForSeconds(0.2f * rate);
        }
    }
}
