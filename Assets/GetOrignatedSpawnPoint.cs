using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrignatedSpawnPoint : MonoBehaviour
{
    public static GetOrignatedSpawnPoint Instance;
    public GameObject originatedSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log(originatedSpawnPoint.name);
    }
}
