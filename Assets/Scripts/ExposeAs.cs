using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposeAs : MonoBehaviour
{
    [SerializeField]
    private string exposeAs = "";

    public string GetExposedAs() => exposeAs;
}
