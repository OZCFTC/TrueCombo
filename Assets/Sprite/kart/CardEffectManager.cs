using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager I;

    CardType? activeBuff;

    Transform playerTf;

    // DoubleSize için: her aktivasyonda yeniden kaydedilecek
    Vector3 sizeBaseScale;
    bool sizeBuffActive;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    void OnEnable()
    {
        if (BuffTimerManager.I != null)
            BuffTimerManager.I.OnTimerEnded += OnBuffEnded;
    }

    void OnDisable()
    {
        if (BuffTimerManager.I != null)
            BuffTimerManager.I.OnTimerEnded -= OnBuffEnded;
    }

    public void Activate(CardData card)
    {
        if (card == null) return;

        // önce varsa önceki buffı temizle
        RestoreAll();

        activeBuff = card.type;

        // timer başlat
        BuffTimerManager.I?.StartTimer(card.duration);

        switch (card.type)
        {
            case CardType.DoubleScorePlusTime30:
                TimeManager.I?.AddTime(30f);
                ScoreComboManager.I?.SetScoreMultiplier(2);
                break;

            case CardType.DoubleCoin:
                CoinManager.I?.SetMultiplier(2);
                break;

            case CardType.DoubleSize:
                CachePlayer();
                if (playerTf != null)
                {
                    // ✅ her seferinde o anki scale'i base olarak kaydet
                    sizeBaseScale = playerTf.localScale;
                    sizeBuffActive = true;

                    playerTf.localScale = sizeBaseScale * 2f;
                }
                break;
        }
    }

    void OnBuffEnded()
    {
        RestoreAll();
        BuffTimerManager.I?.StopTimer();
    }

    void RestoreAll()
    {
        // buff yoksa da DoubleSize aktif kalmış olabilir → yine düzelt
        ScoreComboManager.I?.SetScoreMultiplier(1);
        CoinManager.I?.SetMultiplier(1);

        // ✅ DoubleSize geri dönüş
        if (sizeBuffActive)
        {
            CachePlayer(); // player değiştiyse yeniden bul
            if (playerTf != null)
                playerTf.localScale = sizeBaseScale;

            sizeBuffActive = false;
        }

        activeBuff = null;
    }

    void CachePlayer()
    {
        if (playerTf != null) return;

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogError("Player tag'li obje yok! (DoubleSize çalışmaz)");
            return;
        }
        playerTf = p.transform;
    }
}
