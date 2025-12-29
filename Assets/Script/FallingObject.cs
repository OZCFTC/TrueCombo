using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public string playerTag = "Player";
    public string groundTag = "Zemin";

    FallingObjectData data;

    void Awake()
    {
        data = GetComponent<FallingObjectData>();
        if (data == null)
            Debug.LogError($"{name}: FallingObjectData yok! Prefab'a ekle.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (data == null) return;

        if (other.CompareTag(playerTag))
        {
            // 1) Combo etkisi
            if (ScoreComboManager.I != null)
            {
                switch (data.comboEffect)
                {
                    case ComboEffect.Add:
                        ScoreComboManager.I.OnHitWithCustom(data.scoreDelta, data.timeDelta);
                        break;

                    case ComboEffect.Break:
                        ScoreComboManager.I.OnBreakWithPenalty(data.scoreDelta, data.timeDelta);
                        break;

                    case ComboEffect.None:
                        ScoreComboManager.I.ApplyCustomNoCombo(data.scoreDelta, data.timeDelta);
                        break;
                }
            }
            else
            {
                // Manager yoksa bile en azından time eklemeyi dene
                TimeManager.I?.AddTime(data.timeDelta);
            }

            if (data.destroyOnPlayerHit)
                Destroy(gameObject);

            return;
        }

        if (other.CompareTag(groundTag))
        {
            // yere düştüğünde ceza/etki
            if (ScoreComboManager.I != null)
            {
                if (data.scoreDeltaOnGround != 0 || Mathf.Abs(data.timeDeltaOnGround) > 0.0001f)
                    ScoreComboManager.I.ApplyCustomNoCombo(data.scoreDeltaOnGround, data.timeDeltaOnGround);

                // çoğu oyunda kaçırınca combo kırılır
                ScoreComboManager.I.OnMiss();
            }

            if (data.destroyOnGroundHit)
                Destroy(gameObject);
        }
    }
}
