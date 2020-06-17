using UnityEngine;
using TMPro;
public class PlayerPawn : Pawn
{
    public static new PlayerPawn Instance;

    public PlayerController controller;

    public float movementSpeed;
    public float rotationSpeed;
    public float maxSpeed;

    public Transform originOfRotation;

    public float radius = 6f;
    readonly public float radiusSpeed = 5f;
    public bool isMoving;

    private void Awake()
    {
        if(Instance == null)
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
        priority = basePriority;

        //Get Components
        srenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();

        isVisible = srenderer.isVisible;
        xScale = transform.localScale;
        xScaleVal = xScale.x;
        srendererColor = srenderer.color;

        //Read your SpellLibrary, and override GameUi spell text
        for (int i = 0; i < library.spells.Length; i++)
        {
            GameManager.Instance.SLOTS[i].GetComponentInChildren<TextMeshProUGUI>().text = library.spells[i].name;
        }
    }

    private void Update()
    {
        if (recoil == true) Wait(0.05f);
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing

        //For our blinking effect
        if (hit == true)
        {
            Blink(0.15f, 5f);
        }
        else
        {
            isVisible = true;
            srenderer.color = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
        }
    }

    public override void Shoot(string bulletName)
    {
        if (recoil == false)
        {
            Standard_Shoot.Instance = GetComponent<Standard_Shoot>();
            Standard_Shoot.Instance.SpawnBullets(1, bulletName);
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
        Spell spell = library.FindSpell(_name);

        GameManager.Instance.ActivateSlot((int)library.GetSpellIndex(_name), true);

        if (GameManager.Instance.GetPlayerMagic() > spell.magicConsumtion && SpellLibrary.library.spellInUse == null)
        {
            library.spellInUse = spell;

            GameManager.Instance.DecrementMagic(spell.magicConsumtion);

            //Increate pawn's priority!!!
            priority += spell.spellPriority;

            //We give all values to our Sequencer
            sequencer.stepSpeed = spell.stepSpeed;

            //We have to loop each routine, and add them the list
            for (int routinePos = 0; routinePos < spell.routine.Count; routinePos++)
            {
                sequencer.routine.Add(spell.routine[routinePos]);

                //And then we check if we enable looping
                if (sequencer.allowOverride) sequencer.enableSequenceLooping = spell.enableSequenceLooping;
            }

            //Now that all value have passed in, we enable
            sequencer.enabled = true;
        }
        base.ActivateSpell(_name);
    }

    public override void Wait(float _duration)
    {
        timer.StartTimer(2);
        returnVal = timer.SetFor(_duration, 2);
        if (returnVal == true) { recoil = false; timer.SetToZero(2, true); }
    }

    public override void Blink(float _blinkRate, float _duration)
    {
        if (hit == true)
        {
            if (GameManager.Instance.GetPlayerLives() < 1)
            {
                Application.Quit();
            }

            //Timer for blinking rate
            timer.StartTimer(0);

            //Timer for duration
            timer.StartTimer(1);

            //Since there's a timer in Pawn, and I've initialized 12, I'm going to use alarm 6
            //We'll pass the blink rate to our SetFor method.
            returnVal = timer.SetFor(_duration, 1, true);
            if (timer.SetFor(_blinkRate, 0) && isVisible)
            {
                Color invisible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 0f);
                srenderer.color = invisible;
                isVisible = false;
            }
            else if (timer.SetFor(_blinkRate, 0) && isVisible == false)
            {
                Color visible = new Color(srendererColor.r, srendererColor.g, srendererColor.b, 255f);
                srenderer.color = visible;
                isVisible = true;
            }

            if (returnVal)
            {
                hit = false;
                timer.SetToZero(1, true);
                timer.SetToZero(0, true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        GetOrignatedSpawnPoint objectOrigin = other.GetComponent<GetOrignatedSpawnPoint>();

        if (objectOrigin != null && objectOrigin.originatedSpawnPoint.name != "Raven_Obj")
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
}