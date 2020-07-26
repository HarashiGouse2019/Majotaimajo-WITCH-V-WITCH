using Alarm;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType
{

    NoRotation,
    ClockwiseI,
    ClockwiseII,
    ClockwiseIII,
    CounterClockwiseI = -1,
    CounterClockwiseII = -2,
    CounterClockwiseIII = -3,
    Random  = 999
};
public enum DistributionType
{
    Uniformed,
    Biformed,
    UniformedIncrement,
    BiformedIncrement,
    Variant
}

public enum RotationFocusEffect
{
    Static,
    Increment
}

public enum RotationIntensityEffect
{
    Static,
    Increment
}

public enum Transition
{
    DESTROY_AT_END,
    LOOP,
    PINGPONG
}

/// <summary>
/// Motion type while emitter is moving
/// 
/// Linear - Nothing special. Got to each point.
/// Curve - A curve will be generated as it goes to each point
/// EaseIn - Going to each point starting fast, speeding down
/// EaseOut - Going to each point starting slow, speeding up;
/// </summary>
public enum Motion
{
    Linear,
    Curve,
    EaseIn,
    EaseOut
}


[DisallowMultipleComponent]
[RequireComponent(typeof(DanmakuSequencer))]
public abstract class Emitter : MonoBehaviour
{
    private static Emitter Instance;


    public DanmakuSequencer Sequencer;

    public Pawn ParentPawn { get; private set; }

    [SerializeField]
    protected int id = 0;

    [SerializeField]
    protected List<GameObject> bulletPrefab = new List<GameObject>();

    [SerializeField]
    protected Vector3[] points;

    [SerializeField]
    protected float motionThreshold;

    [SerializeField]
    protected float initialEmitterSpeed;

    [SerializeField]
    protected float emitterMinimumSpeed, emitterMaximumSpeed;

    [SerializeField]
    protected float emitterSpeedDelta;

    [SerializeField]
    protected GameObject originObject;

    protected string bulletMember;



    [Range(1, 10)] protected int numberOfProjectiles = 1;

    protected List<GameObject> existingProjectiles;

    protected Vector3 initialPosition, targetPosition;

    protected float bulletInitialSpeed;

    protected int incrementVal;

    protected float bulletSpeedLimit;

    protected float g_angle;

    protected bool loop;

    protected float loopSpeed = 0.05f;

    protected RotationType rotation;

    protected DistributionType distribution;

    protected Transition emitterTransition;

    protected Motion emitterMotion;

    protected float rotationFocus;

    protected float rotationFocusIncrementVal;

    protected float rotationFocusLimit;

    protected float rotationIntensity;

    protected float rotationIntensityIncrementVal;

    protected float rotationIntensityLimit;

    protected const float radius = 1f;

    protected int distStep = 0;

    protected Timer loopTimer;

    protected AudioClip sound;

    protected Pawn pawnOriginObject;

    protected int steps;

    protected float emitterSpeed;

    protected float emitterRotation;

    protected int currentPoint;

    protected float xInterval = 1f;
    protected float yInterval = 1f;

    //How many times to spawn
    protected int intervalCountLimit = 3;

    protected float projectileLifeTime = 10;

    protected bool animateOnDestroy = false;

    protected int currentInteval = 1;

    protected const int RESET = 0;

    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        originObject = gameObject;
        
    }

    public virtual void SetSpawnXInterval(float value)
    {
        xInterval = value;
    }

    public virtual void SetSpawnYInterval(float value)
    {
        yInterval = value;
    }

    public virtual void SetProjectileLifeTime(float value)
    {
        projectileLifeTime = value;
    }

    public virtual void SetAnimateProjectileOnDestroy(bool value)
    {
        animateOnDestroy = value;
    }

    public virtual void ResetIntervalCount()
    {
        currentInteval = RESET + 1;
    }

    public virtual void SetIntervalCountLimit(int value)
    {
        intervalCountLimit = value;
    }

    public virtual void SetBulletMember(string name)
    {
        bulletMember = name;
    }

    public virtual void SetProjectileDensity(int value)
    {
        numberOfProjectiles = value;
    }

    public virtual void SetLoopSpeed(float value)
    {
        loopSpeed = value;
    }

    public virtual void SetLooping(bool value)
    {
        loop = value;
    }
    public virtual void ToggleLooping()
    {
        loop = !loop;
    }

    public virtual void SetBulletInitialSpeed(float value)
    {
        bulletInitialSpeed = value;
    }

    public virtual void SetBulletLimitSpeed(float value)
    {
        bulletSpeedLimit = value;
    }

    public virtual void SetIncrementValue(int value)
    {
        incrementVal = value;
    }

    public virtual void SetAngle(float value)
    {
        g_angle = value;
    }

    public virtual void SetRotationFocus(float value)
    {
        rotationFocus = value;
    }

    public virtual void SetRotationFocusIncrement(float value)
    {
        rotationFocusIncrementVal = value;
    }

    public virtual void SetRotationFocusLimit(float value)
    {
        rotationFocusLimit = value;
    }

    public virtual void SetRotationIntensity(float value)
    {
        rotationIntensity = value;
    }

    public virtual void SetRotationIntensityIncrement(float value)
    {
        rotationIntensityIncrementVal = value;
    }

    public virtual void SetRotationIntensityLimit(float value)
    {
        rotationIntensityLimit = value;
    }

    public virtual void SetRotationType(RotationType type)
    {
        rotation = type;
    }

    public virtual void SetDistributionType(DistributionType type)
    {
        distribution = type;
    }

    public virtual GameObject GetOriginObject() => originObject;

    public virtual void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    public virtual void SetRelativePosition(Vector2 position)
    {
        transform.localPosition = ParentPawn.transform.position + (Vector3)position;
    }

    public virtual void SetPawnParent(Pawn pawn) => ParentPawn = pawn;

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        Sequencer = GetComponent<DanmakuSequencer>();
        if (Sequencer == null) Debug.Log("Failed to reference Sequencer...");
    }

    public virtual void ClearValues()
    {
        loop = false;
        distribution = default;
        rotation = default;
        bulletInitialSpeed = RESET;
        bulletSpeedLimit = RESET;
        loopSpeed = RESET;
        numberOfProjectiles = 1;
        g_angle = RESET;
        incrementVal = RESET;
        rotationFocus = RESET;
        rotationIntensity = RESET;
        rotationFocusIncrementVal = RESET;
        rotationIntensityIncrementVal = RESET;
        rotationIntensityLimit = RESET;
        rotationFocusLimit = RESET;
    }

    public virtual void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {

    }

    protected virtual void Transit()
    {

    }

    protected virtual void UpdateStartPoint()
    {

    }

    protected virtual void Loop()
    {

    }
}
