using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float rotateSpeed;

    // Use this for initialization
    void Start()
    {
        rotateSpeed = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 360 * rotateSpeed * Time.deltaTime);
    }
}
