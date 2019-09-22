using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour
{
    public static FaceTowards Instance;

    public Transform target;

    public PlayerPawn pawn;

    private void Awake()
    {
        Instance = this;
        target = GameObject.FindGameObjectWithTag("target").GetComponent<Transform>();
        pawn = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pawn.CheckIfMoving())
        {
           
        }
    }
}
