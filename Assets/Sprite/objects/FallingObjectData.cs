using UnityEngine;

public enum ComboEffect
{
    Add,        // Combo +1
    Break,      // Combo = 0
    None        // Combo değişmesin
}

public class FallingObjectData : MonoBehaviour
{
    [Header("Score")]
    public int scoreDelta = 10;          // +10 iyi, -20 kötü

    [Header("Time")]
    public float timeDelta = 0.0f;       // +0.3 iyi, -1.5 kötü, +20 bonus

    [Header("Combo")]
    public ComboEffect comboEffect = ComboEffect.Add;

    [Header("Behavior")]
    public bool destroyOnPlayerHit = true;
    public bool destroyOnGroundHit = true;

    // İstersen: yere düşünce ceza uygula
    public int scoreDeltaOnGround = 0;   // kaçırınca -puan gibi
    public float timeDeltaOnGround = 0f; // kaçırınca -süre gibi
}
