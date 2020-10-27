using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField, Range(-1f, 1f)]
    private float rotationSpeed;

    private float currentAngle = 0f;

    private const float MAX_ANGLE = 360f;

    private const float RESET = 0f;

    private void OnEnable()
    {
        Rotation(0.01f).Start();
    }

    IEnumerator Rotation(float delta = 0)
    {
        while (true)
        {
            currentAngle += rotationSpeed;
            UpdateRotation();
            
            yield return new WaitForSeconds(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }

    void UpdateRotation()
    {
        CheckRotation();
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void CheckRotation()
    {
        if (currentAngle >= MAX_ANGLE)
            ResetAngle();
    }

    void ResetAngle() => currentAngle = RESET;
}
