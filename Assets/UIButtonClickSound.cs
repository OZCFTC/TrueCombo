using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonClickSound : MonoBehaviour, IPointerUpHandler
{
    [Header("Audio")]
    public AudioSource uiAudioSource;   // UIAudio objesindeki AudioSource
    public AudioClip clickClip;
    [Range(0f, 1f)] public float volume = 0.8f;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!uiAudioSource || !clickClip) return;
        uiAudioSource.PlayOneShot(clickClip, volume);
    }
}
