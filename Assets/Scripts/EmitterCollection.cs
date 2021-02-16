using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletPro;

public class EmitterCollection : MonoBehaviour
{
    /// <summary>
    /// This object is for having an array of
    /// different emitters for an object, and assigning those 
    /// emitters with a set Emitter Profile (which uses the
    /// BulletPro asset)
    /// </summary>
    /// 

    [System.Serializable]
    public class EmitterPatternPair
    {
        public BulletEmitter emitter;
        public int profileValue = 0;
    }
    public EmitterProfile[] emitterProfiles;
    public EmitterPatternPair[] shotTypes;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        for(int index = 0; index < shotTypes.Length; index++)
        {
            shotTypes[index].emitter.emitterProfile = emitterProfiles[shotTypes[index].profileValue];
        }
    }
}
