using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform playerReference;
    public GameObject[] lifeImages;
    public Color fadeFrom;
    public Color fadeTo;
    [Min(0.001f)]
    public float fadeTime;

    private PlayerBase playerStats;
    private int lifeIndex;

    // Start is called before the first frame update
    void Start()
    {
        int numLives = 0;
        if (playerReference != null && playerReference.TryGetComponent(out playerStats))
        {
            numLives = (int)playerStats.healthCurrent;
        }
        Debug.Log(String.Format("Number of Lives: {0}", numLives));
        AddLives(numLives);
    }

    // Update is called once per frame
    void Update()
    {
        var numLives = 0;
        if (playerStats != null)
        {
            numLives = (int)Mathf.Max(playerStats.healthCurrent, 0);
        }

        // Update life bar state to match the game state.
        var deltaLives = numLives - lifeIndex;
        if (deltaLives > 0)
        {
            AddLives(deltaLives);
        }
        else if (deltaLives < 0)
        {
            RemoveLives(-deltaLives);
        }
    }

    private void AddLives(int numLives = 1)
    {
        StopAllCoroutines();
        for (; lifeIndex < lifeImages.Length - numLives; lifeIndex++)
        {
            Image image = lifeImages[lifeIndex].GetComponent<Image>();
            // Enable and set to the fadeFrom color.
            image.gameObject.SetActive(true);
            image.color = fadeFrom;
        }
    }

    private void RemoveLives(int numLives = 1)
    {
        // Fade out lost lives.
        for (int i = lifeIndex - 1; i >= 0 && i > lifeIndex - numLives - 1; i--)
        {
            var lastLife = lifeImages[i];
            StartCoroutine(FadeOutLifeIcon(lastLife));
        }
    }

    private IEnumerator FadeOutLifeIcon(GameObject lastLife)
    {
        Image fadeImage = lastLife.GetComponent<Image>();
        if (fadeImage == null)
        {
            Debug.LogWarning("Life icon not found.");
            yield break;
        }
        float fadeDuration = 0;

        while (fadeImage.color != fadeTo)
        {
            fadeImage.color = Color.Lerp(fadeFrom, fadeTo, fadeDuration / fadeTime);
            yield return null; // Wait for after update.
            fadeDuration += Time.deltaTime;
        }
        Destroy(lastLife);
    }
}
