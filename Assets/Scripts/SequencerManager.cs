using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerManager : MonoBehaviour
{
    public static SequencerManager Instance;

    [System.Serializable]
    public class Spell
    {
        public string name;
        public DanmakuSequencer sequence;
    }

    public List<Spell> spells;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }

    public Spell FindSpell(string _name)
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (_name == spells[i].name) return spells[i];
        }
        return null;
    }
}
