using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class PlayerPawn : MonoBehaviour
{
    public static PlayerPawn player;

    #region Public Members
    //Our movment speeds
    public float movementSpeed;
    public float rotationSpeed;

    public Transform originOfRotation;

    public float radius = 6f;
    readonly public float radiusSpeed = 5f;
    public bool isMoving;

    #endregion


    #region Private Members
    //We'll need our Transform
    Transform pawnTransform;

    //We'll add the important stuff
    Vector2 startPoint;
    public float g_angle = 0f;

    bool recoil = false;
    bool returnVal;
    bool hit;
    
    Timer timer = new Timer(1, true);
    SpriteRenderer srenderer;
    bool isVisible;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = this;
        pawnTransform = GetComponent<Transform>();
        transform.position = (transform.position - originOfRotation.position).normalized * radius + originOfRotation.position;
        srenderer = GetComponent<SpriteRenderer>();
        isVisible = srenderer.isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (recoil == true) Wait(0.05f);
        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing
        MoveInCircle(0);
        if (hit) { GetHurt(3); }
    }

    //We'll get 2 functions, MoveInCircle, and MoveOnDiameter
    //Either circle around Luu, or go towards her.
    public void MoveInCircle(float _speed)
    {

        transform.RotateAround(originOfRotation.position, Vector3.back, _speed * Time.deltaTime);
        Vector2 desiredPosition = (transform.position - originOfRotation.position).normalized * radius + originOfRotation.position;
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

        Quaternion zLock = transform.rotation;
        zLock.eulerAngles = new Vector3(zLock.eulerAngles.x, zLock.eulerAngles.y, 0);
        transform.rotation = zLock;

        g_angle += -(_speed * Time.deltaTime);

    }

    public void MoveOnDiameter(float _speed, Transform _target)
    {
        //This will calculate the vector between Raven and Luu, and will move on that normalized vector
        radius += _speed * Time.deltaTime;
    }

        public void Shoot(int _index)
    {
        if (recoil == false)
        {
            Standard_Shoot.Instance = GetComponent<Standard_Shoot>();
            Standard_Shoot.Instance.SpawnBullets(1, _index);
            recoil = true;
        }
    }

    public bool CheckIfMoving()
    {
        return isMoving;
    }

    public void Flip(int _direction)
    {
        Vector3 xScale = transform.localScale;
        xScale.x = _direction;

        transform.localScale = xScale;
    }

    public void ActivateSpell(SequencerManager.Spell _spell)
    {
        _spell.sequence.enabled = true;
    }

    void Wait(float _duration)
    {
        timer.StartTimer(0);
        returnVal = timer.SetFor(_duration, 0);
        if (returnVal == true) recoil = false;
    }

    void GetHurt(int _duration)
    {
        timer.StartTimer(1);
        returnVal = timer.SetFor(_duration, 1);
        if (!returnVal)
        {
            Flash(0.05f);
        } else
        {
            hit = false;
            timer.SetToZero(1, true);
        }
    }

    void Flash(float _duration)
    {
        timer.StartTimer(0);
        if (timer.SetFor(_duration, 0)) {
            if (isVisible == true) { isVisible = false; timer.SetToZero(0); }
            if (isVisible == false) {isVisible = true; timer.SetToZero(0); }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
       switch(other.name)
        {
            case "testBullet(Clone)":
                hit = true;
                GameManager.Instance.DecrementLives();
                break;
            case "Luu_Obj":
                hit = true;
                GameManager.Instance.DecrementLives();
                break;
            default:

                break;
        }
    }


}
