/*One big enumerator and string that can be used to pawn an emitter, or create a component of one to an object*/
using System.IO;
using UnityEngine;

public static class EmitterLog
{
    public enum EmitterType
    {
        Linear,
        Rotation,
        HoningLinear,
        HoningRotation,
        Transitional,
        Orbital,
        SpawningTrail
    }

    //Emitter Names
    readonly public static string[] EmitterNames =
    {
        "LinearEmitter",
        "RotationEmitter",
        "HoningLinearEmitter",
        "HoningRotationEmitter",
        "TransitionalEmitter",
        "OrbitalEmitter",
        "SpawningTrailEmitter"
    };

    /// <summary>
    /// Create an Emitter from the Object Pooler
    /// </summary>
    /// <returns></returns>
    public static Emitter CreateEmitter()
    {
        return null;
    }

    /// <summary>
    /// Add an Emitter Component to an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectTarget"></param>
    public static void AddEmitterComponent<T>(GameObject objectTarget) where T : Emitter
    {
        objectTarget.AddComponent<T>();
    }
}