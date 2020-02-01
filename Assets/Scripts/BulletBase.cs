using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    // is this a player bullet or an enemy bullet?  friendly fire: OFF!
    public ShipBase.Team bulletTeam;
    
    // movement speed
    public float moveSpeedX;
    public float moveSpeedY;

    // how much damage?
    public int damage;


    // Update is called once per frame
    public virtual void Update()
    {
        transform.Translate(moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime, 0f);
    }

    public virtual void Impact()
    {
        Destroy(gameObject);
    }
}
