using UnityEngine;
using UnityEngine.UI;

public class CardDurationUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup cg;

    [Header("Pulse")]
    public float pulseStartTime = 3f;
    public float pulseSpeed = 6f;
    public Color normalColor = Color.white;
    public Color pulseColor = Color.red;

    void Awake()
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0f;
            slider.interactable = false;
        }
        Hide();
    }

    void OnEnable()
    {
        if (BuffTimerManager.I != null)
        {
            BuffTimerManager.I.OnTimerChanged += OnTimerChanged;
            BuffTimerManager.I.OnTimerEnded += OnTimerEnded;
        }
    }

    void OnDisable()
    {
        if (BuffTimerManager.I != null)
        {
            BuffTimerManager.I.OnTimerChanged -= OnTimerChanged;
            BuffTimerManager.I.OnTimerEnded -= OnTimerEnded;
        }
    }

    void OnTimerChanged(float timeLeft, float duration)
    {
        if (slider == null || cg == null) return;

        if (duration <= 0.01f)
        {
            Hide();
            return;
        }

        Show();

        float normalized = Mathf.Clamp01(timeLeft / duration);
        slider.value = normalized;

        // son 3 saniye pulse
        if (fillImage != null)
        {
            if (timeLeft <= pulseStartTime)
            {
                float p = Mathf.Abs(Mathf.Sin(Time.unscaledTime * pulseSpeed));
                fillImage.color = Color.Lerp(normalColor, pulseColor, p);
            }
            else
            {
                fillImage.color = normalColor;
            }
        }

        if (timeLeft <= 0f) Hide();
    }

    void OnTimerEnded()
    {
        Hide();
    }

    void Show()
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    void Hide()
    {
        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;

        if (slider != null) slider.value = 0f;
        if (fillImage != null) fillImage.color = normalColor;
    }
}
