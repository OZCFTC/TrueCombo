using System.Collections;
using UnityEngine;
using TMPro;

public class ComboTextAnimator : MonoBehaviour
{
    public float popScale = 1.3f;
    public float popDuration = 0.12f;

    public float pulseScale = 1.05f;
    public float pulseSpeed = 2.5f;

    public float fadeOutTime = 0.2f;

    RectTransform rt;
    TMP_Text text;

    Coroutine pulseRoutine;
    Coroutine animRoutine;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        StartPulse();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetVisual();
    }

    // 🔥 Combo artınca çağır
    public void PlayPop()
    {
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(PopRoutine());
    }

    // ❌ Combo bozulunca çağır
    public void PlayFadeOutAndHide()
    {
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(FadeOutRoutine());
    }

    IEnumerator PopRoutine()
    {
        rt.localScale = Vector3.one;

        float t = 0f;
        while (t < popDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / popDuration;
            rt.localScale = Vector3.Lerp(Vector3.one, Vector3.one * popScale, k);
            yield return null;
        }

        t = 0f;
        while (t < popDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / popDuration;
            rt.localScale = Vector3.Lerp(Vector3.one * popScale, Vector3.one, k);
            yield return null;
        }
    }

    void StartPulse()
    {
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        while (true)
        {
            float s = 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * (pulseScale - 1f);
            rt.localScale = Vector3.one * s;
            yield return null;
        }
    }

    IEnumerator FadeOutRoutine()
    {
        Color c = text.color;
        float startAlpha = c.a;

        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(startAlpha, 0f, t / fadeOutTime);
            text.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
        ResetVisual();
    }

    void ResetVisual()
    {
        rt.localScale = Vector3.one;
        Color c = text.color;
        c.a = 1f;
        text.color = c;
    }
}
