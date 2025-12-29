using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup logoGroup;
    public Slider progressBar;

    [Header("Timing")]
    [Tooltip("Logo kaç saniyede fade-in yapsın")]
    public float fadeInTime = 0.5f;

    [Tooltip("Toplam splash süresi (saniye)")]
    public float splashDuration = 3f;

    [Header("Next Scene")]
    public string nextSceneName = "MainMenu";

    private void Start()
    {
        if (logoGroup) logoGroup.alpha = 0f;
        if (progressBar) progressBar.value = 0f;

        StartCoroutine(RunSplash());
    }

    IEnumerator RunSplash()
    {
        // Fade in (sadece giriş)
        yield return FadeIn();

        float timer = 0f;

        // Sabit süre boyunca bekle + bar doldur
        while (timer < splashDuration)
        {
            timer += Time.unscaledDeltaTime;

            if (progressBar)
                progressBar.value = Mathf.Clamp01(timer / splashDuration);

            yield return null;
        }

        // Direkt sahne geçişi
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.unscaledDeltaTime;
            logoGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            yield return null;
        }

        logoGroup.alpha = 1f;
    }
}
