using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : ShipBase
{
    // key code parameters
    public KeyCode leftKeyCode = KeyCode.LeftArrow;
    public KeyCode rightKeyCode = KeyCode.RightArrow;
    public KeyCode fireKeyCode = KeyCode.Space;
    public KeyCode tractorKeyCode = KeyCode.RightShift;

    // track relevant objects
    public GameObject tractorBeam;
    public List<GameObject> fireSources;


    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        if (canMove)
        {
            if (Input.GetKey(leftKeyCode))
            {
                newPos.x -= moveSpeedX * Time.deltaTime;
            }
            if (Input.GetKey(rightKeyCode))
            {
                newPos.x += moveSpeedX * Time.deltaTime;
            }
        }
        transform.position = newPos;


        if (Input.GetKeyDown(fireKeyCode) && canFire)
        {
            FireBullet();
        }

        if (Input.GetKey(tractorKeyCode) != tractorBeam.activeSelf)
        {
            tractorBeam.SetActive(!tractorBeam.activeSelf);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOver();
    }

    public override void FireBullet()
    {
        foreach (GameObject fireSource in fireSources)
        {
            Instantiate(bulletType, fireSource.transform.position, Quaternion.identity);
        }
    }
}
