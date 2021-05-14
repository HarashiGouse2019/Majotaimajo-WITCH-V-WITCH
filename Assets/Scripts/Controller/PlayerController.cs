using UnityEngine;
using static Keymapper;
using Extensions;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool IsDashing = false;

    #region Private Members
    //Reference Pawn
    PlayerPawn pawn;
    SpellLibrary spellLibrary;
    IEnumerator controlCycle, movementCycle, magicCycle;
    #endregion



    // Start is called before the first frame update
    void Awake()
    {
        pawn = GetComponent<PlayerPawn>();
        spellLibrary = pawn.library;
        controlCycle = InitControlsCycle();
        movementCycle = InitMovementCycle(null);
        magicCycle = MagicUseCycle();
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

    IEnumerator InitMovementCycle(float? delta = 0)
    {
        while (true)
        {
            InitMovementControls();

            if ((delta == 0)) yield return new WaitForEndOfFrame();
            else if (delta == null) yield return null;
            else yield return new WaitForSeconds(delta.Value);
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
                    pawn.Shoot();

                    GameManager.Instance.DecrementMagic(0.1f);
                    pawn.IsMagicActivelyUsed = true;
                }
            },

            //OnKeyRelease
            () =>
            {
                pawn.CeaseShoot();
                pawn.IsMagicActivelyUsed = false;
            });

            yield return null;
        }
    }

    void InitMovementControls()
    {
        //Movement Action Left
        ControlAction("left", true, () =>
        {
            pawn.Left();
            pawn.IsMoving = true;
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving = false;
            return;
        });

        //Movement Action Right
        ControlAction("right", true, () =>
        {
            pawn.Right();
            pawn.IsMoving = true;
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving = false;
            return;
        });

        //Movement Action Up
        ControlAction("up", true, () =>
        {
            pawn.Foward();
            pawn.IsMoving = true;
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving = false;
            return;
        });

        //Movement Action Down
        ControlAction("down", true, () =>
        {
            pawn.Back();
            pawn.IsMoving = true;
            return;
        }, () =>
        {
            pawn.Steady();
            pawn.IsMoving = false;
            return;
        });
    }
    //Remember, we are controlling pawn!!!
    void InitControls()
    {
        pawn.IsOnFocus = OnKey("sneak");

        EventManager.Watch(IsDashing == false, () =>
            ControlAction("dash", false, () => IsDashing = true),
            out bool results);

        RunSpecial();
    }

    void RunSpecial()
    {
        //This looks a lot nicer!!!!
        ControlAction("specialA", false, () =>
        {
            Debug.Log("Special A Ssu!");
            pawn.ActivateSpell(spellLibrary.spells[pawn.IsOnFocus ? 4 : 0].name);
        });

        ControlAction("specialB", false, () =>
        {
            pawn.ActivateSpell(spellLibrary.spells[pawn.IsOnFocus ? 5 : 1].name);
        });

        ControlAction("specialC", false, () =>
        {
            pawn.ActivateSpell(spellLibrary.spells[pawn.IsOnFocus ? 6 : 2].name);
        });
    }
}