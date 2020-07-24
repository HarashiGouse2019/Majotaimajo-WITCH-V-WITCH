using System;
using UnityEngine;

[Serializable]
public class EmitterSpawner
{
    private static EmitterSpawner Instance;
    public EmitterType emitterType;
    public Vector2 spawnPosition;
    public Enchantment enchantment;

    //This is when the type is determined
    Emitter emitter;
    GameObject emitterGameObject;

    //Emitter Names
    readonly private string[] emitterNames =
    {
        "LinearEmitter",
        "RotationEmitter",
        "HoningLinearEmitter",
        "HoningRotationEmitter",
        "TransitionalEmitter",
        "OrbitalEmitter"
    };

    /// <summary>
    /// Initializaation of Emitter. This doesn't spawn it, but it allocates memory for this object.
    /// </summary>
    public void Create()
    {
        Instance = this;
        DetermineEmitterType();
    }

    void DetermineEmitterType()
    {
        int typeValue = (int)emitterType;
        emitterGameObject = ObjectPooler.GetMember(emitterNames[typeValue], out emitter);
    }

    public GameObject GetObject() => emitterGameObject;

    public Emitter GetEmitter() => emitter;

    public Emitter SpawnEmitter()
    {
        if (!emitterGameObject.activeInHierarchy)
        {
            emitter.Activate();
            emitter.SetPosition(spawnPosition);

            return emitter;
        }
        return null;
    }
}