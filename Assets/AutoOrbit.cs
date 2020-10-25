using UnityEngine;
using System.Collections;
using Extensions;

public class AutoOrbit : MonoBehaviour
{
    [SerializeField]
    GameObject objTarget;

    public Transform center;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 4.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    void Start()
    {
        center = objTarget.transform;
        transform.position = (transform.position - center.position).normalized * radius + center.position;
        radius = 4.0f;

        OrbitCycle().Start();
    }

    IEnumerator OrbitCycle()
    {
        while (true)
        {
            transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - center.position).normalized * radius + center.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

            yield return new  WaitForEndOfFrame();
        }
    }
}