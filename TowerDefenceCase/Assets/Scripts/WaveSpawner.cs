using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Vector3 spawnPoint;
    public float offsetX = 1f;
    public float offsetZ = 1f;

    public ObjectPooler pooler;
    public WaveDataSO[] waveData;
    public GameObject BossWarning;
    public GameObject BaseEnemyPrefab;
    public TextMeshProUGUI cooldownText;

    public bool autoStart = true;

    public int enemiesRemaining;
    private int currentWaveIndex = -1;
    private WaveDataSO currentWaveData;
    private Coroutine spawnCoroutine;
    private Coroutine cooldownCoroutine;
    private bool isCooldownActive = false;
    private bool isWaveStarting = false;

    public event Action OnWaveCompleted;
    public event Action OnWavesEnd;

    private void Start()
    {
        if (autoStart) StartWaves();
    }

    public void StartWaves()
    {
        currentWaveIndex = -1;
        if (cooldownText != null) cooldownText.gameObject.SetActive(true);
        StartNextWaveCountdown();
    }

    private void StartNextWaveCountdown()
    {
        int nextIndex = currentWaveIndex + 1;
        if (BossWarning != null && waveData[nextIndex].IsBossWave) BossWarning.gameObject.SetActive(true);
        if (nextIndex >= waveData.Length)
        {
            OnWavesEnd?.Invoke();
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
            if (BossWarning != null) BossWarning.gameObject.SetActive(false);
            return;
        }
        if (cooldownCoroutine != null) StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = StartCoroutine(CooldownRoutine(nextIndex));
    }

    private IEnumerator CooldownRoutine(int nextWaveIndex)
    {
        isCooldownActive = true;
        isWaveStarting = false;

        float cooldown = Mathf.Max(0f, waveData[nextWaveIndex].waveCooldown);
        float timer = cooldown;

        while (timer > 0f)
        {
            if (isWaveStarting) yield break;
            if (cooldownText != null) cooldownText.text = $"Next wave in {Mathf.Ceil(timer)}s";
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!isWaveStarting)
        {
            StartWaveImmediate(nextWaveIndex);
        }

        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
        isCooldownActive = false;
        cooldownCoroutine = null;
    }

    public void SkipCooldown()
    {
        if (!isCooldownActive || isWaveStarting) return;
        isWaveStarting = true;
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = null;
        }
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
        StartWaveImmediate(currentWaveIndex + 1);
        isCooldownActive = false;
    }

    private void StartWaveImmediate(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveData.Length)
        {
            OnWavesEnd?.Invoke();
            return;
        }

        currentWaveIndex = waveIndex;
        currentWaveData = waveData[currentWaveIndex];

        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnWaveCoroutine(currentWaveData));
    }

    private IEnumerator SpawnWaveCoroutine(WaveDataSO wave)
    {
        enemiesRemaining = 0;

        int totalEnemies = 0;
        foreach (var it in wave.enemies) totalEnemies += it.count;
        if (totalEnemies <= 0)
        {
            OnWaveCompleted?.Invoke();
            StartNextWaveCountdown();
            yield break;
        }

        int spawned = 0;
        while (spawned < totalEnemies)
        {
            EnemyWaveItem itemType = GetWaveItemByIndex(spawned, wave);

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
            float delay = UnityEngine.Random.Range(wave.waveTimeMin, wave.waveTimeMax);
            yield return new WaitForSeconds(delay);
        }

        spawnCoroutine = null;
    }

    private EnemyWaveItem GetWaveItemByIndex(int index, WaveDataSO wave)
    {
        int counter = 0;
        foreach (var item in wave.enemies)
        {
            for (int j = 0; j < item.count; j++)
            {
                if (counter == index) return item;
                counter++;
            }
        }
        return wave.enemies[0];
    }

    private void HandleEnemyDead(GameObject enemyObj, bool spawnedFromBoss)
    {
        pooler.ReturnToPool(enemyObj, 2f);

        if (spawnedFromBoss)
            return;

        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (BossWarning != null && waveData[currentWaveIndex].IsBossWave) BossWarning.gameObject.SetActive(false);
            OnWaveCompleted?.Invoke();
            int nextIndex = currentWaveIndex + 1;
            if (nextIndex < waveData.Length)
            {
                // Her durumda cooldown başlat
                if (cooldownCoroutine != null) StopCoroutine(cooldownCoroutine);
                if (cooldownText != null) cooldownText.gameObject.SetActive(true);
                StartNextWaveCountdown();
            }
            else
            {
                OnWavesEnd?.Invoke();
            }
        }
    }
}
