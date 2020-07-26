﻿using UnityEngine;

public class GetOrignatedSpawnPoint : MonoBehaviour
{
    public static GetOrignatedSpawnPoint Instance;

    public GameObject originatedSpawnPoint;

    public Emitter emitter;

    public Pawn pawn;

    public uint priority;

    readonly uint demolishVal = 5;

    private ItemDrops items;

    private Projectile projectile;


    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        projectile = GetComponent<Projectile>();

    }

    private void OnEnable()
    {
        try
        {
            if (projectile != null)
                emitter = projectile.GetEmitter();

            if (emitter.ParentPawn != null)
                pawn = emitter.ParentPawn;

            if (priority != 999) items = GetComponent<ItemDrops>();
        }
        catch { }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Check the priority level of a character
        GetOrignatedSpawnPoint self = this;
        GetOrignatedSpawnPoint targetOriginPoint = col.GetComponent<GetOrignatedSpawnPoint>();

        if (targetOriginPoint != null && targetOriginPoint != self)
        {
            if (targetOriginPoint.priority > priority + demolishVal && (targetOriginPoint.priority != 999))
            {
                items.Drop();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Border")) gameObject.SetActive(false);
    }

    public void SetPawnOrigin(Pawn pawn)
    {
        this.pawn = pawn;
    }

    public void SetEmitterOrigin(Emitter emitter)
    {
        this.emitter = emitter;
    }

    private void OnDisable()
    {

    }
}
