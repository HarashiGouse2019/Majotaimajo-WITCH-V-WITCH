using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField, Range(-1f, 1f)]
    private float rotationSpeed;

    [SerializeField]
    private AutoRotate objectReference;

    private float currentAngle = 0f;

    private const float MAX_ANGLE = 360f;

    private const float RESET = 0f;

    IEnumerator rotationCycle;

    private void Start()
    {
        rotationCycle = Rotation(0.01f);
        rotationCycle.Start();
    }

    private void OnEnable()
    {
        currentAngle = objectReference ? objectReference.GetCurrentAngle() + rotationSpeed : default;
        if(rotationCycle != null) rotationCycle.Start();
    }

    IEnumerator Rotation(float delta = 0)
    {
        while (true)
        {
            currentAngle += rotationSpeed;
            UpdateRotation();

            if ((delta == 0)) yield return new WaitForEndOfFrame();
            else yield return new WaitForSeconds(delta);
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

    public float GetCurrentAngle() => currentAngle;

    private void OnDisable()
    {
        if(rotationCycle != null)
            rotationCycle.Stop();
    }
}
