using System;
using UnityEngine;

public interface ITargetable 
{
    GameObject targetObj { get; set; }
    Transform targetTransform { get; set; }
    Vector3 targetPosition { get; set; }
}