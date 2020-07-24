using System.Collections;
using UnityEngine;

public class TransitionalEmitter : Emitter
{
    /*This emitter is able to interpolate between 2 or more points.
     You are also decide to loop the interpolation, pingpong, or destoryatend*/


    //The flow of going to each point
    int flow = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        StartCoroutine(MotionCycle());
    }

    IEnumerator MotionCycle()
    {
        while (true)
        {
            Transit();
            yield return null;
        }
    }

    protected override void Transit()
    {
        Vector3 direction;

        //Move between each point
        switch (emitterMotion)
        {
            case Motion.Linear:
                //Go to each point like normal
                direction = points[currentPoint] - transform.position;
                direction.Normalize();
                transform.Translate(direction * emitterSpeed * Time.deltaTime);
                break;

            case Motion.Curve:
                //Gradual Targetting
                direction = points[currentPoint] - transform.position;
                direction.Normalize();

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, emitterRotation * Time.deltaTime);
                transform.Translate(Time.deltaTime * emitterSpeed, 0f, 0f);
                break;

            case Motion.EaseIn:
                //Go to each point at a fast speed, slowing down
                DecreaseSpeed();
                direction = points[currentPoint] - transform.position;
                direction.Normalize();
                transform.Translate(direction * emitterSpeed * Time.deltaTime);
                break;

            case Motion.EaseOut:
                //Go to each point at a slow speed, speeding up
                IncreaseSpeed();
                direction = points[currentPoint] - transform.position;
                direction.Normalize();
                transform.Translate(direction * emitterSpeed * Time.deltaTime);
                break;

            default:
                break;
        }

        CheckPointDistance();
    }

    /// <summary>
    /// Checks the distance between this and the point that it's going to
    /// according to the motionThreshold
    /// </summary>
    void CheckPointDistance()
    {
        float distance = Vector3.Distance(gameObject.transform.position, points[currentPoint]);

        if (distance <= motionThreshold)
            NextPoint();
    }

    void NextPoint()
    {
        currentPoint += flow;
        switch (emitterMotion)
        {
            case Motion.Linear:
                emitterSpeed = initialEmitterSpeed;
                break;

            case Motion.Curve:
                emitterSpeed = initialEmitterSpeed;
                break;

            case Motion.EaseIn:
                emitterSpeed = emitterMaximumSpeed;
                break;

            case Motion.EaseOut:
                emitterSpeed = emitterMinimumSpeed;
                break;

            default:
                break;
        }
        ValidatePoint();
    }

    void IncreaseSpeed()
    {
        if (emitterSpeed > emitterMinimumSpeed && emitterSpeed < emitterMaximumSpeed)
            emitterSpeed += emitterSpeedDelta;
    }

    void DecreaseSpeed()
    {
        if (emitterSpeed > emitterMinimumSpeed && emitterSpeed < emitterMaximumSpeed)
            emitterSpeed -= emitterSpeedDelta;
    }

    void ValidatePoint()
    {
        switch (emitterTransition)
        {
            case Transition.DESTROY_AT_END:
                //Check if it's the last point
                if (AtLastPoint())
                {
                    ClearValues();
                    gameObject.SetActive(false);
                }

                break;

            case Transition.LOOP:
                //If we're at the last point, set the current position to zero
                if (AtLastPoint())
                    ZeroCurrentPoint();
                break;

            case Transition.PINGPONG:
                if(AtFirstPoint() || AtLastPoint())
                    InvertFlow();
                break;

            default:
                break;
        }
    }

    bool AtLastPoint() => currentPoint > points.Length - 1;

    bool AtFirstPoint() => currentPoint < 0;

    void ZeroCurrentPoint() => currentPoint = 0;

    void InvertFlow()
    {
        flow *= -1;
    }
}
