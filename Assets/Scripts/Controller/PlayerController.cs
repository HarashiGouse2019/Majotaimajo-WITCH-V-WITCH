using System.Collections;
using UnityEngine;

using static Keymapper;
using Extensions;

public class PlayerController : MonoBehaviour
{

    #region Public Members
    #endregion

    #region Private Members
    //Reference Pawn
    PlayerPawn pawn;
    GameManager manager;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        pawn = GetComponent<PlayerPawn>();
    }

    private void Start()
    {
        InitControlsCycle().Start();
        InitMovementCycle().Start();
    }

    IEnumerator InitControlsCycle()
    {
        while (true)
        {
            InitControls();
            yield return null;
        }
    }

    IEnumerator InitMovementCycle(float delta = 0)
    {
        while (true)
        {
            InitMovementControls();
            yield return new WaitForSeconds(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }


    void InitMovementControls()
    {
        //Movement Action Left
        ControlAction("left", () =>
        {
            pawn.Left();
            pawn.isMoving = true;
            return;
        }, () =>
        {
            pawn.isMoving = false;
            return;
        });

        //Movement Action Right
        ControlAction("right", () =>
        {
            pawn.Right();
            pawn.isMoving = true;
            return;
        }, () =>
        {
            pawn.isMoving = false;
            return;
        });

        //Movement Action Up
        ControlAction("up", () =>
        {
            pawn.Foward();
            pawn.isMoving = true;
            return;
        }, () =>
        {
            pawn.isMoving = false;
            return;
        });

        //Movement Action Down
        ControlAction("down", () =>
        {
            pawn.Back();
            pawn.isMoving = true;
            return;
        }, () =>
        {
            pawn.isMoving = false;
            return;
        });
    }
    //Remember, we are controlling pawn!!!
    void InitControls()
    {
        if (OnKey("shoot") && GameManager.Instance.GetPlayerMagic() > 0)
        {
                pawn.Shoot("Crystal");
                GameManager.Instance.DecrementMagic(0.01f);
                pawn.isMagicActivelyUsed = true;
        } else
            pawn.isMagicActivelyUsed = false;

       pawn.isSneaking = OnKey("sneak");

        RunSpecial();
    }

    void RunSpecial()
    {
        manager = GameManager.Instance;

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


