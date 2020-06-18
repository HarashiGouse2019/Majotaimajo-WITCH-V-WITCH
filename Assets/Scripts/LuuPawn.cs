using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuuPawn : PawnProgrammable, IBossEntity
{
    public int UniqueIdentifier { get; set; } = 1;
    public float BossCurrentHealth { get; set; } = 0f;
    public float BossMaxHealth { get; set; } = 100f;
    public float CurrentPatience { get; set; } = 0f;
    public float MaxPatience { get; set; } = 500f;
    public float PatienceDepletionRate { get; set; } = 0.02f;
    public int HPLayer { get; set; } = 4;


    //State that the boss is active
    public bool IsActive { get; set; } = false;

    public bool HasLostPatience { get; set; } = false;

    //How many seconds it takes to deplete patience
    const float DEPLETEION_PER_SEC = 0.001f;


    void Start()
    {
        priority = basePriority;

        //Initialize Patience Cycle
        StartCoroutine(PatienceCycle());

        //Setting Max Values
        SetMaxHealthValue((int)BossMaxHealth);
        SetMaxPatienceValue((int)MaxPatience);

        //Setting Current Values to Max
        SetHealthValue(BossMaxHealth);
        SetPatienceValue(MaxPatience);

        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();
    }

    public override void ActivateSpell(string _name)
    {
        Spell spell = library.FindSpell(_name);

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

    private void OnTriggerStay2D(Collider2D other)
    {
        GetOrignatedSpawnPoint objectOrigin = other.GetComponent<GetOrignatedSpawnPoint>();
        if (objectOrigin != null && objectOrigin.originatedSpawnPoint.name != "Luu_Obj")
        {
            if (!HasLostPatience)
                SetPatienceValue(-0.05f, true);
            else
                SetHealthValue(-0.05f, true);

            GameManager.Instance.timesHit++;
            GameManager.Instance.AddToScore((10 * GameManager.Instance.timesHit) + 1);
            other.gameObject.SetActive(false);
        }
    }

    public void SetTotalPhases(int value)
    {
        HPLayer = value;
        UI_BossHealth.SetLayerCount((uint)value);
    }

    public void SetPatienceValue(float value, bool addTo = false)
    {
        CurrentPatience = addTo ? CurrentPatience += value : CurrentPatience = value;

        UI_PatienceMeter.SetValue(CurrentPatience);
    }

    public void SetMaxPatienceValue(int value)
    {
        MaxPatience = value;
        UI_PatienceMeter.SetMaxValue(value);
    }

    public void SetHealthValue(float value, bool addTo = false)
    {
        BossCurrentHealth = addTo ? BossCurrentHealth += value : BossCurrentHealth = value;

        UI_BossHealth.SetValue(BossCurrentHealth);
    }

    public void SetMaxHealthValue(int value)
    {
        BossMaxHealth = value;
        UI_BossHealth.SetMaxValue(value);
    }

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
        UI_BossHealth.SetLayerCount((uint)HPLayer);
    }

    /// <summary>
    /// The routine where Luu loses her patience
    /// </summary>
    /// <returns></returns>
    IEnumerator PatienceCycle()
    {
        while (true)
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
}