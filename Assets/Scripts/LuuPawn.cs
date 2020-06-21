using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class LuuPawn : Pawn, IBossEntity
{
    public int UniqueIdentifier { get; set; } = 1;
    public float BossCurrentHealth { get; set; } = 0f;
    public float BossMaxHealth { get; set; } = 1000f;
    public float CurrentPatience { get; set; } = 0f;
    public float MaxPatience { get; set; } = 5000f;
    public float PatienceDepletionRate { get; set; } = 2f;
    public int HPLayer { get; set; } = 4;

    //State that the boss is active
    public bool IsActive { get; set; } = false;

    public bool HasLostPatience { get; set; } = false;

    public bool HasHealthLowered { get; set; } = false;

    public bool IsDefeated { get; set; } = false;

    //How many seconds it takes to deplete patience
    const float DEPLETEION_PER_SEC = 0.001f;

    const float ZERO = 0f;

    LuuEventTimeline LuuEventTimeline;

    void Awake() {
        LuuEventTimeline = gameObject.AddComponent<LuuEventTimeline>();
        LuuEventTimeline.SetupEvents();
    }

    void Start()
    {
        LuuEventTimeline.Initialize(this);

        priority = basePriority;

        //Initialize Patience Cycle
        StartCoroutine(PatienceCycle());

        //Setting Max Values
        SetMaxHealthValue((int)BossMaxHealth);
        SetMaxPatienceValue((int)MaxPatience);

        //Setting Current Values to Max
        SetHealthValue(BossMaxHealth);
        SetPatienceValue(MaxPatience);

        //Set Layer
        SetHealthLayer(HPLayer);

        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Get the information that tells  where the bullet came from
        GetOrignatedSpawnPoint objectOrigin = other.GetComponent<GetOrignatedSpawnPoint>();

        //If this bullet did not come from Luu herself, she'll take damage
        if (objectOrigin != null && objectOrigin.originatedSpawnPoint.name == "Raven_Obj" && !IsDefeated)
        {
            try
            {
                PlayerPawn player = objectOrigin.pawn as PlayerPawn;
                Stats playerStats = player.GetStats();

                //If no patience has been lost, decrease that
                if (!HasLostPatience)
                    SetPatienceValue(-playerStats.GetCurrentAttributeValue(Stats.StatsAttribute.ANNOYANCE), true);

                //Otherwise, she has lost her patience, which leave her vulnerable
                else
                    SetHealthValue(-playerStats.GetCurrentAttributeValue(Stats.StatsAttribute.POWER), true);

                //Keep track of how many times you've hit her without losing a life
                GameManager.Instance.timesHit++;

                //Add it to your current score
                GameManager.Instance.AddToScore((10 * GameManager.Instance.timesHit) + 1);

                //Set the projectile object to false
                other.gameObject.SetActive(false);
            }
            catch { return; }
        }
    }

    /// <summary>
    /// Activate a spell from the spell library by name
    /// </summary>
    /// <param name="_name"></param>
    public override void ActivateSpell(string _name)
    {
        //Find a spell in the library by name
        Spell spell = library.FindSpell(_name);

        //If a spell is not in used, use it
        if (SpellLibrary.library.spellInUse == null)
        {
            library.spellInUse = spell;

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
    }

    /// <summary>
    /// HasLostPatience will be true if patience value is 0.
    /// </summary>
    /// <returns></returns>
    void CheckPatienceStatus()
    {
        HasLostPatience = (CurrentPatience <= ZERO);
    }

    /// <summary>
    /// Check if health is zero. If it is, isHealthLowered is set to true;
    /// </summary>
    void CheckHealthStatus()
    {
        HasHealthLowered = (BossCurrentHealth <= ZERO);
    }

    /// <summary>
    /// Checks if the boss is defeated, IsDefeated will be set to true if
    /// health is lowered and there's no patience
    /// </summary>
    void CheckDefeated()
    {
        IsDefeated = (HPLayer <= ZERO + 1 && HasHealthLowered && HasLostPatience);
    }

    /// <summary>
    /// Set the total amount of health layers
    /// </summary>
    /// <param name="value"></param>
    public void SetHealthLayer(int value)
    {
        HPLayer = value;

        //Update UI
        UI_BossHealth.SetLayerCount((uint)value);
    }

    /// <summary>
    /// Set the current patience value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="addTo"></param>
    public void SetPatienceValue(float value, bool addTo = false)
    {
        //Set CurrentPatience based on addTo boolean
        CurrentPatience = addTo ? CurrentPatience += value : CurrentPatience = value;

        //Check Patience Status
        CheckPatienceStatus();

        //Update UI
        UI_PatienceMeter.SetValue(CurrentPatience);
    }

    /// <summary>
    /// Set the max patience value
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxPatienceValue(int value)
    {
        MaxPatience = value;

        //Check Patience Status
        CheckPatienceStatus();

        //Update UI
        UI_PatienceMeter.SetMaxValue(value);
    }

    /// <summary>
    /// Set the health value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="addTo"></param>
    public void SetHealthValue(float value, bool addTo = false)
    {
        BossCurrentHealth = addTo ? BossCurrentHealth += value : BossCurrentHealth = value;

        //Check Health Status
        CheckHealthStatus();

        //Update UI
        UI_BossHealth.SetValue(BossCurrentHealth);
    }

    /// <summary>
    /// Set the max health value
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxHealthValue(int value)
    {
        BossMaxHealth = value;

        //Check Health Status
        CheckHealthStatus();

        //Update UI
        UI_BossHealth.SetMaxValue(value);
    }

    public void SetBasePriority(int value)
    {
        basePriority = (uint)value;
    }

    /// <summary>
    /// Set pawn to active (not the gameObject)
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        IsActive = active;
    }

    /// <summary>
    /// Decreases the number you see on the right side of the boss' HP by 1
    /// </summary>
    public void DecrementHPLayer()
    {
        HPLayer--;

        //Update UI
        UI_BossHealth.SetLayerCount((uint)HPLayer);
    }

    /// <summary>
    /// The routine where Luu loses her patience
    /// </summary>
    /// <returns></returns>
    IEnumerator PatienceCycle()
    {
        while (!IsDefeated)
        {
            try
            {
                //We'll decrement with the depletion rate
                if(IsActive) SetPatienceValue(-PatienceDepletionRate, true);
            }
            catch(IOException e)
            {
                Debug.LogException(e);
            }
            yield return new WaitForSecondsRealtime(DEPLETEION_PER_SEC);
        }
    }

    /// <summary>
    /// On the start of this pawn beginning to fight
    /// </summary>
    public void OnInitialized()
    {
        IsActive = true;
        ActivateSpell("Sakura Burst");
    }

    /// <summary>
    /// (Temporary) Resets Patience and HP to MaxValues
    /// </summary>
    public void ResetValues()
    {
        //As we reset, check if she has been defeated
        CheckDefeated();

        if (!IsDefeated)
        {
            SetPatienceValue(MaxPatience);
            SetHealthValue(BossMaxHealth);

            //Decrement Layer
            DecrementHPLayer();
        }
    }
}