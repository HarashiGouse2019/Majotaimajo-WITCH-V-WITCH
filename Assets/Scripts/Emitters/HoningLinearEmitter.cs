using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Extensions;

public class HoningLinearEmitter : Emitter
{
    #region Public Members
    [Header("Origin and Target")]
    public ITargetable targetableObj;

    //Homing Detection Range (how far you can detect a target)
    public float detectionRange = 1f;
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
    }

    IEnumerator StartPointUpdateCycle(float delta = 0)
    {
        while (true)
        {
            UpdateStartPoint();
            yield return new WaitForSeconds(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector;

        if (targetableObj != null)
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
