using UnityEngine;

public class PlayerSpawner : Singleton<PlayerSpawner>
{
    [SerializeField]
    private bool overrideParentPosition;

    [SerializeField]
    private Vector3 spawnPosition;

    public static PlayerPawn PlayerInstance { get; private set; }
    public static Transform Transform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Transform = transform;
        Init();
    }

    private void Init()
    {
        PlayerPawn objToSpawn = GameManager.PlayerPawn;
        PlayerInstance = Instantiate(objToSpawn, Transform);
        PowerGradeSystem.Init();
    }
}
