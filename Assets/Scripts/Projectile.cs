using System.Collections;
using System.Collections.Generic;
using Alarm;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /*This will hold information from the ProjectileConfig*/
    [SerializeField]
    private ProjectileConfig configuration;

    SpriteRenderer spriteRenderer;

    //This will be crea
    Emitter emitter;

    float lifeTime = 10f;

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    private GraphicAnimation graphicAnimation;
    private bool noAnimation;
    private float duration = 10f;
    bool done = false;
    #endregion

    private void Awake()
    {
        destroyTimer = new Timer(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        emitter = GetComponent<Emitter>();
        graphicAnimation = GetComponent<GraphicAnimation>();
        origin = FindObjectOfType<RotationEmitter>().GetOriginObject(); //Will find the gameObject that shoot the bullet out
    }

    void ApplyConfiguration()
    {
        spriteRenderer.sprite = configuration.projectileGraphic;
        emitter = configuration.emitter;
        graphicAnimation = configuration.animation;
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer.StartTimer(0);
        done = destroyTimer.SetFor(duration, 0);
        
        if (done && noAnimation == false && graphicAnimation != null)
        {
            graphicAnimation.Animate(false);
            destroyTimer.SetToZero(0, true);
        }
    }

    public void ActivateAttachedSpell()
    {

    }
    public void AnimateOnDestroy() => noAnimation = false;
    public void NoAnimationOnDestroy() => noAnimation = true;

    public void SetDuration(float value)
    {
        duration = value;
    }

    public void SetLifeTime(float duration)
    {
        lifeTime = duration;
       SetDuration(lifeTime);
    }

    private void OnEnable()
    {
        
    }

    
}
