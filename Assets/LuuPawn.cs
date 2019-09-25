using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : MonoBehaviour
{
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