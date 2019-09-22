using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Public Members
    //Have our keys mapped;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode shoot = KeyCode.Z;
    public KeyCode special1 = KeyCode.A;
    public KeyCode special2 = KeyCode.S;
    public KeyCode special3 = KeyCode.D;
    #endregion

    #region Private Members
    //Reference Pawn
    PlayerPawn pawn;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        pawn = GetComponent<PlayerPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        InitControls();
    }

    //Remember, we are controlling pawn!!!
    void InitControls()
    {
        //Movement
        if (Input.GetKey(left))
        {
            pawn.MoveInCircle(-pawn.rotationSpeed);
            pawn.isMoving = true;
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(right))
        {
            pawn.MoveInCircle(pawn.rotationSpeed);
            pawn.isMoving = true;
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(up))
            pawn.MoveOnDiameter(pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(down))
            pawn.MoveOnDiameter(-pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(shoot))
            pawn.Shoot(0);
    }
}
