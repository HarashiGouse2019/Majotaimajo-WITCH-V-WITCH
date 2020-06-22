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
public class Shoot_Trig : MonoBehaviour
{
    public static Shoot_Trig Instance;

    #region Public Members
    [Header("Danmaku Config")]
    [Range(1, 10)] public int numberOfProjectiles = 1;
    public float speed;
    public int incrementVal;
    public int speedLimit;
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
        UniformedIncrement,
        BiformedIncrement,
        Scattered
    }

    public RotationType rotation;
    public DistributionType distribution;

    public float rotationFocus;
    public float rotationFocusIncrementVal;
    public float rotationFocusLimit;

    public float rotationIntensity;
    public float rotationIntensityIncrementVal;
    public float rotationIntensityLimit;

    //Statistics
    [HideInInspector] public List<GameObject> existingProjectiles;
    [HideInInspector] public int steps;

    [Header("Looping")]
    public bool loop;
    [Range(0.05f, 1)] public float loopSpeed = 0.05f;

    [Header("Prefabs")]
    public List<GameObject> bullet = new List<GameObject>();
    public string bulletMember;

    [Header("Origin")]
    public GameObject origin;
    #endregion

    #region Private Members
    private const float radius = 1f;
    private int distStep = 0;

    protected Vector3 startPoint;
    private Timer loopTimer;
    private AudioClip sound;
    protected ObjectPooler pool;
    private Pawn pawnOrigin;
    #endregion

    void OnEnable()
    {
        startPoint = gameObject.transform.position;
    }

    void Start()
    {
        Instance = this;
        loopTimer = new Timer(3); //Reintergrated timer!
        existingProjectiles = new List<GameObject>();
        origin = gameObject;
        pawnOrigin = GetComponent<Pawn>();
        pool = FindObjectOfType<ObjectPooler>();
    }

    void FixedUpdate()
    {
        UpdateStartPoint();

        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing

        if (loop)
        {
            loopTimer.StartTimer(0);
            Loop();
        }
        else
        {
            loopTimer.SetToZero(0, true);
        }
    }

    public virtual void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        //Update rotation focus
        if(rotationFocus < rotationFocusLimit)
            rotationFocus += rotationFocusIncrementVal;

        //Update rotation intensity
        if(rotationIntensity < rotationIntensityLimit)
            rotationIntensity += rotationIntensityIncrementVal;

        //Assing those values to our basic algorithm

        float angleStep = 360f / (_numberOfProjectiles * rotationFocus); //n scales the area in which bullets are spawn
                                                             //You want to concentrate only on one side, but spread them, n is the one.
        float angle = g_angle * rotationIntensity;

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

            case DistributionType.UniformedIncrement:
                if(speed<speedLimit) speed += incrementVal;
                break;

            case DistributionType.BiformedIncrement:
                if (distStep == 0)
                {
                    speed *= 2 + (speed < speedLimit ? incrementVal : 0);
                    distStep++;
                }
                else if (distStep == 1)
                {
                    speed /= 2 + (speed < speedLimit ? incrementVal : 0);
                    distStep--;
                }
                break;

            case DistributionType.Scattered:
                speed = Random.Range(speed, speedLimit + 500);
                break;

            default:
                break;
        }


        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            float projectileAngleX = startPoint.x  + (Mathf.Sin((angle * Mathf.PI) / 180f)) * radius;
            float projectileAngleY = startPoint.y + (Mathf.Cos((angle * Mathf.PI) / 180f)) * radius;

            Vector3 projectileVector = new Vector3(projectileAngleX, projectileAngleY, 0);
            Vector3 projectileMoveDir = (projectileVector - startPoint).normalized * speed;

            //GameObject tmpObj = Instantiate(bullet[_index], startPoint, Quaternion.Euler(0f, 0f, -angle));
            GameObject tmpObj = pool.GetMember(bulletMember);
            if (!tmpObj.activeInHierarchy)
            {
                tmpObj.SetActive(true);
                tmpObj.transform.position = startPoint;
                tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

                //Assign projectile priority from origin
                tmpObj.GetComponent<GetOrignatedSpawnPoint>().priority = pawnOrigin.priority;

                //From here, we tell our temporary object where it came from
                tmpObj.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint = origin;

                tmpObj.GetComponent<Rigidbody2D>().AddForce(new Vector3(projectileMoveDir.x, projectileMoveDir.y, 0) * Time.fixedDeltaTime);
            } else
            {
                Debug.LogWarning("For some reason, this object is inactive.");
            }
            angle += angleStep;
        }
    }

    void Loop()
    {

        if (loopTimer.SetFor(loopSpeed, 0))
        {
            g_angle += (int)rotation;
            SpawnBullets(numberOfProjectiles, bulletMember);
        }
    }

    public virtual void Remove(GameObject obj)
    {
        existingProjectiles.Remove(obj);
    }

    public virtual void UpdateStartPoint()
    {
        startPoint = origin.transform.position;
    }
}
