using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    [Header("Rules")]
    public int requiredCoins = 10;

    [Header("Cards")]
    public CardData[] allCards;

    [Header("UI")]
    public GameObject panel;
    public CardButtonUI leftCard;
    public CardButtonUI rightCard;

    bool choosing;

    void Awake()
    {
        if (panel) panel.SetActive(false);
    }

    void OnEnable()
    {
        if (CoinManager.I != null)
            CoinManager.I.OnCoinsChanged += OnCoinsChanged;
    }

    void OnDisable()
    {
        if (CoinManager.I != null)
            CoinManager.I.OnCoinsChanged -= OnCoinsChanged;
    }

    void Start()
    {
        if (CoinManager.I != null)
            OnCoinsChanged(CoinManager.I.Coins);
    }

    void Update()
    {
        // event kaçarsa diye fallback
        if (choosing) return;
        if (panel != null && panel.activeSelf) return;

        if (CoinManager.I != null && CoinManager.I.Coins >= requiredCoins)
            Open();
    }

    void OnCoinsChanged(int coins)
    {
        if (choosing) return;
        if (panel != null && panel.activeSelf) return;
        if (coins < requiredCoins) return;

        Open();
    }

    void Open()
    {
        if (choosing) return;

        if (allCards == null || allCards.Length < 2)
        {
            Debug.LogError("CardSelectionManager: allCards en az 2 olmalı!");
            return;
        }

        if (panel == null || leftCard == null || rightCard == null)
        {
            Debug.LogError("CardSelectionManager: panel/leftCard/rightCard bağla!");
            return;
        }

        if (CoinManager.I == null || !CoinManager.I.TrySpend(requiredCoins))
            return;

        choosing = true;
        Time.timeScale = 0f;

        CardData a = allCards[Random.Range(0, allCards.Length)];
        CardData b = a;
        int guard = 0;
        while (b == a && guard++ < 50)
            b = allCards[Random.Range(0, allCards.Length)];

        panel.SetActive(true);
        leftCard.Setup(a, OnSelected);
        rightCard.Setup(b, OnSelected);
    }

    void OnSelected(CardData chosen)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        choosing = false;

        CardEffectManager.I?.Activate(chosen);

        if (CoinManager.I != null)
            OnCoinsChanged(CoinManager.I.Coins);
    }
}
