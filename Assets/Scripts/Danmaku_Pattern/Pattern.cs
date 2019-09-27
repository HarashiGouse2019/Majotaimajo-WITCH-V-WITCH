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
        [Range(1, 10)]
        public float initialRotation;
        public float rotationSpeed;

        public GameObject bullet;

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

        public float rotationFocus;
        public float rotationIntensity;

        public float transitionDuration;
    }

    public Block block;
}

//And I think that's it... I think...