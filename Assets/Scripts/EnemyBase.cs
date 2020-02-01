using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ShipBase
{
    // parameters related to firing
    protected float fireTimer;
    public float fireDelay;
    public float fireDelayVariance;

    public bool randomizeStartDirection = false;
    

    // Start is called before the first frame update
    public override void Start()
    {
        // set up difficulty mods
        DifficultyManager difficultyManager = DifficultyManager.instance;
        healthMax = healthMax * difficultyManager.getHealthMod();
        moveSpeedX *= difficultyManager.getMoveSpeedMod();
        moveSpeedY *= difficultyManager.getMoveSpeedMod();
        fireDelay /= difficultyManager.getFiringSpeedMod();
        fireDelayVariance /= difficultyManager.getFiringSpeedMod();

        base.Start();

        // randomize the firing timer
        fireTimer = Random.Range(0f, fireDelay) + Random.Range(-fireDelayVariance, fireDelayVariance);

        // randomize direction? if we want to
        if (randomizeStartDirection && Random.Range(0, 2) == 1)
        {
            moveSpeedX *= -1f;
        }

        // add the enemy to the game manager for tracking
        GameManager.instance.EnemySpawned();
    }

    private void OnDestroy()
    {
        // remove the enemy from the game manager
        GameManager.instance.EnemyDestroyed();
    }

    // Update is called once per frame
    void Update()
    {
        // handle movement
        Vector3 newPos = transform.position;
        if (canMove)
        {
            //transform.Translate(moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime, 0f);
            newPos.x += moveSpeedX * Time.deltaTime;
            newPos.y += moveSpeedY * Time.deltaTime;
        }
        transform.position = newPos;

        // handle firing
        if (canFire)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireBullet();
                fireTimer = fireDelay + Random.Range(-fireDelayVariance, fireDelayVariance);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("boundary"))
        {
            // if we hit the edge of the screen, reverse direction
            moveSpeedX *= -1;
        }
        else if (col.gameObject.CompareTag("bullet"))
        {
            BulletCollision(col);
        }
    }
}
