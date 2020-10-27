﻿using System.Collections.Generic;
using UnityEngine;

public class LinearEmitter : Emitter
{
    void OnEnable()
    {
        if (GetComponent<PawnOwner>() != null)
        {
            ParentPawn = GetComponent<PawnOwner>().GetOwner();

            initialPosition = gameObject.transform.position;
        }
    }

    protected override void Start()
    {
        originObject = gameObject;
        existingProjectiles = new List<Projectile>();
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector = transform.up;
        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out Projectile projectile);

        projectile.AssignEmitter(this);
        projectile.SetCaster(ParentPawn);

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = projectile.GetRigidbody2D();

            tmpObj.transform.position = transform.position;
            tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, projectileBaseAngle);

            //Check for attached emitters
            if (attachedEmitters != null)
                foreach (Emitter emitter in attachedEmitters)
                {
                    emitter.SetPawnParent(ParentPawn);
                    emitter.SetBulletInitialSpeed(bulletInitialSpeed);
                    emitter.SpawnBullets(_numberOfProjectiles, bulletMember);
                }

            rigidbody.AddForce(targetVector * bulletInitialSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
