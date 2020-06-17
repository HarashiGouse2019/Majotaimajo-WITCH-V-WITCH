using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : Pawn, IBossEntity
{
    public float BossCurrentHealth { get; set; }
    public float BossMaxHealth { get; set; } = 100f;
    public float CurrentPatience { get; set; } = 500f;
    public float MaxPatience { get; set; }
    public float PatienceDepletionRate { get; set; }

    //How many seconds it takes to deplete patience
    const float DEPLETEION_PER_SEC = 0.001f; 

    void Start()
    {
        priority = basePriority;

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
            GameManager.Instance.DecrementPatience(0.05f);
            GameManager.Instance.timesHit++;
            GameManager.Instance.AddToScore((10 * GameManager.Instance.timesHit) + 1);
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set the total amount of phases that the boss can have
    /// </summary>
    /// <param name="value"></param>
    public void SetTotalPhases(int value)
    {

    }

    /// <summary>
    /// Set the amount of patience the boss has
    /// </summary>
    /// <param name="value"></param>
    public void SetPatienceValue(int value)
    {

    }

    /// <summary>
    /// Decreases the number you see on the right side of the boss' HP
    /// </summary>
    public void DecrementHPLayer()
    {

    }

    /// <summary>
    /// On the start of this pawn beginning to fight
    /// </summary>
    public void OnInitialized()
    {

    }
}