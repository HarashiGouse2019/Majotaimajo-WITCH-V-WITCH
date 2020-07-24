using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)]
    private float rotationSpeed;

    private float currentAngle = 0f;

    private const float MAX_ANGLE = 360f;

    private const float RESET = 0f;

    private void Start()
    {
        StartCoroutine(Rotation());
    }

    IEnumerator Rotation()
    {
        while (true)
        {
            currentAngle += rotationSpeed;
            UpdateRotation();
            
            yield return null;
        }
    }

    void UpdateRotation()
    {
        CheckRotation();
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void CheckRotation()
    {
        if (currentAngle >= 360)
            ResetAngle();
    }

    void ResetAngle() => currentAngle = RESET;
}
