using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public enum Difficulty
    {
        SuperEasy = 0,
        Easy,
        Medium,
        Hard,
        Deadly,
    }
    public Difficulty difficulty = Difficulty.Medium;

    public List<float> enemyHealthMod;
    public List<float> bossHealthMod;
    public List<float> enemyDamageMod;
    public List<float> enemyMoveSpeed;
    public List<float> enemyBulletSpeed;
    public List<float> enemyFiringSpeedMod;


    private void Awake()
    {
        instance = this;
        difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", (int)difficulty);
    }

    public float getHealthMod()
    {
        return ((int)difficulty < enemyHealthMod.Count) ? enemyHealthMod[(int)difficulty] : 1;
    }

    public float getBossHealthMod()
    {
        return ((int)difficulty < bossHealthMod.Count) ? bossHealthMod[(int)difficulty] : 1;
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
