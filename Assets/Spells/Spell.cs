using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    /*A spell is 1 or a full combination of enchantments being called simultaneously*/

    public uint spellPriority;
    public uint magicConsumtion;
    [SerializeField]
    private EmitterCollection emitterCollection;

    public bool Activated { get; private set; } = false;

    public bool enableSpellLooping = false;

    AnimationClip movement;

    //This will help use know who cast what spell
    Pawn parentPawn;

    /// <summary>
    /// Initialize the spell casting
    /// </summary>
    void Initalize()
    {
     
    }

    /// <summary>
    /// Set up the spawner's sequencer
    /// </summary>
    /// <param name="spawner"></param>
    void Setup(EmitterSpawner spawner)
    {

    }

    /// <summary>
    /// Activate a Spell, in which turn on the sequencers of all emitters.
    /// Parent pawn is who casted the spell.
    /// </summary>
    public void Activate(Pawn parentPawn)
    {
        Initalize();
    }


    /// <summary>
    /// Set the movement clip of a given character using the spell
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public AnimationClip SetMovementClip(AnimationClip clip)
    {
        movement = clip;
        return movement;
    }

    /// <summary>
    /// Set the movement clip of a given character using the spell
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public AnimationClip SetMovementClip(DanmakuMovement movement)
    {
        this.movement = movement.GetClip();
        return this.movement;
    }
}
