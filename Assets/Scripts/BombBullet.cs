using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : BulletBase
{
    public GameObject explodeObject;
    public bool isDocked;
    
    // Start is called before the first frame update
    public override void Start()
    {
        isDocked = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!isDocked)
        {
            base.Update();
        }
    }

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("boundary") || col.gameObject.CompareTag("bullet"))
        {
            // if we hit the edge of the screen (or another bullet), we're done! impact!
            Impact();
        }
    }

    public override void Impact()
    {
        if (explodeObject)
        {
            Instantiate(explodeObject, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public void Launch()
    {
        transform.parent = null;
        isDocked = false;
    }
}
