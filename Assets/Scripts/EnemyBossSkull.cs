using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSkull : EnemyBase
{
    public GameObject beamWeapon;
    public GameObject beamParticles;
    public GameObject beamFiringPoint;
    protected GameObject myBeam;
    protected float beamTimer;
    public float beamDelay;
    public float beamDelayVariance;
    public float beamFiringLength = 5f;
    protected float beamFiringTimer;

    public bool isShaking;
    public bool isFiringBeam;
    public bool soundStarting;

    public AudioSource soundBeamStart;
    public AudioSource soundBeamLoop;
    public AudioSource soundBeamEnd;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // randomize the firing timer
        beamTimer = beamDelay + Random.Range(-beamDelayVariance, beamDelayVariance);

        isShaking = false;
        isFiringBeam = false;
        soundStarting = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // handle firing
        if (canFire)
        {
            beamTimer -= Time.deltaTime;
            if (beamTimer <= 0f)
            {
                ChargeWeapon();
            }
        }

        if (isShaking)
        {

        }

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
    }

    void ChargeWeapon()
    {
        beamParticles.SetActive(true);
        canFire = false;
        StartCoroutine(FireBeam());
    }

    public IEnumerator FireBeam()
    {
        yield return new WaitForSeconds(3f);
        isShaking = true;
        yield return new WaitForSeconds(2f);
        myBeam = Instantiate(beamWeapon, beamFiringPoint.transform.position, Quaternion.identity);
        myBeam.transform.parent = gameObject.transform;
        myBeam.transform.localPosition = new Vector2(myBeam.transform.localPosition.x, -3.580775f);
        isFiringBeam = true;
        beamFiringTimer = beamFiringLength;
        soundBeamStart.Play();
        soundStarting = true;
        yield return new WaitForSeconds(beamFiringLength);
        beamTimer = beamDelay + Random.Range(-beamDelayVariance, beamDelayVariance);
        Destroy(myBeam);
        canFire = true;
        isFiringBeam = false;
        soundBeamLoop.Stop();
        soundBeamEnd.Play();
    }
}
