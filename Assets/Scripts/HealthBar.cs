using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform playerReference;
    public GameObject lifePrefab;
    public Color fadeTo;
    [Min(0.001f)]
    public float fadeTime;

    private PlayerBase playerStats;
    private Stack<GameObject> remainingLives;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int numLives = 0;
        if (playerReference != null && playerReference.TryGetComponent(out playerStats))
        {
            numLives = (int)playerStats.healthCurrent;
        }
        Debug.Log(String.Format("Number of Lives: {0}", numLives));

        remainingLives = new Stack<GameObject>();
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
        var deltaLives = numLives - remainingLives.Count;
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
        var prevLife = (remainingLives.Count != 0) ? remainingLives.Peek() : lifePrefab;
        for (int i = 0; i < numLives; i++)
        {
            // Place life icons adjacent to each other.
            var nextLife = Instantiate(prevLife, transform);
            if (prevLife.TryGetComponent(out RectTransform rectTransform))
            {
                // Get the size of each icon.
                var xScale = rectTransform.sizeDelta.x * prevLife.transform.localScale.x;
                nextLife.transform.Translate(new Vector3(xScale * i, 0));
            }
            remainingLives.Push(nextLife);
        }
    }

    private void RemoveLives(int numLives = 1)
    {
        // Fade out lost lives.
        for (int i = 0; i < numLives; i++)
        {
            var lastLife = remainingLives.Pop();
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
        Color fadeFrom = fadeImage.color;
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
