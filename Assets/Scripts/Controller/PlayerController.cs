using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Keymapper;

public class PlayerController : MonoBehaviour
{

    #region Public Members
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

    private void FixedUpdate()
    {
        InitMovementControls();
    }

    void InitMovementControls()
    {
        //Movement
        if (OnKey("left"))
        {
            pawn.Left();
            pawn.isMoving = true;
        }
        else if (OnKeyRelease("left"))
            pawn.isMoving = false;

        if (OnKey("right"))
        {
            pawn.Right();
            pawn.isMoving = true;
        }
        else if (OnKeyRelease("right"))
            pawn.isMoving = false;

        if (OnKey("up"))
        {
            pawn.Foward();
            pawn.isMoving = true;
        }
        else if (OnKeyRelease("up"))
            pawn.isMoving = false;

        if (OnKey("down"))
        {
            pawn.Back();
            pawn.isMoving = true;
        }
        else if (OnKeyRelease("down"))
            pawn.isMoving = false;
    }
    //Remember, we are controlling pawn!!!
    void InitControls()
    {
        if (OnKey("shoot"))
        {
            if (GameManager.Instance.GetPlayerMagic() > 0)
            {
                pawn.Shoot("Crystal");
                GameManager.Instance.DecrementMagic(0.01f);
                pawn.isMagicActivelyUsed = true;
            }
        } else
        {
            pawn.isMagicActivelyUsed = false;
        }

       pawn.isSneaking = OnKey("sneak");

        RunSpecial();
    }

    void RunSpecial()
    {
        GameManager manager = GameManager.Instance;

        //This looks a lot nicer!!!!
        if (OnKeyDown("special1"))
            pawn.ActivateSpell(SpellLibrary.library.spells[0].name);

        if (OnKeyDown("special2"))
            pawn.ActivateSpell(SpellLibrary.library.spells[1].name);

        if (OnKeyDown("special3"))
            pawn.ActivateSpell(SpellLibrary.library.spells[2].name);

        //We do this for Ui Purposes
        if (OnKeyRelease("special1"))
            manager.ActivateSlot(0, false);

        if (OnKeyRelease("special2"))
            manager.ActivateSlot(1, false);

        if (OnKeyRelease("special3"))
            manager.ActivateSlot(2, false);
    }
}


