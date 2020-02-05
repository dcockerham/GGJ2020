using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossVolt : EnemyBase
{
    public List<LightningBolt> boltList;
    protected float boltTimer;
    public float boltDelay;
    public float boltDelayVariance;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // randomize the firing timer
        boltTimer = Random.Range(0f, boltDelay) + Random.Range(-boltDelayVariance, boltDelayVariance);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // handle firing
        if (canFire)
        {
            boltTimer -= Time.deltaTime;
            if (boltTimer <= 0f)
            {
                boltList[Random.Range(0, boltList.Count)].LightningStrike();
                boltTimer = boltDelay + Random.Range(-boltDelayVariance, boltDelayVariance);
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        foreach (LightningBolt bolt in boltList)
        {
            bolt.gameObject.SetActive(false);
        }
    }
}
