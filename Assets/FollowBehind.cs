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

    [SerializeField]
    bool isCamera = false;

    [SerializeField]
    Transform additionalTarget; //For dynamic scaling of camera

    [SerializeField, Range(0.1f, 10f)]
    float divisorValue = 1f;

    [SerializeField, Range(0.1f, 10f)]
    float distanceTreshold = 1f;

    Vector2 initialPosition;

    Vector3 velocity = Vector2.zero;

    Camera m_camera;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        initialPosition = GetComponent<Transform>().position;
        m_camera = GetComponent<Camera>();
    }

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

        if (isCamera)
        {
            //Get the distance between the two objects
            float distance = Vector3.Distance(targetTranform.localPosition, additionalTarget.localPosition);

            //Change camera size on ration
            m_camera.orthographicSize = Mathf.Abs(distance) > distanceTreshold ? distance / divisorValue : m_camera.orthographicSize * 1;
        } 
    }
}
