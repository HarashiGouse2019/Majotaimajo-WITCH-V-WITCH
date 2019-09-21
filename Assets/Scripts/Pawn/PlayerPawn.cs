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
    float radius = 5f; 
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pawnTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //We do this to eliminate the risk of overflowing
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; 


    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    public void MoveInCircle(float _speed)
    {
        float angleStep = 360f / _speed;
        float angle = g_angle;
        gameObject.transform.Translate((new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius) * _speed);

        angle += angleStep;
    }

    public void MoveOnDiameter(float _speed, Transform _target)
    {
        //This will calculate the vector between Raven and Luu, and will move on that normalized vector
        Vector2 playerToLuu = _target.position - pawnTransform.position;
        gameObject.transform.TransformVector(playerToLuu);
    }
}
