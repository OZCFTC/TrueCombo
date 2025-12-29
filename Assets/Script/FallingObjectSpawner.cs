using System.Collections;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnItem
    {
        public GameObject prefab;
        [Tooltip("Ne kadar yüksekse o kadar sık çıkar")]
        public int weight = 10;
    }

    [Header("Spawn Objects (Weighted)")]
    public SpawnItem[] spawnItems;

    [Header("Spawn Area")]
    public float minX = -4f;
    public float maxX = 4f;
    public float spawnY = 6f;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 1.5f;

    [Header("Optional")]
    public Transform parent;

    int totalWeight;

    void Start()
    {
        CalculateTotalWeight();
        StartCoroutine(SpawnRoutine());
    }

    void CalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var item in spawnItems)
        {
            if (item.prefab != null && item.weight > 0)
                totalWeight += item.weight;
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            Spawn();
        }
    }

    void Spawn()
    {
        if (spawnItems == null || spawnItems.Length == 0 || totalWeight <= 0)
            return;

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var item in spawnItems)
        {
            if (item.prefab == null || item.weight <= 0) continue;

            currentWeight += item.weight;

            if (randomValue < currentWeight)
            {
                float x = Random.Range(minX, maxX);
                Vector3 pos = new Vector3(x, spawnY, 0f);

                GameObject obj = Instantiate(item.prefab, pos, Quaternion.identity);

                if (parent != null)
                    obj.transform.SetParent(parent);

                return;
            }
        }
    }
}
