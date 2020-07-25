using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /*This will hold information from the ProjectileConfig*/

    SpriteRenderer spriteRenderer;

    //This will be crea
    Emitter emitter;

    float lifeTime = 10f;

    CheckDestroy destroy;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        emitter = GetComponent<Emitter>();
        destroy = GetComponent<CheckDestroy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNoAnimation()
    {
        destroy.NoAnimationOnDestroy();
    }

    public void SetAnimation()
    {
        destroy.AnimateOnDestroy();
    }

    public void ActivateAttachedSpell()
    {

    }

    public void SetLifeTime(float duration)
    {
        lifeTime = duration;
        destroy.SetDuration(lifeTime);
    }
}
