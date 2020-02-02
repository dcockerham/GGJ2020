using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagEnemy : EnemyBase
{
    private float enemyDirectionX;

    public override void Start()
    {
        base.Start();

        SetEnemyDirection();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (moveSpeedX != enemyDirectionX)
        {
            SetEnemyDirection();
        }
    }

    private void SetEnemyDirection()
    {
        enemyDirectionX = moveSpeedX;
        if (gameObject.TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.flipX = enemyDirectionX < 0;
        };
    }
}
