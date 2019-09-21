using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;
using DanmakuPattern;
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
public class Shoot_Trig : MonoBehaviour
{
    public static Shoot_Trig shootTrig;

    #region Public Members
    [Header("Danmaku Config")]
    [Range(1, 10)] public int numberOfProjectiles = 1;
    public float speed;
    public float g_angle;
    public enum RotationType
    {

        NoRotation,
        ClockwiseI,
        ClockwiseII,
        ClockwiseIII,
        CounterClockwiseI = -1,
        CounterClockwiseII = -2,
        CounterClockwiseIII = -3
    };
    public enum DistributionType
    {
        Uniformed,
        Biformed,
        Scattered
    }

    public RotationType rotation;
    public DistributionType distribution;

    //Statistics
    [HideInInspector] public List<GameObject> existingProjectiles;
    [HideInInspector] public int steps;

    [Header("Looping")]
    public bool loop;
    [Range(0.05f, 1)] public float loopSpeed = 0.05f;

    [Header("Prefabs")]
    public List<GameObject> bullet;
    public int bulletIndex;

    [Header("Origin")]
    public GameObject origin;
    #endregion

    #region Private Members
    private const float radius = 1f;
    private int distStep = 0;

    private Vector3 startPoint;
    private Timer loopTimer;
    #endregion

    void OnEnable()
    {
        startPoint = gameObject.transform.position;
        origin = gameObject;
    }

    void Start()
    {
        loopTimer = new Timer(3); //Reintergrated timer!
        existingProjectiles = new List<GameObject>();
    }

    void FixedUpdate()
    {
        UpdateStartPoint();

        if (loop) Loop();

        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing
    }

    public void SpawnBullets(int _numberOfProjectiles)
    {
        float angleStep = 360f / _numberOfProjectiles;
        float angle = g_angle;

        AudioManager.audio.Play("Shoot000", 100f);

        switch (distribution)
        {
            case DistributionType.Uniformed:
                //Do Nothing
                break;

            case DistributionType.Biformed:
                if (distStep == 0)
                {
                    speed *= 2;
                    distStep++;
                }
                else if (distStep == 1)
                {
                    speed /= 2;
                    distStep--;
                }
                break;

            case DistributionType.Scattered:
                speed = Random.Range(20f, speed + 50);
                break;

            default:
                break;
        }


        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            float projectileAngleX = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180f) * radius;
            float projectileAngleY = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180f) * radius;

            Vector3 projectileVector = new Vector3(projectileAngleX, projectileAngleY, 0);
            Vector3 projectileMoveDir = (projectileVector - startPoint).normalized * speed;

            GameObject tmpObj = Instantiate(bullet[bulletIndex], startPoint, Quaternion.identity);

            existingProjectiles.Add(tmpObj);
            tmpObj.GetComponent<Rigidbody2D>().AddForce(new Vector3(projectileMoveDir.x, projectileMoveDir.y, 0) * Time.fixedDeltaTime);

            angle += angleStep;
        }
    }
    void Loop()
    {

        //Now... The moment of truth...
        Pattern simplePattern = new Simple_Pattern(numberOfProjectiles, loopSpeed, (g_angle += (int)rotation), loopTimer, loopTimer.GetSize(), 0, () => SpawnBullets(numberOfProjectiles));

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
