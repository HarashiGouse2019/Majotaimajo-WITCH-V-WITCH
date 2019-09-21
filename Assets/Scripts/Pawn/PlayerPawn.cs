using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    #region Public Members
    //Our movment speeds
    public float movementSpeed;
    public float rotationSpeed;

    public Transform originOfRotation;
    #endregion


    #region Private Members
    //We'll need our Transform
    Transform pawnTransform;

    //We'll add the important stuff
    Vector2 startPoint;
    float g_angle = 0f;
    readonly float radius = 6f;
    readonly float radiusSpeed = 5f;
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

    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    public void MoveInCircle(float _speed)
    {
        transform.RotateAround(originOfRotation.position, Vector3.back, _speed * Time.deltaTime);
        Vector2 desiredPosition = (transform.position - originOfRotation.position).normalized * radius + originOfRotation.position;
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    public void MoveOnDiameter(float _speed, Transform _target)
    {
        //This will calculate the vector between Raven and Luu, and will move on that normalized vector
        Vector2 playerToLuu = (_target.position - pawnTransform.position).normalized;
        gameObject.transform.TransformVector((playerToLuu * _speed) * Time.deltaTime) ;
    }
}
