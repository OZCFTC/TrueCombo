using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("UI")]
    public CanvasGroup fadeGroup;

    [Header("Timing")]
    public float fadeOutTime = 0.3f;
    public float fadeInTime = 0.3f;
    public float waitBeforeLoad = 1f;   // siyah ekranda bekleme


    bool isTransitioning;

    void Awake()
    {
        // Tek instance garantisi
        if (Instance != null && Instance != this)
        {
            StopAllCoroutines();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Güvenli başlangıç
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Her sahne yüklendiğinde mutlaka aç (siyah ekranda kalmayı engeller)
        if (fadeGroup != null)
            StartCoroutine(Fade(1f, 0f, fadeInTime));
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

IEnumerator LoadSceneRoutine(string sceneName)
{
    isTransitioning = true;

    yield return Fade(0f, 1f, fadeOutTime);

    // ✅ Sahneye geçmeden önce 1 sn bekle
    if (waitBeforeLoad > 0f)
        yield return new WaitForSecondsRealtime(waitBeforeLoad);

    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
    while (!op.isDone) yield return null;

    isTransitioning = false;
}


    IEnumerator Fade(float from, float to, float duration)
    {
        // ✅ CanvasGroup yoksa sessizce çık (MissingReference fix)
        if (fadeGroup == null) yield break;

        bool toBlack = to > from;
        fadeGroup.blocksRaycasts = toBlack;
        fadeGroup.interactable = toBlack;

        fadeGroup.alpha = from;

        float t = 0f;
        while (t < duration)
        {
            if (fadeGroup == null) yield break; // ekstra güvenlik
            t += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        if (fadeGroup == null) yield break;

        fadeGroup.alpha = to;

        if (Mathf.Approximately(to, 0f))
        {
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }
    }
}
