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
            pawn.MoveInCircle(pawn.rotationSpeed);
            pawn.isMoving = true;
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(right))
        {
            pawn.MoveInCircle(-pawn.rotationSpeed);
            pawn.isMoving = true;
        }
        else if (Input.GetKeyUp(left))
            pawn.isMoving = false;

        if (Input.GetKey(up))
            pawn.MoveOnDiameter(-pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(down))
            pawn.MoveOnDiameter(pawn.movementSpeed, pawn.originOfRotation);

        if (Input.GetKey(shoot))
            pawn.Shoot(0);

        if (Input.GetKeyDown(special1))
        {
            Slot1_Old = GameManager.Instance.SLOT1.color;
            GameManager.Instance.SLOT1.color = new Color(250f, 255f, 255f);
            pawn.ActivateSpell(SequencerManager.Instance.FindSpell("Witch's Ritual"));
        } else if (Input.GetKeyUp(special1))
        {
            GameManager.Instance.SLOT1.color = Slot1_Old;
        }

        if (Input.GetKeyDown(special2))
        {
            Slot2_Old = GameManager.Instance.SLOT2.color;
            GameManager.Instance.SLOT2.color = new Color(225f, 255f ,231f);
            pawn.ActivateSpell(SequencerManager.Instance.FindSpell("Chime"));
        }
        else if (Input.GetKeyUp(special2))
        {
            GameManager.Instance.SLOT2.color = Slot2_Old;
        }

        if (Input.GetKeyDown(special3))
        {
            Slot3_Old = GameManager.Instance.SLOT3.color;
            GameManager.Instance.SLOT3.color = new Color(204f, 255f, 209f);
            pawn.ActivateSpell(SequencerManager.Instance.FindSpell("Spider's Nest"));
        }
        else if (Input.GetKeyUp(special3))
        {
            GameManager.Instance.SLOT3.color = Slot3_Old;
        }

        //Check pawn positioning
        if (pawn.transform.position.x > pawn.originOfRotation.gameObject.transform.position.x)
            pawn.Flip(-1);
        else
            pawn.Flip(1);
    }
}
