using UnityEngine;
using Alarm;

public class CheckDestroy : MonoBehaviour
{

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    private GraphicAnimation graphicAnimation;
    private bool noAnimation;
    private float duration = 10f;
    #endregion

    void Awake()
    {
        destroyTimer = new Timer(1);
    }

    void Start()
    {
        graphicAnimation = GetComponent<GraphicAnimation>();
        origin = FindObjectOfType<RotationEmitter>().GetOriginObject(); //Will find the gameObject that shoot the bullet out
    }

    private void OnEnable()
    {
        destroyTimer.StartTimer(0);
        if (destroyTimer.currentTime[0] > duration)
        {
            if (noAnimation)
                gameObject.SetActive(false);
            else
                graphicAnimation.Animate(false);

            destroyTimer.SetToZero(0, true);
        }
    }

    public void AnimateOnDestroy() => noAnimation = false;
    public void NoAnimationOnDestroy() => noAnimation = true;

    public void SetDuration(float value)
    {
        duration = value;
    }
}
