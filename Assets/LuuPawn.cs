using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : Pawn
{

    void Start()
    {
        sequencer = GetComponent<DanmakuSequencer>();
        library = GetComponent<SpellLibrary>();
    }
    public override void ActivateSpell(string _name)
    {
        base.ActivateSpell(_name);
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