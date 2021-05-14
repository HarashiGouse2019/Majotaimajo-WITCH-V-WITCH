using System;
using UnityEngine;

[Serializable]
public class Parameter
{
    [SerializeField]
    object _value;

    public Parameter(object value)
    {
        _value = value;
    }
}
