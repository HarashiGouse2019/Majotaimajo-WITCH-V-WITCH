using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Extensions;

public class HoningLinearEmitter : Emitter
{
    #region Public Members
    [Header("Origin and Target")]
    public EnemyTest targetableObj;

    //Homing Detection Range (how far you can detect a target)
    public float detectionRange = 1f;

    private float distance;
    #endregion


    void OnEnable()
    {
        if (GetComponent<PawnOwner>() != null)
            ParentPawn = GetComponent<PawnOwner>().GetOwner();

        initialPosition = gameObject.transform.position;
    }

    protected override void Start()
    {
        PlayerPawn ppawn = (ParentPawn as PlayerPawn);
        targetableObj = ppawn.target;

        originObject = gameObject;
        existingProjectiles = new List<Projectile>();

        StartPointUpdateCycle().Start();
        SearchForTargetCycle().Start();
    }

    IEnumerator StartPointUpdateCycle(float delta = 0)
    {
        while (true)
        {
            UpdateStartPoint();
            yield return new WaitForSeconds(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }

    IEnumerator SearchForTargetCycle()
    {
        while (true)
        {
            Debug.Log("Hi!!!");
            DetectDistanceFromTarget();
            yield return null;
        }
    }

    void DetectDistanceFromTarget()
    {
        distance = (targetableObj.targetTransform.position - transform.position).magnitude;
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector;

        if (targetableObj != null && distance <= detectionRange)
            targetVector = (targetableObj.targetTransform.position - originObject.transform.position).normalized;
        else
            targetVector = Vector2.up;

        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out Projectile projectile);

        projectile.AssignEmitter(this);

        projectile.SetCaster(ParentPawn);

        float angle = 0f;
        if(targetableObj != null)
            angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = projectile.GetRigidbody2D();

            projectile.transform.position = transform.position;
            projectile.transform.rotation = (targetableObj != null) ? 
                Quaternion.Euler(0f, 0f, angle + 270f) :
                Quaternion.Euler(0f, 0f, projectileBaseAngle);

            rigidbody.AddForce(targetVector * bulletInitialSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
