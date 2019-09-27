using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public static Spell Instance;

    public uint spellPriority;

    public float stepSpeed;

    [System.Serializable]
    public class Routine
    {
        public Pattern pattern;
        public uint stepPos;
    }

    public List<Routine> routine = new List<Routine>();

    public bool enableSequenceLooping;
}
