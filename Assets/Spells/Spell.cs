using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum EmitterType
{
    Linear,
    Rotation,
    HoningLinear,
    HoningRotation,
    Transitional,
    Orbital
}

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    /*A spell is 1 or a full combination of enchantments being called simultaneously*/

    public uint spellPriority;
    public uint magicConsumtion;
    [SerializeField]
    private List<EmitterSpawner> emitterSpawners = new List<EmitterSpawner>();

    public bool Activated { get; private set; } = false;

    public bool enableSpellLooping = false;
    AnimationClip movement;

    Animator animator;


    //This will help use know who cast what spell
    Pawn parentPawn;

    /// <summary>
    /// Initialize the spell casting
    /// </summary>
    void Initalize()
    {
        foreach(EmitterSpawner spawner in emitterSpawners)
        {
            spawner.Create();
            spawner.GetEmitter().SetPawnParent(parentPawn);
            spawner.SpawnEmitter(spawner.worldSpace);
            Setup(spawner);
            Activated = true;
        }
    }

    public void SetPawnParent(Pawn pawn) => parentPawn = pawn;

    /// <summary>
    /// Set up the spawner's sequencer
    /// </summary>
    /// <param name="spawner"></param>
    void Setup(EmitterSpawner spawner)
    {
        DanmakuSequencer sequencer = spawner.GetEmitter().Sequencer;

        if (sequencer == null) { Debug.Log("At setup, sequencer is null...");  return; }

        sequencer.spellOrigin = this;
        sequencer.enableSequenceLooping = enableSpellLooping;

        Enchantment enchanment = spawner.enchantment;

        sequencer.statistics.stepSpeed = enchanment.stepSpeed;

        for(int routinePos = 0; routinePos < enchanment.routine.Count; routinePos++)
        {
            sequencer.routines.Add(enchanment.routine[routinePos]);

            //And then we check if we enable looping
            if (sequencer.allowOverride) sequencer.enableSequenceLooping = enchanment.enableSequenceLooping;
        }

        sequencer.enabled = true;
    }

    /// <summary>
    /// Activate a Spell, in which turn on the sequencers of all emitters.
    /// Parent pawn is who casted the spell.
    /// </summary>
    public void Activate(Pawn parentPawn)
    {
        SetPawnParent(parentPawn);
        Initalize();
    }

    /// <summary>
    /// Get emitter spawners
    /// </summary>
    /// <returns></returns>
    public List<EmitterSpawner> GetSpawners() => emitterSpawners;

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

    /// <summary>
    /// Assign the animator 
    /// </summary>
    /// <param name="animator"></param>
    public void AssignAnimator(Animator animator)
    {
        this.animator = animator;
    }
}
