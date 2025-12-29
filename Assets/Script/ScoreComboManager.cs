using UnityEngine;
using TMPro;

public class ScoreComboManager : MonoBehaviour
{
    public static ScoreComboManager I;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text multiplierText;

    [Header("Combo Colors")]
    public Color tier1Color = Color.white;
    public Color tier2Color = new Color(0.5f, 1f, 0.6f);
    public Color tier3Color = new Color(0.5f, 0.8f, 1f);
    public Color tier4Color = new Color(1f, 0.85f, 0.4f);
    public Color tier5Color = new Color(1f, 0.45f, 0.65f);

    [Header("Scoring")]
    public int basePoints = 10;

    [Header("Time Gain (normal hit)")]
    public float addTimePerHit = 1f;

    [Header("Shake (combo only)")]
    public float hitShakeDuration = 0.06f;
    public float hitShakeStrength = 0.06f;
    public float tierUpShakeDuration = 0.10f;
    public float tierUpShakeStrength = 0.12f;

    public int Score { get; private set; }
    public int Combo { get; private set; }
    public int TotalCombos { get; private set; }

    int scoreMultiplier = 1; // ✅ 2X kart bununla çalışacak

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        RefreshUI();
    }

    public void SetScoreMultiplier(int m)
    {
        scoreMultiplier = Mathf.Max(1, m);
    }

    public int GetMultiplier()
    {
        if (Combo >= 35) return 5;
        if (Combo >= 20) return 4;
        if (Combo >= 10) return 3;
        if (Combo >= 5) return 2;
        return 1;
    }

    public void OnHit()
    {
        int beforeM = GetMultiplier();

        Combo++;
        TotalCombos++;

        int afterM = GetMultiplier();

        // ✅ HER OBJE 2X SKOR: scoreMultiplier burada
        Score += basePoints * afterM * scoreMultiplier;

        TimeManager.I?.AddTime(addTimePerHit);

        DoComboShake(beforeM, afterM);
        RefreshUI();
    }

    // Custom objeler kullanıyorsan:
    public void OnHitWithCustom(int scoreDelta, float timeDelta)
    {
        int beforeM = GetMultiplier();

        Combo++;
        TotalCombos++;

        int afterM = GetMultiplier();

        // pozitif skorlar 2X etkilenir, negatif ceza etkilenmez (daha adil)
        int appliedScore = (scoreDelta >= 0)
            ? scoreDelta * afterM * scoreMultiplier
            : scoreDelta;

        Score += appliedScore;

        if (timeDelta != 0f) TimeManager.I?.AddTime(timeDelta);

        DoComboShake(beforeM, afterM);
        RefreshUI();
    }

    public void OnMiss()
    {
        Combo = 0;
        RefreshUI();
    }

    void DoComboShake(int beforeM, int afterM)
    {
        if (Combo <= 0) return;
        if (CameraShake.I == null) return;

        CameraShake.I.Shake(hitShakeDuration, hitShakeStrength);
        if (afterM > beforeM)
            CameraShake.I.Shake(tierUpShakeDuration, tierUpShakeStrength);
    }

    public void RefreshUI()
    {
        if (scoreText) scoreText.text = $"SCORE: {Score}";

        bool active = Combo > 0;
        int m = GetMultiplier();

        if (comboText)
        {
            comboText.gameObject.SetActive(active);
            if (active) comboText.text = $"COMBO {Combo}";
        }

        if (multiplierText)
        {
            multiplierText.gameObject.SetActive(active);
            if (active) multiplierText.text = $"x{m}";
        }

        if (active)
        {
            Color c = GetTierColor(m);
            if (comboText) comboText.color = c;
            if (multiplierText) multiplierText.color = c;
        }
    }

    Color GetTierColor(int m)
    {
        return m switch
        {
            2 => tier2Color,
            3 => tier3Color,
            4 => tier4Color,
            _ => (m >= 5 ? tier5Color : tier1Color)
        };
    }
    // ✅ Kötü obje: combo kır + ceza uygula (score/time negatif olabilir)
public void OnBreakWithPenalty(int scoreDelta, float timeDelta)
{
    Combo = 0;

    if (scoreDelta != 0)
        Score += scoreDelta;

    if (timeDelta != 0f)
        TimeManager.I?.AddTime(timeDelta);

    RefreshUI();
}

// ✅ Combo değiştirmeyen objeler (bonus gibi)
public void ApplyCustomNoCombo(int scoreDelta, float timeDelta)
{
    if (scoreDelta != 0)
        Score += scoreDelta;

    if (timeDelta != 0f)
        TimeManager.I?.AddTime(timeDelta);

    RefreshUI();
}

}
