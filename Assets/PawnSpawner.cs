using UnityEngine;

public class PawnSpawner : MonoBehaviour
{
    [SerializeField]
    private Pawn pawnToSpawn;

    private void OnEnable()
    {
        Pawn newPawn = Instantiate(pawnToSpawn);
        newPawn.transform.parent = transform.parent;
        newPawn.transform.rotation = Quaternion.identity;
    }
}
