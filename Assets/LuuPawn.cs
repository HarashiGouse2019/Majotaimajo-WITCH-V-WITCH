using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuuPawn : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        switch(other.name)
        {
            case "testBullet1(Clone)":
                GameManager.Instance.DecrementProgress(0.05f);
                GameManager.Instance.timesHit++;
                GameManager.Instance.AddToScore((30 * GameManager.Instance.timesHit) + 1);
                Destroy(other.gameObject);
                break;

        }
    }
}
