using System.Collections.Generic;
using UnityEngine;
//This is a set up for the near future
//This will be important for creating this game
//That has existing mechanics such as these
//I mean a DAN-MA-KU! DAMMAKA
//
//
//
public class Standard_Shoot : Shoot_Trig
{
    public new static Standard_Shoot Instance;

    #region Public Members
    [Header("Origin and Target")]
    public Transform target;
    #endregion

    #region Private Members
    private const float radius = 1f;

    #endregion

    void OnEnable()
    {
        startPoint = gameObject.transform.position;
    }

    void Start()
    {
        Instance = this;
        existingProjectiles = new List<GameObject>();
        pool = FindObjectOfType<ObjectPooler>();
    }

    void FixedUpdate()
    {
        UpdateStartPoint();
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        Vector3 targetVector = (target.position - origin.transform.position).normalized;
        GameObject tmpObj = ObjectPooler.GetMember(bulletMember);
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        if (!tmpObj.activeInHierarchy)
        {
            tmpObj.SetActive(true);

            
            GetOrignatedSpawnPoint spawnPoint = tmpObj.GetComponent<GetOrignatedSpawnPoint>();

            Rigidbody2D rigidbody = tmpObj.GetComponent<Rigidbody2D>();

            tmpObj.transform.position = transform.position;
            tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, angle + 270f);

            spawnPoint.originatedSpawnPoint = origin;

            rigidbody.AddForce(targetVector * speed * Time.fixedDeltaTime);
        }
    }

    public override void UpdateStartPoint()
    {
        startPoint = origin.transform.position;
    }
}
