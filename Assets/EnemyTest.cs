using System.Collections;
using UnityEngine;
using Extensions;

public class EnemyTest : MonoBehaviour, ITargetable
{
    public GameObject targetObj { get; set; }
    public Transform targetTransform { get; set; }
    public Vector3 targetPosition { get; set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        LifeCycle().Start();
    }
    IEnumerator LifeCycle()
    {
        while (true)
        {
            targetObj = gameObject;
            targetTransform = transform;
            targetPosition = targetTransform.localPosition;
            yield return null;
        }
    }
}
