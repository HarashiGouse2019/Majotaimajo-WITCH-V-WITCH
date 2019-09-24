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

    Color Slot1_Old, Slot2_Old, Slot3_Old;

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
            //pawn.MoveInCircle(pawn.rotationSpeed);
            pawn.Left();
            pawn.isMoving = true;
            pawn.CalculateAngle();
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(right))
        {
            //pawn.MoveInCircle(-pawn.rotationSpeed);
            pawn.Right();
            pawn.isMoving = true;
            pawn.CalculateAngle();
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(up))
        {
            pawn.Foward();
            pawn.isMoving = true;
            pawn.CalculateAngle();
        }
        //pawn.MoveOnDiameter(-pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(down))
        {
            pawn.Back();
            pawn.isMoving = true;
            pawn.CalculateAngle();
        }
            //pawn.MoveOnDiameter(pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(shoot))
        {
            if (GameManager.Instance.textBoxUI.gameObject.activeSelf != true)
            {
                pawn.Shoot(0);
                GameManager.Instance.DecrementMagic(0.01f);
            }
        }

        RunSpecial();

        //Check pawn positioning
        if (pawn.transform.position.x > pawn.originOfRotation.gameObject.transform.position.x)
            pawn.Flip(-1);
        else
            pawn.Flip(1);
    }

    void RunSpecial()
    {
        //This looks a lot nicer!!!!
        if (Input.GetKeyDown(special1))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT1, true);

        if (Input.GetKeyUp(special1))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT1, false);

        if (Input.GetKeyDown(special2))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT2, true);

        if (Input.GetKeyUp(special2))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT2, false);

        if (Input.GetKeyDown(special3))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT3, true);

        if (Input.GetKeyUp(special3))
            GameManager.Instance.ActivateSlot(GameManager.Instance.SLOT3, false);

    }
       
}


