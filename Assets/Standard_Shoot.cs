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
public class Standard_Shoot : MonoBehaviour
{
    public static Standard_Shoot Instance;

    #region Public Members

    public float speed;

    //Statistics
    [HideInInspector] public List<GameObject> existingProjectiles;

    [Header("Prefabs")]
    public List<GameObject> bullet;
    public int bulletIndex;

    [Header("Origin and Target")]
    public Transform origin;
    public Transform target;
    #endregion

    #region Private Members
    private const float radius = 1f;

    private Vector3 startPoint;

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

    public void SpawnBullets(int _numberOfProjectiles, int _index = 0)
    {

   
        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            Vector3 targetVector = (target.position - origin.transform.position).normalized;
            GameObject tmpObj = Instantiate(bullet[_index], startPoint, transform.rotation);

            existingProjectiles.Add(tmpObj);


            tmpObj.transform.eulerAngles = new Vector3(0, 0, GetComponent<PlayerPawn>().g_angle);
            tmpObj.GetComponent<Rigidbody2D>().AddForce(targetVector * speed * Time.fixedDeltaTime);
        }
    }
    public void Remove(GameObject obj)
    {
        existingProjectiles.Remove(obj);
    }

    public void UpdateStartPoint()
    {
        startPoint = origin.transform.position;
    }
}
