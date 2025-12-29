using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public CardType type;
    public Sprite cardSprite;

    [Min(0)] public int price = 10;
    [Min(0f)] public float duration = 10f; // hepsi 10sn (sen böyle istedin)
}
