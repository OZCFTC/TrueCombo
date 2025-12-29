using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;        // PausePanel
    public CanvasGroup canvasGroup;      // PausePanel üzerindeki CanvasGroup
    public RectTransform content;        // PauseContent (scale animasyonu için)

    [Header("Animation")]
    public float animDuration = 0.2f;
    public float startScale = 0.92f;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    bool isPaused;
    bool isAnimating;

    void Awake()
    {
        // Güvenli başlangıç
        if (pausePanel) pausePanel.SetActive(false);
        if (canvasGroup) { canvasGroup.alpha = 0f; canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false; }
        if (content) content.localScale = Vector3.one;
        Time.timeScale = 1f;
        isPaused = false;
        isAnimating = false;
    }

    void Update()
    {
        // Mobil geri tuşu = Android Back, Unity’de KeyCode.Escape olarak gelir.
        // PC'de de ESC ile aynı çalışır.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void restartGame()
    {
        // Mevcut sahneyi yeniden yükle
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (isPaused || isAnimating) return;
        StartCoroutine(OpenPause());
    }

    public void ResumeGame()
    {
        if (!isPaused || isAnimating) return;
        StartCoroutine(ClosePause());
    }

    public void GoToMainMenu()
    {
        // Menüye dönmeden önce timeScale'ı geri al
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    IEnumerator OpenPause()
    {
        isAnimating = true;

        // Paneli aç, input'u kapat (animasyon bitene kadar)
        pausePanel.SetActive(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Oyunu durdur
        Time.timeScale = 0f;
        isPaused = true;

        float t = 0f;
        canvasGroup.alpha = 0f;
        content.localScale = Vector3.one * startScale;

        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / animDuration);

            // Ease-out
            float eased = 1f - Mathf.Pow(1f - k, 3f);

            canvasGroup.alpha = eased;
            content.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one, eased);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        content.localScale = Vector3.one;

        // Input'u aç
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        isAnimating = false;
    }

    IEnumerator ClosePause()
    {
        isAnimating = true;

        // Input'u kapat (kapanırken tıklanmasın)
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float t = 0f;
        float startAlpha = canvasGroup.alpha;
        Vector3 startS = content.localScale;

        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / animDuration);

            // Ease-in
            float eased = k * k * k;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, eased);
            content.localScale = Vector3.Lerp(startS, Vector3.one * startScale, eased);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        content.localScale = Vector3.one * startScale;

        // Paneli kapat
        pausePanel.SetActive(false);

        // Oyunu devam ettir
        Time.timeScale = 1f;
        isPaused = false;

        isAnimating = false;
    }
}
