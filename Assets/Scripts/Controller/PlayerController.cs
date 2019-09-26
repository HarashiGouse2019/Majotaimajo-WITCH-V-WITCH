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
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(right))
        {
            //pawn.MoveInCircle(-pawn.rotationSpeed);
            pawn.Right();
            pawn.isMoving = true;
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(up))
        {
            pawn.Foward();
            pawn.isMoving = true;
        }
        //pawn.MoveOnDiameter(-pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(down))
        {
            pawn.Back();
            pawn.isMoving = true;
        }
            //pawn.MoveOnDiameter(pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(shoot))
        {
            if (GameManager.Instance.textBoxUI.gameObject.activeSelf != true && GameManager.Instance.MAGIC)
            {
                pawn.Shoot(0);
                GameManager.Instance.DecrementMagic(0.01f);
            }
        }

        RunSpecial();

        //Check pawn positioning
        if (pawn.transform.position.x > pawn.originOfRotation.gameObject.transform.position.x)
            pawn.Flip(-1);
        else if (pawn.transform.position.x < pawn.originOfRotation.gameObject.transform.position.x)
            pawn.Flip(1);
    }

    void RunSpecial()
    {
        GameManager manager = GameManager.Instance;

        //This looks a lot nicer!!!!
        if (Input.GetKeyDown(special1))
            pawn.ActivateSpell("Witch's Ritual");

        if (Input.GetKeyDown(special2))
            pawn.ActivateSpell("Chime");

        if (Input.GetKeyDown(special3))
            pawn.ActivateSpell("Spider's Nest");

        //We do this for Ui Purposes
        if (Input.GetKeyUp(special1))
            manager.ActivateSlot(manager.SLOTS[0], false);

        if (Input.GetKeyUp(special2))
            manager.ActivateSlot(manager.SLOTS[1], false);

        if (Input.GetKeyUp(special3))
            manager.ActivateSlot(manager.SLOTS[2], false);
    }
       
}


