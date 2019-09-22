using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class PlayerPawn : MonoBehaviour
{
    #region Public Members
    //Our movment speeds
    public float movementSpeed;
    public float rotationSpeed;

    public Transform originOfRotation;

    public float radius = 6f;
    readonly public float radiusSpeed = 5f;
    public bool isMoving;

    #endregion


    #region Private Members
    //We'll need our Transform
    Transform pawnTransform;

    //We'll add the important stuff
    Vector2 startPoint;
    public float g_angle = 0f;

    bool recoil = false;
    bool returnVal;
    
    Timer timer = new Timer(1, true);
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pawnTransform = GetComponent<Transform>();
        transform.position = (transform.position - originOfRotation.position).normalized * radius + originOfRotation.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (recoil == true) Wait(0.05f);
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing
    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    public void MoveInCircle(float _speed)
    {

        transform.RotateAround(originOfRotation.position, Vector3.back, _speed * Time.deltaTime);
        Vector2 desiredPosition = (transform.position - originOfRotation.position).normalized * radius + originOfRotation.position;
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

        Quaternion zLock = transform.rotation;
        zLock.eulerAngles = new Vector3(zLock.eulerAngles.x, zLock.eulerAngles.y, 0);
        transform.rotation = zLock;

        g_angle += -(_speed * Time.deltaTime);

    }

    public void MoveOnDiameter(float _speed, Transform _target)
    {
        //This will calculate the vector between Raven and Luu, and will move on that normalized vector
        Vector2 playerToLuu = (_target.position - pawnTransform.position).normalized;
        gameObject.transform.Translate(playerToLuu * _speed * Time.deltaTime);
        radius = radius / _speed;
    }

    public void Shoot(int _index)
    {
        if (recoil == false)
        {
            Standard_Shoot.Instance = GetComponent<Standard_Shoot>();
            Standard_Shoot.Instance.SpawnBullets(1, _index);
            recoil = true;
        }
    }

    public bool CheckIfMoving()
    {
        return isMoving;
    }

    void Wait(float _duration)
    {
        timer.StartTimer(0);
        returnVal = timer.SetFor(_duration, 0);
        if (returnVal == true) recoil = false;
    }
}
