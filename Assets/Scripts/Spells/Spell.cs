using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public static Spell Instance;

    public uint spellPriority;
    public uint magicConsumtion;

    public float stepSpeed;

    [System.Serializable]
    public class Routine
    {
        public Pattern pattern;
        public uint stepPos;
    }

    [System.Serializable]
    public class Layer
    {
        public List<Routine> routine = new List<Routine>();
        public bool enableSequenceLooping;
    }

    public List<Layer> routineLayer = new List<Layer>();

    public List<Routine> routine = new List<Routine>();

    public bool enableSequenceLooping;
}
