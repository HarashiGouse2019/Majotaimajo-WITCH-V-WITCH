using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : MonoBehaviour
{
    DanmakuSequencer sequencer;
    SpellLibrary library;
    void Start()
    {
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();
    }
    public void ActivateSpell(string _name)
    {
        GameManager manager = GameManager.Instance;

        Spell spell = library.FindSpell(_name);

        //We give all values to our Sequencer
        sequencer.stepSpeed = spell.stepSpeed;

        //We have to loop each routine, and add them the list
        for (int routinePos = 0; routinePos < spell.routine.Count; routinePos++)
        {
            sequencer.routine.Add(spell.routine[routinePos]);
        }

        //And then we check if we enable looping
        sequencer.enableSequenceLooping = spell.enableSequenceLooping;

        //Now that all value have passed in, we enable
        sequencer.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D bullets)
    {
        if (bullets.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint.name == "Raven_Obj")
        {
            GameManager.Instance.DecrementProgress(0.05f);
            GameManager.Instance.timesHit++;
            GameManager.Instance.AddToScore((10 * GameManager.Instance.timesHit) + 1);
            Destroy(bullets.gameObject);
        }
    }
}