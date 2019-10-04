using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Pattern")]
public class Pattern : ScriptableObject
{
    public static Pattern pattern;
    [Serializable]
    public class Block
    {
        public int amount;
        public float speed;
        public int incrementVal;
        public bool overrideRotation;
        [Range(1, 360)]
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
            Increment,
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

        public RotationIntensityEffect rotationIntensityEffect;
        public float rotationIntensity;
        public float rotationIntensityIncrementVal;

        public float transitionDuration;
    }

    public Block block;
}

//And I think that's it... I think...