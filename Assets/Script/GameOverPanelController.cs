using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverPanelController : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text scoreText;
    public TMP_Text totalComboText;

    [Header("Count-Up")]
    public float scoreCountDuration = 0.9f;
    public float comboCountDuration = 0.6f;
    public bool useThousandsSeparator = true;

    Coroutine routine;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(int finalScore, int finalTotalCombos)
    {
        // Referanslar boşsa direkt anlayalım
        if (scoreText == null || totalComboText == null)
        {
            Debug.LogError("GameOverPanelController: scoreText / totalComboText is NOT assigned in Inspector!");
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(true);

        SetScore(0);
        SetCombos(0);

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CountUp(finalScore, finalTotalCombos));
    }

    IEnumerator CountUp(int targetScore, int targetCombos)
    {
        // SCORE
        float t = 0f;
        while (t < scoreCountDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / scoreCountDuration);
            k = k * k * (3f - 2f * k); // smooth

            int v = Mathf.RoundToInt(Mathf.Lerp(0, targetScore, k));
            SetScore(v);
            yield return null;
        }
        SetScore(targetScore);

        // COMBOS
        t = 0f;
        while (t < comboCountDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / comboCountDuration);
            k = k * k * (3f - 2f * k);

            int v = Mathf.RoundToInt(Mathf.Lerp(0, targetCombos, k));
            SetCombos(v);
            yield return null;
        }
        SetCombos(targetCombos);

        routine = null;
    }

    void SetScore(int value)
    {
        string v = useThousandsSeparator ? value.ToString("N0") : value.ToString();
        scoreText.text = $"SCORE\n{v}";
    }

    void SetCombos(int value)
    {
        string v = useThousandsSeparator ? value.ToString("N0") : value.ToString();
        totalComboText.text = $"TOTAL COMBOS\n{v}";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
