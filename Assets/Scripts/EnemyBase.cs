using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ShipBase
{
    // movement speed
    public float moveSpeedX;
    public float moveSpeedY;

    // parameters related to firing
    public GameObject bulletType;
    protected float fireTimer;
    public float fireDelay;
    public float fireDelayVariance;

    // boolean state parameters
    public bool isMoving;
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        fireTimer = fireDelay + Random.Range(-fireDelayVariance, fireDelayVariance);
    }

    // Update is called once per frame
    void Update()
    {
        // handle movement
        Vector3 newPos = transform.position;
        if (isMoving)
        {
            //transform.Translate(moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime, 0f);
            newPos.x += moveSpeedX * Time.deltaTime;
            newPos.y += moveSpeedY * Time.deltaTime;
        }
        transform.position = newPos;

        // handle firing
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            FireBullet();
            fireTimer = fireDelay + Random.Range(-fireDelayVariance, fireDelayVariance);
        }
    }

    void FireBullet()
    {
        Instantiate(bulletType, transform.position, Quaternion.identity);
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
