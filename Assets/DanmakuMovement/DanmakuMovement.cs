using System;
using UnityEngine;

[Serializable]
public class DanmakuMovement
{
    //The name of the movement
    [SerializeField]
    private string movementName = null;

    [SerializeField]
    private AnimationClip clip = null;

    public AnimationClip GetClip() => clip; 
}
