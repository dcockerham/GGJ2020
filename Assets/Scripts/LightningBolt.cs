using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : BulletBase
{
    public float activeTime;
    protected float boltTimer;
    public Collider2D boltCollider;
    protected FlashingScript boltFlasher;

    public AudioSource boltSound;
    
    // Start is called before the first frame update
    void Awake()
    {
        boltFlasher = gameObject.GetComponent<FlashingScript>();
        boltCollider = gameObject.GetComponent<Collider2D>();
        boltCollider.enabled = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!boltCollider.enabled)
        {
            if (!boltFlasher.isFlashing)
            {
                boltCollider.enabled = true;
                boltTimer = Time.time + activeTime;
            }
        }
        else if (Time.time >= boltTimer)
        {
            boltCollider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void LightningStrike()
    {
        boltCollider.enabled = false;
        gameObject.SetActive(true);
        boltFlasher.StartFlashing();
        if (boltSound)
        {
            boltSound.Play();
        }
    }
}
