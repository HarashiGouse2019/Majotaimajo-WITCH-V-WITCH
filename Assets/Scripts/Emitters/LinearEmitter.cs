using System.Collections.Generic;
using UnityEngine;

public class LinearEmitter : Emitter
{
    [SerializeField]
    float projectileAngle = 0;
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
        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out GetOrignatedSpawnPoint spawnPoint);
        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = tmpObj.GetComponent<Rigidbody2D>();

            tmpObj.transform.position = transform.position;
            tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, projectileAngle);

            //Check for attached emitters
            if (attachedEmitters != null)
                foreach (Emitter emitter in attachedEmitters)
                {
                    emitter.SetPawnParent(pawnOriginObject);
                    emitter.SetBulletInitialSpeed(bulletInitialSpeed);
                    emitter.SpawnBullets(_numberOfProjectiles, bulletMember);
                }

            spawnPoint.originatedSpawnPoint = originObject;
            rigidbody.AddForce(targetVector * bulletInitialSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
