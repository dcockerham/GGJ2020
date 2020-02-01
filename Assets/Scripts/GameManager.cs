using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPlaying;
    public int numLivingEnemies;

    private void Awake()
    {
        instance = this;
        isPlaying = true;
    }

    public void EnemySpawned()
    {
        numLivingEnemies++;
    }

    public void EnemyDestroyed()
    {
        numLivingEnemies--;
        if (isPlaying && numLivingEnemies <= 0)
        {
            print("~=STAGE CLEAR=~");
            isPlaying = false;
        }
    }

    public void GameOver()
    {
        if (isPlaying)
        {
            print("_//GAME OVER\\\\_");
        }
    }
}
