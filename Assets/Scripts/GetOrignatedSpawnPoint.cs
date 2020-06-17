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

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Check the priority level of a character
        GetOrignatedSpawnPoint originPoint = col.GetComponent<GetOrignatedSpawnPoint>();

        if (originPoint != null)
        {
            if (originPoint.priority > priority + demolishVal && (originPoint.priority != 999))
                gameObject.SetActive(false);

        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Border") gameObject.SetActive(false);
    }

}
