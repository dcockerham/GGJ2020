using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion : EnemyBase
{
    public float dropTime;

    private float yCounter;
    private bool moveLeft;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        moveLeft = moveSpeedX < 0;
        yCounter = 0;
        moveSpeedY = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (yCounter > 0)
        {
            yCounter -= Time.deltaTime;
            if (yCounter <= 0)
            {
                moveSpeedX = moveLeft ? moveSpeedY : -moveSpeedY;
                moveSpeedY = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("boundary") && !ignoringBoundaries)
        {
            // if we hit the edge of the screen, reverse direction
            if (yCounter <= 0)
            {
                moveSpeedY = moveLeft ? moveSpeedX : -moveSpeedX;
                moveSpeedX = 0;
                moveLeft = !moveLeft;
                yCounter = dropTime;
            }
        }
        else if (col.gameObject.CompareTag("bullet"))
        {
            BulletCollision(col);
        }
    }

}