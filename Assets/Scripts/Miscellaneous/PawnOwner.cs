using UnityEngine;

public class PawnOwner : MonoBehaviour
{
    /*Which pawn did this emitter come from?*/

    [SerializeField]
    Pawn pawnOwner;

    public void AssignOwner(Pawn owner)
    {
        pawnOwner = owner;
    }

    public Pawn GetOwner() => pawnOwner;
}
