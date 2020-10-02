using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOrbit : MonoBehaviour
{

    [SerializeField]
    Transform centerOfOrbit;

    [SerializeField]
    private float rotationSpeed = 5;

    [SerializeField]
    private float radius = 0.5f;

    private float angleValue = 0;
    private void Start()
    {
        angleValue = Vector2.Angle(centerOfOrbit.position, transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        
        angleValue += rotationSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(angleValue), Mathf.Cos(angleValue)) * radius;
        transform.position = (Vector2)centerOfOrbit.position + offset;
    }
}
