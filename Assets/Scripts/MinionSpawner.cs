using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float spawnTime;
    public float spawnVariance;
    public float firstTimeDelay;
    protected float spawnClock;
    public ParticleSystem spawnEffect;
    public List<GameObject> spawnables;

    protected GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnClock = spawnTime + Random.Range(-spawnVariance, spawnVariance) + firstTimeDelay;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playState == GameManager.PlayState.Playing)
        {
            spawnClock -= Time.deltaTime;
            if (spawnClock <= 0)
            {
                spawnEffect.Play();
                Instantiate(spawnables[Random.Range(0, spawnables.Count)], transform.position, Quaternion.identity);
                spawnClock = spawnTime + Random.Range(-spawnVariance, spawnVariance);
            }
        }
    }
}
