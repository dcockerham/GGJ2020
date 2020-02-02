using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyBase
{
    public BombBullet currentBomb;

    public override void Start()
    {
        base.Start();
        currentBomb = GetComponentInChildren<BombBullet>();
    }

    public override void FireBullet()
    {
        if (currentBomb)
        {
            currentBomb.Launch();
            currentBomb = null;
        }
        else if (bulletType)
        {
            if (fireSources.Count <= 0)
            {
                GameObject newBomb = Instantiate(bulletType, transform.position, Quaternion.identity);
                newBomb.transform.SetParent(transform);
                currentBomb = newBomb.GetComponent<BombBullet>();
            }
            else
            {
                GameObject newBomb = Instantiate(bulletType, fireSources[0].transform.position, Quaternion.identity);
                newBomb.transform.SetParent(fireSources[0].transform);
                currentBomb = newBomb.GetComponent<BombBullet>();
            }
        }
    }
}
