using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIButtonAnimator : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler,
    ISelectHandler, IDeselectHandler
{
    [Header("Scales")]
    public float normalScale = 1f;
    public float hoverScale = 1.06f;   // PC hover / gamepad select
    public float pressedScale = 0.94f; // basılıyken

    [Header("Timings (seconds)")]
    public float hoverTime = 0.10f;
    public float pressTime = 0.06f;
    public float releaseTime = 0.08f;

    [Header("Optional Punch")]
    public bool punchOnClick = true;
    public float punchScale = 1.10f;
    public float punchTime = 0.08f;

    RectTransform rt;
    Coroutine anim;

    bool isHover;
    bool isPressed;
    bool isSelected; // keyboard/gamepad

    void Awake()
    {
        rt = (RectTransform)transform;
        rt.localScale = Vector3.one * normalScale;
    }

    void OnDisable()
    {
        if (anim != null) StopCoroutine(anim);
        rt.localScale = Vector3.one * normalScale;
        isHover = isPressed = isSelected = false;
    }

    float TargetScale()
    {
        if (isPressed) return pressedScale;
        if (isHover || isSelected) return hoverScale;
        return normalScale;
    }

    void AnimateTo(float target, float duration)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(ScaleRoutine(target, duration));
    }

    IEnumerator ScaleRoutine(float target, float duration)
    {
        Vector3 start = rt.localScale;
        Vector3 end = Vector3.one * target;

        if (duration <= 0f)
        {
            rt.localScale = end;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // pause'da da çalışsın
            float k = Mathf.Clamp01(t / duration);
            // smoothstep
            k = k * k * (3f - 2f * k);

            rt.localScale = Vector3.LerpUnclamped(start, end, k);
            yield return null;
        }
        rt.localScale = end;
    }

    IEnumerator PunchRoutine()
    {
        // hızlı büyüt-küçült
        yield return ScaleRoutine(punchScale, punchTime);
        yield return ScaleRoutine(TargetScale(), releaseTime);
    }

    // PC hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        AnimateTo(TargetScale(), hoverTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        if (!isPressed) AnimateTo(TargetScale(), hoverTime);
    }

    // Mobil + PC basma
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        AnimateTo(TargetScale(), pressTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        AnimateTo(TargetScale(), releaseTime);

        if (punchOnClick)
        {
            if (anim != null) StopCoroutine(anim);
            anim = StartCoroutine(PunchRoutine());
        }
    }

    // Keyboard/Gamepad selection
    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        AnimateTo(TargetScale(), hoverTime);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        AnimateTo(TargetScale(), hoverTime);
    }
}
