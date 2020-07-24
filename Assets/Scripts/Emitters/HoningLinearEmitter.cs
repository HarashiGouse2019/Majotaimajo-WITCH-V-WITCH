using System.Collections.Generic;
using UnityEngine;
//This is a set up for the near future
//This will be important for creating this game
//That has existing mechanics such as these
//I mean a DAN-MA-KU! DAMMAKA
//
public class HoningLinearEmitter : Emitter
{
    #region Public Members
    [Header("Origin and Target")]
    public Transform target;
    #endregion


    void OnEnable()
    {
        initialPosition = gameObject.transform.position;
    }

    protected override void Start()
    {
        originObject = gameObject;
        existingProjectiles = new List<GameObject>();
    }

    void FixedUpdate()
    {
        UpdateStartPoint();
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector = (target.position - originObject.transform.position).normalized;

        GameObject tmpObj = ObjectPooler.GetMember(bulletMember, out GetOrignatedSpawnPoint spawnPoint);

        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            Rigidbody2D rigidbody = tmpObj.GetComponent<Rigidbody2D>();

            tmpObj.transform.position = transform.position;
            tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, angle + 270f);

            spawnPoint.originatedSpawnPoint = originObject;

            rigidbody.AddForce(targetVector * bulletInitialSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
