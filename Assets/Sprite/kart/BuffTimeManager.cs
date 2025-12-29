using UnityEngine;
using System;

public class BuffTimerManager : MonoBehaviour
{
    public static BuffTimerManager I;

    public event Action<float, float> OnTimerChanged; // timeLeft, duration
    public event Action OnTimerEnded;

    float duration;
    float timeLeft;
    bool running;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.unscaledDeltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        OnTimerChanged?.Invoke(timeLeft, duration);

        if (timeLeft <= 0f)
        {
            running = false;               // ✅ önce durdur
            OnTimerEnded?.Invoke();        // ✅ sonra haber ver
        }
    }

    public void StartTimer(float seconds)
    {
        duration = Mathf.Max(0.01f, seconds);
        timeLeft = duration;
        running = true;

        OnTimerChanged?.Invoke(timeLeft, duration);
    }

    public void StopTimer()
    {
        running = false;
        duration = 0f;
        timeLeft = 0f;

        OnTimerChanged?.Invoke(0f, 0f);   // UI kapansın
    }

    public bool IsRunning() => running;
}
