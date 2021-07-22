using System;
using UnityEngine;

[Serializable]
public class NativeEnemy
{
    [SerializeField]
    EnemyPawn pawn;

    GameObject enemyObj;
    Transform tranform;
    Vector3 position;
    SpriteRenderer graphics;

    internal void Init()
    {
        enemyObj = pawn.gameObject;
        tranform = enemyObj.transform;
        position = tranform.position;

        //TODO: Give pawn Sprite Renderer field for optimization
        graphics = pawn.GetComponent<SpriteRenderer>();
    }

    internal Pawn Pawn => pawn;
    internal GameObject Object => enemyObj;
    internal Transform Transform => tranform;
    internal Vector3 Position => position;
    internal SpriteRenderer Graphics => graphics;
}