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
    public int healthMax;
    public int healthCurrent;

    // movement speed
    public float moveSpeedX;
    public float moveSpeedY;

    // parameters related to firing
    public GameObject bulletType;

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
        Instantiate(bulletType, transform.position, Quaternion.identity);
    }

    public virtual void TakeDamage(int damage)
    {
        healthCurrent -= damage;
        if (healthCurrent <= 0)
        {
            // BOOM! BLOW UP!
            Destroy(gameObject);
        }
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
