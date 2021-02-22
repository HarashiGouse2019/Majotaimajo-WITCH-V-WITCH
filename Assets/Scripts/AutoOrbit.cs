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

    public float focusRadius = 2.0f;
    public float unfocusRadius = 4.0f;

    public float radiusSpeed = 5f;

    public float focusRotationSpeed;
    public float unfocusRotationSpeed;
    public float rotationSpeed = 80.0f;

    public float lerpSpeed = 1;

    private float radius = 4.0f;
    float accelerationRate = 0f;

    [SerializeField]
    private bool focusMode;

    void Start()
    {
        radius = unfocusRadius;
        rotationSpeed = unfocusRotationSpeed;

        center = objTarget.transform;
        transform.position = (transform.position - center.position).normalized * radius + center.position;

        OrbitCycle().Start();
    }

    IEnumerator OrbitCycle()
    {
        while (true)
        {
            transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, 0);
            desiredPosition = (transform.position - center.position).normalized * radius + center.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            yield return null;
        }
    }

    public void ChangeRadius(bool focus = false)
    {
        ChangeRadiusCycle(focus).Start();
    }

    IEnumerator ChangeRadiusCycle(bool focus = false)
    {


        switch (focus)
        {
            case true:
                if (focusMode == false)
                {
                    focusMode = focus;
                    accelerationRate = 0f;
                    while (radius != focusRadius)
                    {
                        radius = Mathf.Lerp(radius, focusRadius, radius * lerpSpeed);
                        rotationSpeed = Mathf.Lerp(unfocusRotationSpeed, focusRotationSpeed, 1f);
                        yield return null;
                    }
                }
                break;

            case false:
                if (focusMode)
                {
                    focusMode = focus;
                    accelerationRate = 0f;
                    while (radius != unfocusRadius)
                    {
                        radius = Mathf.Lerp(radius, unfocusRadius, radius * lerpSpeed);
                        rotationSpeed = Mathf.Lerp(focusRotationSpeed, unfocusRotationSpeed, 1f);
                        yield return null;
                    }
                }
                break;
        }
    }
}