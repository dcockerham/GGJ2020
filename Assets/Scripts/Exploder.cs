using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.tag);
        if (collider.CompareTag("Bullet"))
        {
            Explode();
        }
        else if (CompareTag("Player") && collider.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Exploded");
        if (particles != null && !particles.isPlaying)
        {
            particles.Play();
        }
        Destroy(gameObject/*, particles.main.duration*/);
    }
}
