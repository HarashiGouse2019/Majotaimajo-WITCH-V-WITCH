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
            Scattered
        }

        public RotationType rotation;
        public DistributionType distribution;

        public float transitionDuration;
    }

    public Block block;
}

//And I think that's it... I think...