using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Extensions;
using System.Threading.Tasks;

public class HoningLinearEmitter : Emitter
{
    #region Public Members
    [Header("Origin and Target")]
    public EnemyPawn mainTargetObj;

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

    private IEnumerator SearchForTargetCycle()
    {
        while (true)
        {
            DetectDistanceFromTarget();

            yield return null;
        }
    }

    private void DetectDistanceFromTarget()
    {
        foreach (EnemyPawn target in ESSequenceScript.Enemies)
        {
            distance = (target.targetTransform.position - transform.position).magnitude;
            if (distance <= detectionRange)
            {
                mainTargetObj = target;
                return;
            }
        }
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector;

        if (mainTargetObj != null && distance <= detectionRange)
            targetVector = (mainTargetObj.targetTransform.position - originObject.transform.position).normalized;
        else
            targetVector = Vector2.up;

        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out Projectile projectile);

        projectile.AssignEmitter(this);

        projectile.SetCaster(ParentPawn);

        float angle = 0f;
        if (mainTargetObj != null)
            angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = projectile.GetRigidbody2D();

            projectile.transform.position = transform.position;
            projectile.transform.rotation = (mainTargetObj != null) ?
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
