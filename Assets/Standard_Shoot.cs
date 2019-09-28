using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;
using Alarm;

using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
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
    }

    void FixedUpdate()
    {
        UpdateStartPoint();
    }

    public override void SpawnBullets(int _numberOfProjectiles, int _index = 0)
    {

   
        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            Vector3 targetVector = (target.position - origin.transform.position).normalized;
            GameObject tmpObj = ObjectPooler.Instance.GetObject("bullet0");
            if (tmpObj != null)
                tmpObj.SetActive(true);

            tmpObj.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint = origin;
            existingProjectiles.Add(tmpObj);
            
            //This is what I wanted
            tmpObj.transform.rotation = Quaternion.FromToRotation(target.position, transform.position) ;

            tmpObj.GetComponent<Rigidbody2D>().AddForce(targetVector * speed * Time.fixedDeltaTime);
        }
    }
    public override void Remove(GameObject obj)
    {
        existingProjectiles.Remove(obj);
    }

    public override void UpdateStartPoint()
    {
        startPoint = origin.transform.position;
    }
}
