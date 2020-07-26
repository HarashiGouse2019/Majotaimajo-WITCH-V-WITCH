using UnityEngine;

public class Collector : MonoBehaviour
{
    bool ValidateCollisionOrigin<T>(Collider2D collision, out T result)
    {
        T type = collision.GetComponent<T>();
        result = type;
        return (type != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we came across an item pick up
        if (ValidateCollisionOrigin(collision, out CollectibleItem item))
            item.Collect();
    }
}
