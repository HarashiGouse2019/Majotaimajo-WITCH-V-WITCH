using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Routine
{
    public Pattern pattern;
    public uint stepPos;
}

[CreateAssetMenu(fileName = "New Enchantment", menuName = "Enchantment")]
public class Enchantment : ScriptableObject
{
    public static Enchantment Instance;

    public float stepSpeed;

    public List<Routine> routine;

    public bool enableSequenceLooping;
}
