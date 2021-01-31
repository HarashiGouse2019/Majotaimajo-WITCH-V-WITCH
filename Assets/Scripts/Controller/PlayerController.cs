using UnityEngine;
using static Keymapper;
using Extensions;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    #region Private Members
    //Reference Pawn
    PlayerPawn pawn;
    GameManager manager;
    IEnumerator controlCycle, movementCycle, magicCycle;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        pawn = GetComponent<PlayerPawn>();
        controlCycle = InitControlsCycle();
        movementCycle = InitMovementCycle(Time.deltaTime);
        magicCycle = MagicUseCycle(0.01f);
    }

    private void Start()
    {
        controlCycle.Start();
        movementCycle.Start();
        magicCycle.Start();
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
            yield return new WaitForSecondsRealtime(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }

    IEnumerator MagicUseCycle(float delta = 0)
    {
        while (true)
        {

            ControlAction("shoot", true,
            //OnKeyPressed
            () =>
            {
                if (GameManager.Instance.GetPlayerMagic() > 0)
                {
                    pawn.Shoot("Crystal");
                    GameManager.Instance.DecrementMagic(0.01f);
                    pawn.IsMagicActivelyUsed.Set(true);
                }
            },

            //OnKeyRelease
            () => pawn.IsMagicActivelyUsed.Set(false)

            ) ;

            yield return new WaitForSeconds(delta == 0 ? Time.fixedDeltaTime : delta);
        }
    }

    void InitMovementControls()
    {
        //Movement Action Left
        ControlAction("left", true, () =>
        {
            pawn.Left();
            pawn.IsMoving.Set(true);
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving.Set(false);
            return;
        });

        //Movement Action Right
        ControlAction("right", true, () =>
        {
            pawn.Right();
            pawn.IsMoving.Set(true);
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving.Set(false);
            return;
        });

        //Movement Action Up
        ControlAction("up", true, () =>
        {
            pawn.Foward();
            pawn.IsMoving.Set(true);
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving.Set(false);
            return;
        });

        //Movement Action Down
        ControlAction("down", true, () =>
        {
            pawn.Back();
            pawn.IsMoving.Set(true);
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving.Set(false);
            return;
        });
    }
    //Remember, we are controlling pawn!!!
    void InitControls()
    {
        pawn.IsOnFocus = OnKey("sneak");

        RunSpecial();
    }

    void RunSpecial()
    {
        manager = GameManager.Instance;

        //This looks a lot nicer!!!!
        ControlAction("special1", false, () =>
        {
            pawn.ActivateSpell(pawn.library.spells[0].name);
        }, () =>
        {
            manager.ActivateSlot(0, false);
        });

        ControlAction("special2", false, () =>
        {
            pawn.ActivateSpell(pawn.library.spells[1].name);
        }, () =>
        {
            manager.ActivateSlot(1, false);
        });

        ControlAction("special3", false, () =>
        {
            pawn.ActivateSpell(pawn.library.spells[2].name);
        }, () =>
        {
            manager.ActivateSlot(2, false);
        });
    }
}