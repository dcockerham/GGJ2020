using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBase : MonoBehaviour
{
    public float tractorMoveSpeed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sucker"))
        {
            ShipBase parentShip = collision.transform.GetComponentInParent<ShipBase>();
            if (parentShip)
            {
                transform.position = Vector3.MoveTowards(transform.position, parentShip.transform.position, 
                    tractorMoveSpeed * Time.deltaTime);
            }
        }
    }
}
