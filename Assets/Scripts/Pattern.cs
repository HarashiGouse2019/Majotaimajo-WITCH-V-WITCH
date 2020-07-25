using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Pattern")]
public class Pattern : ScriptableObject
{
    public static Pattern pattern;

    [Header("Basic Pattern Setup")]
    [Tooltip("How many bullets will you spawn?")]
    public int amount;

    [Tooltip("Will the pattern be tracking the target. Make sure overrideRotation is enabled")]
    public bool isHoming;

    [Tooltip("The starting speed of the bullets")]
    public float initialSpeed;

    [Tooltip("The change in speed over time")]
    public int incrementalSpeed;

    [Tooltip("The speed limit")]
    public int speedLimit;

    [Tooltip("Carry over the speed from the previous pattern")]
    public bool carryOverSpeed;

    [Tooltip("Override the rotation")]
    public bool overrideRotation;

    [Tooltip("The starting rotation of the bullets"), Range(0, 360)]
    public float initialRotation;

    public float rotationSpeed;

    [Header("Bullet Type")]
    public string bulletType;

    [Header("Loop Rate")]
    public bool loop;
    public float loopRate;


    [Header("Rotation Configuration")]
    public RotationType rotation;

    public DistributionType distribution;

    public RotationFocusEffect rotationFocusEffect;

    public float rotationFocus;
    public float rotationFocusIncrementVal;
    public float rotationFocusLimit;

    public RotationIntensityEffect rotationIntensityEffect;

    public float rotationIntensity;
    public float rotationIntensityIncrementVal;
    public float rotationIntensityLimit;

    public float transitionDuration;

    [Header("Spawning Rate Duration")]

    public float spawningXInterval;
    public float spawningYInterval;
    public int spawningCountLimit = 3;


    public int layerID = 0;
}

//And I think that's it... I think...