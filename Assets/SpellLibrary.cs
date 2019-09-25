using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLibrary : MonoBehaviour
{
    public static SpellLibrary library;

    public Spell[] spells = new Spell[3];
    readonly bool isActivated;
    private void Awake()
    {
        library = this;
    }
    public Spell FindSpell(string _name)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (_name == spells[i].name) return spells[i];
            
        }
        return null;
    }
}
