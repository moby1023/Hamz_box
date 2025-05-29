using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<GameObject> enemyPrefabs;
        public int enemyCount;
        public float spawnRate;
    }

    [Header("Wave Settings")]
    public List<Wave> waves = new List<Wave>();
    public float delayBetweenWaves = 5f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI subText;

    [Header("Countdown Settings")]
    public float startCountdownTime = 60f;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartCountdownThenSpawn(startCountdownTime));
    }

    IEnumerator StartCountdownThenSpawn(float countdownTime)
    {
        float timeLeft = countdownTime;

        if (subText != null)
            subText.text = "Protect the survival!";

        while (timeLeft > 0)
        {
            if (countdownText != null)
                countdownText.text = $"Horde will arrive in {Mathf.CeilToInt(timeLeft)}";

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (countdownText != null)
            countdownText.text = "";

        if (subText != null)
            subText.text = "";

        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("✅ ทุก Wave ผ่านเรียบร้อยแล้ว!");
            if (countdownText != null)
                countdownText.text = "ALL WAVES CLEARED!";
            if (subText != null)
                subText.text = "";
            yield break;
        }

        isSpawning = true;
        Wave wave = waves[currentWaveIndex];

        // แสดงข้อความ WAVE
        if (countdownText != null)
            countdownText.text = $"WAVE {currentWaveIndex + 1}";

        if (subText != null)
            subText.text = "";

        Debug.Log($"🌊 เริ่ม Wave {currentWaveIndex + 1}: {wave.waveName}");

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        isSpawning = false;

        // รอจนศัตรูหมด
        while (enemiesAlive > 0)
        {
            yield return null;
        }

        currentWaveIndex++;

        // INTERMISSION พร้อมนับถอยหลัง
        float timer = delayBetweenWaves;

        if (subText != null)
            subText.text = "";

        while (timer > 0)
        {
            if (countdownText != null)
                countdownText.text = $"INTERMISSION... {Mathf.CeilToInt(timer)}";

            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        if (countdownText != null)
            countdownText.text = "";

        StartCoroutine(SpawnNextWave());
    }

    void SpawnEnemy(Wave wave)
    {
        if (wave.enemyPrefabs.Count == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        enemiesAlive++;

        AiChase ai = enemy.GetComponent<AiChase>();
        if (ai != null)
        {
            ai.OnDeath += OnEnemyKilled;
        }
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
