﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrignatedSpawnPoint : MonoBehaviour
{
    public static GetOrignatedSpawnPoint Instance;

    public GameObject originatedSpawnPoint;
    public uint priority;

    readonly uint demolishVal = 5;

    private Pawn origin;

    private void Awake()
    {
        Instance = this;
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Check the priority level of a character
        GetOrignatedSpawnPoint originPoint = col.GetComponent<GetOrignatedSpawnPoint>();

        if (originPoint != null)
        {
            if (originPoint.priority > priority + demolishVal)
            {
                Debug.Log("If you're getting this message, these bullets are probably destroying themselves!!!!\n Comparing priorities: " + priority + " : " + originPoint.priority);
                gameObject.SetActive(false);
            }
        }
    }
}
