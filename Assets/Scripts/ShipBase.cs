using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBase : MonoBehaviour
{
    // what team is this ship on?  player or enemy?
    public enum Team
    {
        PlayerTeam,
        EnemyTeam,
    }
    public Team shipTeam;
    
    // health of the ship
    public float healthMax;
    public float healthCurrent;

    // movement speed
    public float moveSpeedX;
    public float moveSpeedY;

    // GameObjects that we need (bullets, explosions, etc.)
    public GameObject bulletType;
    public GameObject deathEffect;
    public List<GameObject> fireSources;

    // sounds!
    public AudioSource damageSound;

    // boolean state parameters
    public bool canMove = true;
    public bool canFire = true;


    // Start is called before the first frame update
    public virtual void Start()
    {
        healthCurrent = healthMax;
    }

    public virtual void FireBullet()
    {
        if (bulletType)
        {
            if (fireSources.Count <= 0)
            {
                Instantiate(bulletType, transform.position, Quaternion.identity);
            }
            else
            {
                foreach (GameObject fireSource in fireSources)
                {
                    Instantiate(bulletType, fireSource.transform.position, Quaternion.identity);
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        healthCurrent -= damage;
        if (healthCurrent > 0)
        {
            if (damageSound)
            {
                damageSound.Play();
            }
        }
        else
        {
            // BOOM! BLOW UP!
            Die();
        }
    }

    public virtual void Die()
    {
        if (deathEffect)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public virtual void BulletCollision(Collider2D col)
    {
        BulletBase bullet = col.GetComponent<BulletBase>();
        if (bullet && shipTeam != bullet.bulletTeam)
        {
            TakeDamage(bullet.damage);
            bullet.Impact();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bullet"))
        {
            BulletCollision(col);
        }
    }
}
