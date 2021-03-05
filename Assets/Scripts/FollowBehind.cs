using UnityEngine;

public class FollowBehind : MonoBehaviour
{
    //An object will follow it's target smoothly
    //Mainly for Raven and Maple's Emitters

    [SerializeField]
    float dampTime = 0.15f;

    [SerializeField]
    Vector3 _offset;

    [SerializeField]
    Transform targetTranform;

    [SerializeField, Range(-10f, 10f)]
    float zDepth = 10f;

    Vector3 velocity = Vector2.zero;

    Transform originParent;

    private void Update()
    {
        if (targetTranform && gameObject.activeInHierarchy)
        {
            Vector2 point = (Vector2)transform.position - new Vector2(_offset.x * Mathf.Sign(targetTranform.localScale.x), _offset.y);
            Vector2 delta = (Vector2)targetTranform.position - new Vector2(point.x, point.y);
            Vector2 destination = (Vector2)transform.position + delta;

            //Z will be changed
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    private void OnEnable()
    {
            originParent = transform.parent;
            transform.parent = PlayerSpawner.Transform;
    }

    private void OnDisable()
    {
        if(gameObject.activeInHierarchy)
        transform.parent = originParent;
    }

    public void UpdateOffset(Vector3 offset)
    {
        _offset = offset;
    }
}
