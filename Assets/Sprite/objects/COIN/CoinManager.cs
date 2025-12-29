using UnityEngine;
using TMPro;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager I;

    [SerializeField] private TMP_Text coinText;

    public int Coins { get; private set; }
    public event Action<int> OnCoinsChanged;

    int coinMultiplier = 1;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        RefreshUI();
        OnCoinsChanged?.Invoke(Coins);
    }

    public void SetMultiplier(int m)
    {
        coinMultiplier = Mathf.Max(1, m);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        Coins += amount * coinMultiplier;
        RefreshUI();
        OnCoinsChanged?.Invoke(Coins);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (Coins < amount) return false;

        Coins -= amount;
        RefreshUI();
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }

    void RefreshUI()
    {
        if (coinText) coinText.text = $"{Coins}";
    }
}
