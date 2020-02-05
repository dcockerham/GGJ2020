using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingScript : MonoBehaviour
{
    public Color flashColor = Color.white;
    protected Color originalColor;
    protected SpriteRenderer sprite;

    public int numberOfFlashes;
    public float flashLength;
    public float flashLengthVariance;
    public float flashTimer;
    public bool isFlashing;
    protected int flashCount;

    public bool flashOnStart;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        isFlashing = false;

        if (flashOnStart)
        {
            StartFlashing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            if (Time.time >= flashTimer)
            {
                if (sprite.color == flashColor)
                {
                    flashCount++;
                    sprite.color = originalColor;
                }
                else
                {
                    sprite.color = flashColor;
                }

                if (flashCount < numberOfFlashes)
                {
                    flashTimer = Time.time + flashLength + Random.Range(-flashLengthVariance, flashLengthVariance); ;
                }
                else
                {
                    isFlashing = false;
                }
            }
        }
    }

    public void StartFlashing()
    {
        flashTimer = Time.time + flashLength + Random.Range(-flashLengthVariance, flashLengthVariance);
        isFlashing = true;
        flashCount = 0;
        sprite.color = flashColor;
    }

    public void FlashOut()
    {
        flashTimer = Time.time + flashLength;
        isFlashing = true;
        flashCount = numberOfFlashes-1;
        sprite.color = flashColor;
        Invoke("DeactivateBolt", flashLength);
    }

    void DeactivateBolt()
    {
        gameObject.SetActive(false);
    }
}
