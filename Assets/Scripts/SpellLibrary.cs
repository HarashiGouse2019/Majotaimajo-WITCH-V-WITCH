using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLibrary : MonoBehaviour
{
    public static SpellLibrary library;

    public Spell spellInUse;

    public Spell[] spells = new Spell[3];

    public uint spellIndex = 0;

    [Tooltip("If you have a bullet that can spawn other bullets, " +
        "you can attach a single spell to the library, and toggle this" +
        "feature on.")]
    public bool initializeOnStart;
    
    readonly bool isActivated;
    readonly uint reset = 0;
    

    private void Awake()
    {
        library = this;
    }
    public Spell FindSpell(string _name)
    {
        #region ForLoop
        //Make sure the spellIndex is reset before you loop again
        spellIndex = reset;

        for (int i = 0; i < spells.Length; i++)
        {
            if (_name == spells[i].name) 
                return spells[i];
            else
                spellIndex++;
        }
        return null; 
        #endregion
    }

    public uint GetSpellIndex(string _name)
    {
        FindSpell(_name);
        return spellIndex;
    }
}
