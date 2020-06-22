﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Pattern")]
public class Pattern : ScriptableObject
{
    public static Pattern pattern;
    [Serializable]
    public class Block
    {
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

        public string bulletType;

        public bool loop;
        public float loopRate;

        public enum RotationType
        {

            NoRotation,
            ClockwiseI,
            ClockwiseII,
            ClockwiseIII,
            CounterClockwiseI = -1,
            CounterClockwiseII = -2,
            CounterClockwiseIII = -3
        };
        public enum DistributionType
        {
            Uniformed,
            Biformed,
            UniformedIncrement,
            BiFormedIncrement,
            Scattered
        }

        public RotationType rotation;
        public DistributionType distribution;

        public enum RotationFocusEffect
        {
            Static,
            Increment
        }

        public enum RotationIntensityEffect
        {
            Static,
            Increment
        }

        public RotationFocusEffect rotationFocusEffect;
        public float rotationFocus;
        public float rotationFocusIncrementVal;
        public float rotationFocusLimit;

        public RotationIntensityEffect rotationIntensityEffect;
        public float rotationIntensity;
        public float rotationIntensityIncrementVal;
        public float rotationIntensityLimit;

        public float transitionDuration;
    }

    [Header("Block Layer")]
    public Block[] blocks;
}

//And I think that's it... I think...