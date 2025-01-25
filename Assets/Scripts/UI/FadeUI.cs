using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public static FadeUI Instance { get; private set; }

    [SerializeField] private Image image;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Instance already exists!");
        }
        Instance = this;

        image.color = new Color(0, 0, 0, 0);
    }

    public void FadeToBlack(Action callback)
    {
        StartCoroutine(FadeToBlackCoroutine(4.0f, callback));
    }

    public void FadeToClear(Action callback)
    {
        StartCoroutine(FadeToClearCoroutine(4.0f, callback));
    }

    private IEnumerator FadeToBlackCoroutine(float seconds, Action callback)
    {
        float timer = 0;
        while (timer < seconds)
        {
            timer += Time.deltaTime;
            image.color = new Color(0, 0, 0, timer / seconds);
            yield return null;
        }

        callback();
    }

    private IEnumerator FadeToClearCoroutine(float seconds, Action callback)
    {
        float timer = 0;
        while (timer < seconds)
        {
            timer += Time.deltaTime;
            image.color = new Color(0, 0, 0, 1 - timer / seconds);
            yield return null;
        }

        callback();
    }
}
