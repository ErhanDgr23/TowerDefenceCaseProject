using UnityEngine;
using System;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Point")]
    public Vector3 spawnPoint;

    [Header("Spawn Offset")]
    public float offsetX = 1f;
    public float offsetZ = 1f;

    [Header("Pooling")]
    public ObjectPooler pooler;

    [Header("Wave Data")]
    public WaveDataSO waveData;

    [Header("Other")]
    public GameObject BaseEnemyPrefab;

    private int enemiesRemaining;
    public event Action OnWaveCompleted;

    public void StartWave()
    {
        if (waveData == null || pooler == null) return;
        StopAllCoroutines();
        StartCoroutine(SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        enemiesRemaining = 0;

        int totalEnemies = 0;
        foreach (var item in waveData.enemies)
            totalEnemies += item.count;

        int spawned = 0;

        while (spawned < totalEnemies)
        {
            EnemyWaveItem itemType = GetWaveItemByIndex(spawned);

            float randomX = UnityEngine.Random.Range(-offsetX, offsetX);
            float randomZ = UnityEngine.Random.Range(-offsetZ, offsetZ);
            Vector3 spawnPos = spawnPoint + new Vector3(randomX, 0f, randomZ);

            GameObject enemy = pooler.SpawnFromPool(BaseEnemyPrefab, spawnPos, Quaternion.identity);
            EnemyManager enemyScript = enemy.GetComponent<EnemyManager>();
            if (enemyScript != null)
            {
                enemyScript.Type = itemType.enemyType;
                enemyScript.InitEnemy();

                enemyScript.OnDead -= HandleEnemyDead;
                enemyScript.OnDead += HandleEnemyDead;

                enemiesRemaining++;
            }

            spawned++;

            float delay = UnityEngine.Random.Range(waveData.waveTimeMin, waveData.waveTimeMax);
            yield return new WaitForSeconds(delay);
        }
    }

    private EnemyWaveItem GetWaveItemByIndex(int index)
    {
        int counter = 0;
        foreach (var item in waveData.enemies)
        {
            for (int j = 0; j < item.count; j++)
            {
                if (counter == index) return item;
                counter++;
            }
        }
        return waveData.enemies[0];
    }

    private void HandleEnemyDead(GameObject enemyObj)
    {
        pooler.ReturnToPool(enemyObj, 2f);

        enemiesRemaining--;
        if (enemiesRemaining <= 0)
            OnWaveCompleted?.Invoke();
    }
}
