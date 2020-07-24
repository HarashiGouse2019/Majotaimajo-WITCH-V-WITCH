using System;
using UnityEngine;

[Serializable]
public class DanmakuMovement
{
    //The name of the movement
    [SerializeField]
    private string movementName;

    [SerializeField]
    private AnimationClip clip;

    public AnimationClip GetClip() => clip; 
}
