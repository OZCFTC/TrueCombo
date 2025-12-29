using UnityEngine;

public class TimeBonusObject : MonoBehaviour
{
    [Header("Settings")]
    public float bonusTime = 20f;   // +20 saniye

    public string playerTag = "Player";
    public string groundTag = "Zemin";

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player aldıysa
        if (other.CompareTag(playerTag))
        {
            if (TimeManager.I != null)
            {
                TimeManager.I.AddTime(bonusTime);
            }

            Destroy(gameObject);
            return;
        }

        // Yere düşerse yok olsun (boşa gitmesin)
        if (other.CompareTag(groundTag))
        {
            Destroy(gameObject);
        }
    }
}
