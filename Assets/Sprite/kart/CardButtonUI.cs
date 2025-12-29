using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardButtonUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button button;

    CardData data;
    Action<CardData> onPick;

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
    }

    public void Setup(CardData card, Action<CardData> onSelected)
    {
        data = card;
        onPick = onSelected;

        if (cardImage) cardImage.sprite = card.cardSprite;
        if (priceText) priceText.text = card.price.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onPick?.Invoke(data));
    }
}
