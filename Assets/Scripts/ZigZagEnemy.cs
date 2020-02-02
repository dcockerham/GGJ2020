using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class ZigZagEnemy : MonoBehaviour
{
    private EnemyBase enemy;
    private float enemyDirectionX;

    void Start()
    {
        enemy = GetComponent<EnemyBase>();
        SetEnemyDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.moveSpeedX != enemyDirectionX)
        {
            SetEnemyDirection();
        }
    }

    private void SetEnemyDirection()
    {
        enemyDirectionX = enemy.moveSpeedX;
        var spriteTransform = gameObject.transform.GetChild(0);
        if (spriteTransform != null && spriteTransform.gameObject.TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.flipX = enemyDirectionX < 0;
        };
    }
}
