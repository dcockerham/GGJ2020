using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielder : EnemyBase
{
    public float shieldTime;
    public float rechargeTime;
    public float rechargeTimeVariance;
    protected float shieldTimer;

    public Sprite shieldedSprite;
    public Sprite unshieldedSprite;
    public AudioSource pingSound;
    protected SpriteRenderer spriteRenderer;

    public bool isShielded;

    public override void Start()
    {
        base.Start();

        isShielded = false;
        shieldTimer = Random.Range(0f, rechargeTime + rechargeTimeVariance);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();

        shieldTimer -= Time.deltaTime;
        if (shieldTimer <= 0f)
        {
            spriteRenderer.sprite = isShielded ? unshieldedSprite : shieldedSprite;
            isShielded = !isShielded;
            shieldTimer = isShielded ? shieldTime : (rechargeTime + Random.Range(-rechargeTimeVariance, rechargeTimeVariance));
        }
    }

    public override void TakeDamage(float damage)
    {
        if (!isShielded)
        {
            base.TakeDamage(damage);
        }
        else if (pingSound)
        {
            pingSound.Play();
        }
    }
}
