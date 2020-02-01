using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public enum Difficulty
    {
        Easy = 0,
        Medium,
        Hard,
    }
    public Difficulty difficulty = Difficulty.Medium;

    public List<float> enemyHealthMod;
    public List<float> enemyDamageMod;
    public List<float> enemyMoveSpeed;
    public List<float> enemyBulletSpeed;
    public List<float> enemyFiringSpeedMod;


    private void Awake()
    {
        instance = this;
    }

    public float getHealthMod()
    {
        return ((int)difficulty < enemyHealthMod.Count) ? enemyHealthMod[(int)difficulty] : 1;
    }

    public float getDamageMod()
    {
        return ((int)difficulty < enemyDamageMod.Count) ? enemyDamageMod[(int)difficulty] : 1;
    }

    public float getMoveSpeedMod()
    {
        return ((int)difficulty < enemyMoveSpeed.Count) ? enemyMoveSpeed[(int)difficulty] : 1;
    }

    public float getBulletSpeedMod()
    {
        return ((int)difficulty < enemyBulletSpeed.Count) ? enemyBulletSpeed[(int)difficulty] : 1;
    }

    public float getFiringSpeedMod()
    {
        return ((int)difficulty < enemyFiringSpeedMod.Count) ? enemyFiringSpeedMod[(int)difficulty] : 1;
    }
}
