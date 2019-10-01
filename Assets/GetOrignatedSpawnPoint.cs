using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrignatedSpawnPoint : MonoBehaviour
{
    public static GetOrignatedSpawnPoint Instance;

    public GameObject originatedSpawnPoint;
    public uint priority;

    readonly uint demolishVal = 5;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log(originatedSpawnPoint.name);
        priority = originatedSpawnPoint.GetComponent<Pawn>().priority;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<GetOrignatedSpawnPoint>().priority > priority + demolishVal)
        {
            gameObject.SetActive(false);
        }
    }
}
