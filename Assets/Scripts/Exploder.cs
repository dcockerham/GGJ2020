using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public Color fadeTo;

    private ParticleSystem particles;
    private SpriteRenderer sprite;
    private Color fadeFrom;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (particles != null && particles.isPlaying)
        {
            var p = particles.time / particles.main.duration;
            sprite.color = Color.Lerp(fadeFrom, fadeTo, p);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (ShouldExplode(collider))
        {
            Explode();
        }
    }

    private bool ShouldExplode(Collider2D collider)
    {
        if (CompareTag("Player"))
        {
            return collider.CompareTag("Enemy");
        }
        return collider.CompareTag("Bullet");
    }

    private void Explode()
    {
        Debug.Log("Exploded");
        if (particles != null && sprite != null && !particles.isPlaying)
        {
            particles.Play();
            fadeFrom = sprite.color;
        }
        Destroy(gameObject, particles.main.duration);
    }
}
