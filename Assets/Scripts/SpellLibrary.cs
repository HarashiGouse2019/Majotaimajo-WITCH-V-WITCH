using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for a caster
/// </summary>
public interface ICaster
{
    SpellLibrary library { get; set; }
    void ActivateSpell(string _name, bool cancelRunningSpell = false);
}

public class SpellLibrary : MonoBehaviour
{
    public static SpellLibrary library;

    public Spell spellInUse;
    private int spellSize = 3;
    public List<Spell> spells;

    //We need the spell caster.
    private ICaster caster;

    public uint spellIndex = 0;

    [Tooltip("If you have a bullet that can spawn other bullets, " +
        "you can attach a single spell to the library, and toggle this" +
        "feature on.")]
    public bool initializeOnStart;
    
    readonly uint reset = 0;
    
    private void Awake()
    {
        library = this;
    }

    private void OnEnable()
    {
        caster = GetComponent<Pawn>();
        Initialize();
    }

    public void AddNewSpell(Spell spell)
    {
        spells.Add(spell);
    }

    public Spell FindSpell(string _name)
    {
        #region ForLoop
        //Make sure the spellIndex is reset before you loop again
        spellIndex = reset;

        for (int i = 0; i < spells.Count; i++)
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

    private void Initialize()
    {
        try
        {
            foreach (Spell spell in spells)
            {
                spell.SetCaster(caster);
            }
        }catch
        {
            //Do nothing
        }
    }

    /// <summary>
    /// Set who the caster of this spell library is
    /// </summary>
    /// <param name="caster"></param>
    public void SetCaster(ICaster caster)
    {
        this.caster = caster;
    }
}