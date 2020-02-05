using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossFinal : EnemyBase
{
    public float directionRandomizer = 2f;

    public enum BossState
    {
        Idle,
        LazyBlaster,
        RapidBlaster,
        ChargeBeam,
        LightningStorm,
        Bomber,
        NUMBER_OF_STATES,
    }
    public BossState bossState;

    public float stateTimer;

    public List<float> moveSpeedByState;
    public List<float> fireRateByState;
    public List<float> fireVarianceByState;
    public List<float> stateTimeByState;

    public GameObject bombBullet;
    public GameObject beamBullet;
    public List<LightningBolt> boltBullets;
    protected List<LightningBolt> activeBolts = new List<LightningBolt>();
    
    public GameObject beamParticles;
    public GameObject beamFiringPoint;
    protected GameObject myBeam;
    protected float beamTimer;
    public float beamFiringLength = 5f;
    public float fireFadeTime = 0.5f;
    protected float beamFiringTimer;
    
    public bool isFiringBeam;
    public bool fireFading;
    public bool soundStarting;

    public AudioSource soundBeamStart;
    public AudioSource soundBeamLoop;
    public AudioSource soundBeamEnd;

    protected float screenWidth;

    public override void Start()
    {
        base.Start();

        isFiringBeam = fireFading = soundStarting = false;
        ChangeState(BossState.Idle);
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;

        DifficultyManager difficultyManager = DifficultyManager.instance;
        for (int i=0; i < (int)BossState.NUMBER_OF_STATES; i++)
        {
            moveSpeedByState[i] *= difficultyManager.getMoveSpeedMod();
            fireRateByState[i] /= difficultyManager.getFiringSpeedMod();
            fireVarianceByState[i] /= difficultyManager.getFiringSpeedMod();
        }

        stateTimeByState[(int)BossState.LightningStorm] = fireRateByState[(int)BossState.LightningStorm] * (boltBullets.Count + 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (gameManager.playState != GameManager.PlayState.Title && gameManager.playState != GameManager.PlayState.Pause)
        {
            // state change logic
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
            {
                ChangeState(PickNextState());
            }

            // movement logic
            // randomly reverse direction
            if (bossState != BossState.ChargeBeam && Random.Range(0f, 1f) <= (1 / directionRandomizer) * Time.deltaTime)
            {
                if (transform.position.x >= screenWidth / 4)
                {
                    moveSpeedX = -Mathf.Abs(moveSpeedX);
                }
                else if (transform.position.x <= -screenWidth / 4)
                {
                    moveSpeedX = Mathf.Abs(moveSpeedX);
                }
                else
                {
                    moveSpeedX *= -1f;
                }
            }

            // handle movement
            Vector3 newPos = transform.position;
            if (canMove)
            {
                newPos.x += moveSpeedX * Time.deltaTime;
                newPos.y += moveSpeedY * Time.deltaTime;
            }
            transform.position = newPos;

            // firing logic
            if (canFire)
            {
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    FireBullet();
                    fireTimer = fireDelay + Random.Range(-fireDelayVariance, fireDelayVariance);
                }
            }

            // charge beam special logic
            if (bossState == BossState.ChargeBeam)
            {
                if (isFiringBeam && myBeam)
                {
                    beamFiringTimer -= Time.deltaTime;
                    Vector3 newScale = myBeam.transform.localScale;
                    newScale.x = Mathf.Lerp(0f, 3f, (beamFiringLength - beamFiringTimer) / beamFiringLength * 2);
                    myBeam.transform.localScale = newScale;

                    if (soundStarting && !soundBeamStart.isPlaying)
                    {
                        soundBeamStart.Stop();
                        soundBeamLoop.Play();
                        soundStarting = false;
                    }
                }
                else if (fireFading && myBeam)
                {
                    beamFiringTimer -= Time.deltaTime;
                    Vector3 newScale = myBeam.transform.localScale;
                    newScale.x = Mathf.Lerp(3f, 0f, (fireFadeTime - beamFiringTimer) / fireFadeTime);
                    myBeam.transform.localScale = newScale;
                }
            }
        }
    }



    public override void FireBullet()
    {
        switch (bossState)
        {
            case BossState.Idle:
                break;
            case BossState.LazyBlaster:
            case BossState.RapidBlaster:
                if (bulletType)
                {
                    if (fireSources.Count <= 0)
                    {
                        Instantiate(bulletType, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        foreach (GameObject fireSource in fireSources)
                        {
                            Instantiate(bulletType, fireSource.transform.position, Quaternion.identity);
                        }
                    }
                }
                break;
            case BossState.ChargeBeam:
                ChargeWeapon();
                break;
            case BossState.LightningStorm:
                if (boltBullets.Count > 0)
                {
                    int boltNum = Random.Range(0, boltBullets.Count);
                    boltBullets[boltNum].LightningStrike();
                    activeBolts.Add(boltBullets[boltNum]);
                    boltBullets.RemoveAt(boltNum);
                }
                break;
            case BossState.Bomber:
                if (bombBullet)
                {
                    if (fireSources.Count <= 0)
                    {
                        BombBullet newBomb = Instantiate(bombBullet, transform.position, Quaternion.identity).GetComponent<BombBullet>();
                        newBomb.Launch();
                    }
                    else
                    {
                        foreach (GameObject fireSource in fireSources)
                        {
                            BombBullet newBomb = Instantiate(bombBullet, fireSource.transform.position, Quaternion.identity).GetComponent<BombBullet>();
                            newBomb.Launch();
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public BossState PickNextState()
    {
        if (bossState != BossState.Idle)
        {
            return BossState.Idle;
        }

        float randomPicker = Random.Range(0f, 5f);

        if (randomPicker > 4f)
        {
            return BossState.Bomber;
        }
        else if (randomPicker > 3f)
        {
            return BossState.LightningStorm;
        }
        else if (randomPicker > 2f)
        {
            return BossState.ChargeBeam;
        }
        else if (randomPicker > 1f)
        {
            return BossState.RapidBlaster;
        }
        else
        {
            return BossState.LazyBlaster;
        }
    }

    public void ChangeState(BossState newState)
    {
        moveSpeedX = moveSpeedByState[(int)newState];
        fireDelay = fireRateByState[(int)newState];
        fireDelayVariance = fireVarianceByState[(int)newState];
        stateTimer = stateTimeByState[(int)newState];
        fireTimer = fireDelay + Random.Range(-fireDelayVariance, fireDelayVariance);

        if (bossState == BossState.LightningStorm)
        {
            foreach (LightningBolt bolt in activeBolts)
            {
                boltBullets.Add(bolt);
            }
            activeBolts.Clear();
        }

        bossState = newState;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("boundary") && !ignoringBoundaries)
        {
            if (col.gameObject.name == "LowerBound")
            {
                moveSpeedY = 0;
            }
            else
            {
                // if we hit the edge of the screen, reverse direction
                moveSpeedX *= -1;
            }
        }
        else if (col.gameObject.CompareTag("bullet"))
        {
            BulletCollision(col);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boundary") && !ignoringBoundaries)
        {
            if (collision.gameObject.name == "LowerBound")
            {
                moveSpeedY = 0;
            }
            else
            {
                // if we hit the edge of the screen, reverse direction
                if (transform.position.x > 0)
                {
                    moveSpeedX = -Mathf.Abs(moveSpeedX);
                }
                else
                {
                    moveSpeedX = Mathf.Abs(moveSpeedX);
                }
            }
        }
    }

    void ChargeWeapon()
    {
        beamParticles.SetActive(true);
        canFire = false;
        StartCoroutine(FireBeam());
    }

    public IEnumerator FireBeam()
    {
        yield return new WaitForSeconds(2.5f);
        myBeam = Instantiate(beamBullet, beamFiringPoint.transform.position, Quaternion.identity);
        myBeam.transform.parent = beamFiringPoint.transform;
        myBeam.transform.localPosition = new Vector2(myBeam.transform.localPosition.x, -3.580775f);
        isFiringBeam = true;
        beamFiringTimer = beamFiringLength;
        soundBeamStart.Play();
        soundStarting = true;
        yield return new WaitForSeconds(beamFiringLength);
        soundBeamLoop.Stop();
        soundBeamEnd.Play();
        beamParticles.SetActive(false);
        fireFading = true;
        isFiringBeam = false;
        beamFiringTimer = fireFadeTime;
        yield return new WaitForSeconds(fireFadeTime);
        Destroy(myBeam);
        canFire = true;
        fireFading = false;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        foreach (LightningBolt bolt in boltBullets)
        {
            if (bolt)
            {
                bolt.gameObject.SetActive(false);
            }
        }

        foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>())
        {
            if (enemy)
            {
                enemy.Die();
            }
        }
    }
}
