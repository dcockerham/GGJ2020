using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : ShipBase
{
    // track relevant objects
    public GameObject tractorBeam;

    public float fireDelay = 0.5f;
    protected float delayTimer;

    protected GameManager gameManager;


    public override void Start()
    {
        base.Start();
        gameManager = GameManager.instance;
        delayTimer = 0f;
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
                delayTimer -= Time.deltaTime;
            }

            if (Input.GetKey(gameManager.tractorKeyCode) != tractorBeam.activeSelf)
            {
                tractorBeam.SetActive(!tractorBeam.activeSelf);
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOver();
    }
}
