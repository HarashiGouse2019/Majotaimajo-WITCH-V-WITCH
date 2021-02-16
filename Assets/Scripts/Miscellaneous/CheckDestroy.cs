using UnityEngine;

public class CheckDestroy : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.Watch(collision.gameObject.layer == LayerMask.NameToLayer("BulletBounds"),
        () => {
            Debug.Log(collision.name);
            gameObject.SetActive(false);
        }, null);
    }
}
