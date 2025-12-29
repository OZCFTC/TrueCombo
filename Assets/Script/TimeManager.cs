using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager I;

    [Header("UI")]
    public Slider timeSlider;     // 0..1
    public TMP_Text timeText;     // opsiyonel
    public GameOverPanelController gameOverPanel;

    [Header("Time (30-45s average preset)")]
    public float maxTime = 18f;
    public float startTime = 10f;

    [Header("Drain (accelerates)")]
    public float baseDrainPerSec = 0.55f;
    public float drainIncreasePerSec = 0.055f;
    public float maxDrainPerSec = 2.40f;

    public float CurrentTime { get; private set; }

    float drain;
    bool isGameOver;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        CurrentTime = Mathf.Clamp(startTime, 0f, maxTime);
        drain = baseDrainPerSec;

        RefreshUI();
    }

    void Update()
    {
        if (isGameOver) return;

        drain = Mathf.Min(maxDrainPerSec, drain + drainIncreasePerSec * Time.deltaTime);

        CurrentTime -= drain * Time.deltaTime;
        if (CurrentTime <= 0f)
        {
            CurrentTime = 0f;
            RefreshUI();
            TimeOver();
            return;
        }

        RefreshUI();
    }

public void AddTime(float amount)
{
    if (isGameOver) return;

    CurrentTime += amount;              // negatifse düşer
    CurrentTime = Mathf.Clamp(CurrentTime, 0f, maxTime);

    RefreshUI();
}


    void RefreshUI()
    {
        if (timeSlider)
            timeSlider.value = (maxTime <= 0f) ? 0f : (CurrentTime / maxTime);

        if (timeText)
            timeText.text = $"{CurrentTime:0.0}s";
    }

    void TimeOver()
    {
        isGameOver = true;

        int finalScore = 0;
        int finalTotalCombos = 0;

        if (ScoreComboManager.I != null)
        {
            finalScore = ScoreComboManager.I.Score;
            finalTotalCombos = ScoreComboManager.I.TotalCombos;
        }

        // DEBUG: 0 mı geliyor, burada görürsün
        Debug.Log($"TIME OVER -> finalScore:{finalScore}, totalCombos:{finalTotalCombos}, ScoreMgrNull:{ScoreComboManager.I == null}");

        if (gameOverPanel != null)
            gameOverPanel.Show(finalScore, finalTotalCombos);
        else
            Debug.LogError("TimeManager: gameOverPanel is NOT assigned!");

        Time.timeScale = 0f;
    }
}
