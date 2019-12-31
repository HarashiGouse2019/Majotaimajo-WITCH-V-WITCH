using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : Pawn
{

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
            GameManager.Instance.DecrementProgress(0.05f);
            GameManager.Instance.timesHit++;
            GameManager.Instance.AddToScore((10 * GameManager.Instance.timesHit) + 1);
            other.gameObject.SetActive(false);
        }
    }
}