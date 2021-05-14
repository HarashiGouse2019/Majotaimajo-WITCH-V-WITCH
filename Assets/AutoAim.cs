using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class AutoAim : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)]
    private float rotationSpeed = 0.1f;

    [SerializeField]
    private float detectionRange = 100f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float defaultZRotation;

    private void Awake()
    {
        Debug.Log(defaultZRotation);
    }

    private void OnEnable()
    {
        
        AutoAimCycle().Start();
    }

    private void OnDisable()
    {
        AutoAimCycle().Stop();
    }

    IEnumerator AutoAimCycle()
    {

        bool nearbyEnemyFound;
        while (true)
        {
            nearbyEnemyFound = false;

            //Loop through existing enemies, and lock onto the closest one.
            for (int i = 0; i < ESSequenceScript.Enemies.Count; i++)
            {
                EnemyPawn currentEnemy = ESSequenceScript.Enemies[i];

                float distance = (currentEnemy.transform.position - transform.position).sqrMagnitude;

                if (distance <= detectionRange)
                {
                    nearbyEnemyFound = true;
                    target = currentEnemy.targetTransform;
                    LookAtTarget(currentEnemy.transform.position);
                }
            }

            //Evaluation: If current cycle finds no enemies nearby, reset to default rotation
            if (!nearbyEnemyFound)
            {
                target = null;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, defaultZRotation));
            }

            yield return null;
        }
    }

    void LookAtTarget(Vector3 target)
    {
        target.x -= transform.position.x;
        target.y -= transform.position.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }
}
