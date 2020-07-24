using UnityEngine;

public class GetOrignatedSpawnPoint : MonoBehaviour
{
    public static GetOrignatedSpawnPoint Instance;

    public GameObject originatedSpawnPoint;

    public Pawn pawn;

    public uint priority;

    readonly uint demolishVal = 5;

    private ItemDrops items;


    private void Awake()
    {
        Instance = this;

    }

    private void OnEnable()
    {
        
        if(originatedSpawnPoint != null &&
            (originatedSpawnPoint.layer == LayerMask.GetMask("Boss") ||
            originatedSpawnPoint.layer == LayerMask.GetMask("Player")))
            pawn = originatedSpawnPoint.GetComponent<Pawn>();
        if (priority != 999) items = GetComponent<ItemDrops>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Check the priority level of a character
        GetOrignatedSpawnPoint self = this;
        GetOrignatedSpawnPoint targetOriginPoint = col.GetComponent<GetOrignatedSpawnPoint>();

        if (targetOriginPoint != null && targetOriginPoint != self)
        {
            if (targetOriginPoint.priority > priority + demolishVal && (targetOriginPoint.priority != 999))
            {
                items.Drop();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Border")) gameObject.SetActive(false);
    }

    private void OnDisable()
    {

    }
}
