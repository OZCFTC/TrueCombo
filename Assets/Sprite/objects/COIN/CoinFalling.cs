using UnityEngine;

public class CoinFalling : MonoBehaviour
{
    [Header("Coin")]
    public int coinValue = 1;

    public string playerTag = "Player";
    public string groundTag = "Zemin";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            CoinManager.I?.AddCoins(coinValue);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag(groundTag))
        {
            Destroy(gameObject);
        }
    }
}
