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

    Vector3 velocity = Vector3.zero;

    Transform originParent;

    private void Update()
    {
        if (targetTranform && gameObject.activeInHierarchy)
        {
            Vector3 point = (Vector3)transform.position - new Vector3(_offset.x * Mathf.Sign(targetTranform.localScale.x), _offset.y, _offset.z);
            Vector3 delta = (Vector3)targetTranform.position - new Vector3(point.x, point.y, point.z);
            Vector3 destination = (Vector3)transform.position + delta;

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
