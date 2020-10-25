using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Extensions;

public class HoningLinearEmitter : Emitter
{
    #region Public Members
    [Header("Origin and Target")]
    public Transform target;
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

        if (target != null)
            targetVector = (target.position - originObject.transform.position).normalized;
        else
            targetVector = transform.up;

        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out Projectile projectile);

        projectile.AssignEmitter(this);

        projectile.SetCaster(ParentPawn);

        float angle = 0f;
        if(target != null)
            angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = tmpObj.GetComponent<Rigidbody2D>();

            tmpObj.transform.position = transform.position;
            tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, angle + 270f);;

            rigidbody.AddForce(targetVector * bulletInitialSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
