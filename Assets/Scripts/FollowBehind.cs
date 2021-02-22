using UnityEngine;

public class FollowBehind : MonoBehaviour
{
    //An object will follow it's target smoothly
    //Mainly for Raven and Maple's Emitters

    [SerializeField]
    float dampTime = 0.15f;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Transform targetTranform;

    [SerializeField, Range(-10f, 10f)]
    float zDepth = 10f;

    Vector3 velocity = Vector2.zero;

    private void Update()
    {
        if (targetTranform)
        {
            Vector3 point = transform.localPosition - new Vector3(offset.x * Mathf.Sign(targetTranform.localScale.x), offset.y);
            Vector3 delta = targetTranform.localPosition - new Vector3(point.x, point.y, zDepth);
            Vector3 destination = transform.localPosition + delta;

            //Z will be changed
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, destination, ref velocity, dampTime);
        }
    }
}
