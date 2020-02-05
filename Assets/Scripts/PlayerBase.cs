using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : ShipBase
{
    // track relevant objects
    public GameObject tractorBeam;

    public float fireDelay = 0.5f;
    protected float delayTimer;

    public Sprite damagedSprite1;
    public Sprite damagedSprite2;
    protected SpriteRenderer spriteRenderer;


    public override void Start()
    {
        base.Start();
        delayTimer = 0f;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playState == GameManager.PlayState.Playing)
        {
            Vector3 newPos = transform.position;
            if (canMove)
            {
                if (Input.GetKey(gameManager.leftKeyCode))
                {
                    newPos.x -= moveSpeedX * Time.deltaTime;
                }
                if (Input.GetKey(gameManager.rightKeyCode))
                {
                    newPos.x += moveSpeedX * Time.deltaTime;
                }
            }
            transform.position = newPos;

            if (delayTimer <= 0f)
            {
                if (Input.GetKey(gameManager.fireKeyCode) && canFire)
                {
                    FireBullet();
                    delayTimer = fireDelay;
                }
            }
            else
            {
                if (Input.GetKeyUp(gameManager.fireKeyCode))
                {
                    delayTimer = 0f;
                }
                else
                {
                    delayTimer -= Time.deltaTime;
                }
            }

            if (Input.GetKey(gameManager.tractorKeyCode) != tractorBeam.activeSelf)
            {
                tractorBeam.SetActive(!tractorBeam.activeSelf);
            }
        }
        else if (gameManager.playState == GameManager.PlayState.Victory)
        {
            transform.Translate(0f, 10f * Time.deltaTime, 0f);
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance)
        {
            GameManager.instance.GameOver();
        }
    }

    public override void TakeDamage(float damage)
    {
        if (gameManager.playState == GameManager.PlayState.Playing)
        {
            healthCurrent -= damage;
            if (healthCurrent > 0)
            {
                if (damageSound)
                {
                    damageSound.Play();
                }

                if (healthCurrent <= 1)
                {
                    spriteRenderer.sprite = damagedSprite2;
                }
                else if (healthCurrent <= 2)
                {
                    spriteRenderer.sprite = damagedSprite1;
                }
            }
            else
            {
                // BOOM! BLOW UP!
                Die();
            }
        }
    }
}
