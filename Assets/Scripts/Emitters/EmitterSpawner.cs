using System;
using UnityEngine;

using static EmitterLog;

[Serializable]
public class EmitterSpawner
{
    public EmitterType emitterType;
    public Vector2 spawnPosition;
    public bool worldSpace;
    public Enchantment enchantment;

    //This is when the type is determined
    Emitter emitter;
    GameObject emitterGameObject;


    /// <summary>
    /// Initializaation of Emitter. This doesn't spawn it, but it allocates memory for this object.
    /// </summary>
    public void Create()
    {
        DetermineEmitterType();
    }

    void DetermineEmitterType()
    {
        int typeValue = (int)emitterType;
        emitterGameObject = ObjectPooler.GetMember(EmitterNames[typeValue], out emitter);
    }

    public GameObject GetObject() => emitterGameObject;

    public Emitter GetEmitter() => emitter;

    public Emitter SpawnEmitter(bool worldPosition)
    {
        if (!emitterGameObject.activeInHierarchy)
        {
            emitter.Activate();

            if (worldPosition) emitter.SetPosition(spawnPosition);

            else emitter.SetRelativePosition(spawnPosition);

            return emitter;
        }
        return null;
    }
}