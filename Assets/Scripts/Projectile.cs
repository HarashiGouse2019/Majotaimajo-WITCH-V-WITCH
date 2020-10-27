﻿using Alarm;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /*This will hold information from the ProjectileConfig*/
    [SerializeField]
    private ProjectileConfig configuration;

    SpriteRenderer spriteRenderer;

    //This will be crea
    Emitter emitter;

    SpellLibrary library;

    float lifeTime = 10f;

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    private Pawn ParentPawn;
    private GraphicAnimation graphicAnimation;
    private GetOrignatedSpawnPoint pawnOrigin;
    private bool noAnimation;
    private float duration = 10f;
    private Rigidbody2D rgb2d;
    bool done = false;

    ICaster caster;
    #endregion

    private void Awake()
    {
        destroyTimer = new Timer(1);
    }

    public GetOrignatedSpawnPoint GetOriginPoint() => pawnOrigin;
    public Rigidbody2D GetRigidbody2D() => rgb2d;
    public void AssignEmitter<T>(T emitter) where T : Emitter
    {
        this.emitter = emitter;
        pawnOrigin.SetEmitterOrigin(this.emitter);
    }

    void ApplyConfiguration()
    {
        if (configuration == null) return;

        if (spriteRenderer != null && configuration.projectileGraphic != null)
            spriteRenderer.sprite = configuration.projectileGraphic;

        if (configuration.attachedSpell != null)
        {
            library.AddNewSpell(configuration.attachedSpell);
            ActivateAttachedSpell();
        }
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

    public Emitter GetEmitter() => emitter;

    public void ActivateAttachedSpell()
    {
        Spell spell = library.FindSpell(configuration.attachedSpell.name);

        if (emitter != null && spell != null)
            spell.Activate(emitter.ParentPawn);
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        graphicAnimation = GetComponent<GraphicAnimation>();
        pawnOrigin = GetComponent<GetOrignatedSpawnPoint>();
        rgb2d = GetComponent<Rigidbody2D>();

        if (emitter == null) return;

        ParentPawn = emitter.ParentPawn;
        pawnOrigin.SetEmitterOrigin(emitter);
        origin = emitter.GetOriginObject(); //Will find the gameObject that shoot the bullet out

        library = caster != null ? caster.library : null;

        if (configuration != null)
            //Apply Configuration
            ApplyConfiguration();
    }

    /// <summary>
    /// Set caster origin
    /// </summary>
    /// <param name="caster"></param>
    public void SetCaster(ICaster caster)
    {
        this.caster = caster;
    }

    public ICaster GetCaster() => caster;
}
