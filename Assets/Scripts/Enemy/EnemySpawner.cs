using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyList;
    public float spawnRadius = 15f;
    public int mapType = 0;
    [SerializeField] Bounds bounds;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private float circleScale;
    private GameObject levelManager;
    private GameObject[] enemyPrefabs;
    private float[] enemySpawnRate;
    private int[] enemySpawnCount;
    private GameObject[] miniBossPrefabs;
    private GameObject[] bossPrefabs;
    private GameObject player;
    private GameObject timer;
    private int index;
    private int[] indices;
    private bool isSpawning = false;
    private int maxCapacity;
    private float spawnRate;
    private int spawnCount;
    private float elapsedTime = 0.0f;
    private bool spawnMobs = false;

    void Awake()
    {
        enemyList = new List<GameObject>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        levelManager = GameObject.FindWithTag("LevelManager");
        timer = GameObject.FindWithTag("Timer");
        enemyPrefabs = levelManager.GetComponent<LevelManager>().enemyPrefabs;
        miniBossPrefabs = levelManager.GetComponent<LevelManager>().miniBossPrefabs;
        bossPrefabs = levelManager.GetComponent<LevelManager>().bossPrefabs;
        enemySpawnRate = levelManager.GetComponent<LevelManager>().enemySpawnRate;
        enemySpawnCount = levelManager.GetComponent<LevelManager>().spawnCount;
        maxCapacity = levelManager.GetComponent<LevelManager>().maxCapacity;
        StartCoroutine(WinGame());
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnRate && isSpawning && enemyList.Count < maxCapacity)
        {
            if (!spawnMobs)
            {
                elapsedTime = 0.0f;
                SpawnMob(index);
            }
            else
            {
                elapsedTime = 0.0f;
                SpawnMobs(indices);
            }
        }
    }

    IEnumerator SpawnFirstWave()
    {
        SaveGame();
        isSpawning = true;
        spawnMobs = false;
        if (timer.GetComponent<TimerScript>().gameTimer >= 55.0f) isSpawning = false;
        spawnRate = enemySpawnRate[0];
        spawnCount = enemySpawnCount[0];
        index = 0;
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 60.0f);
    }

    IEnumerator SpawnSecondWave()
    {
        yield return SpawnFirstWave();
        SaveGame();
        SpawnMiniBoss(0);
        isSpawning = true;
        if (timer.GetComponent<TimerScript>().gameTimer >= 115.0f) isSpawning = false;
        spawnRate = enemySpawnRate[1];
        spawnCount = enemySpawnCount[1];
        index = 1;
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 120.0f);
    }

    IEnumerator SpawnFirstBoss()
    {
        yield return SpawnSecondWave();
        SaveGame();
        isSpawning = false;
        timer.GetComponent<TimerScript>().StopTimer();
        timer.SetActive(false);
        StartCoroutine(SpawnBoss(0));
        yield return new WaitUntil(() => enemyList.Count == 0);
    }

    IEnumerator SpawnThirdWave()
    {
        yield return SpawnFirstBoss();
        SaveGame();
        timer.GetComponent<TimerScript>().ResumeTimer();
        timer.SetActive(true);
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 122.0f);
        isSpawning = true;
        spawnMobs = true;
        indices = new int[] { 0, 1 };
        if (timer.GetComponent<TimerScript>().gameTimer >= 175.0f) isSpawning = false;
        spawnRate = enemySpawnRate[2];
        spawnCount = enemySpawnCount[2];
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 180.0f);
    }

    IEnumerator SpawnFourthWave()
    {
        yield return SpawnThirdWave();
        SaveGame();
        SpawnMiniBoss(1);
        isSpawning = true;
        if (timer.GetComponent<TimerScript>().gameTimer >= 235.0f) isSpawning = false;
        spawnRate = enemySpawnRate[3];
        spawnCount = enemySpawnCount[3];
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 240.0f);
    }

    IEnumerator SpawnSecondBoss()
    {
        yield return SpawnFourthWave();
        SaveGame();
        isSpawning = false;
        timer.GetComponent<TimerScript>().StopTimer();
        timer.SetActive(false);
        StartCoroutine(SpawnBoss(1));
        yield return new WaitUntil(() => enemyList.Count == 0);
    }

    IEnumerator WinGame()
    {
        yield return SpawnSecondBoss();
        GameManager.Instance.WinGame();
    }

    void SpawnMob(int index)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            if (mapType == 0)
                spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
            GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
            enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
            enemy.tag = "Enemy";
            enemyList.Add(enemy);
        }
    }

    void SpawnMobs(int[] indices)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int index = indices[Random.Range(0, indices.Length)];
            Vector3 spawnPosition = Vector3.zero;
            if (mapType == 0)
                spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
            GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
            enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
            enemy.tag = "Enemy";
            enemyList.Add(enemy);
        }
    }

    void SpawnMiniBoss(int index)
    {
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius / 2;
        GameObject enemy = Instantiate(miniBossPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.tag = "Enemy";
        enemyList.Add(enemy);
    }

    IEnumerator SpawnBoss(int index)
    {
        if (enemyList.Count > 0) DeleteAllEnemies();
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + new Vector3(0, 5f, 0);

        // Spawn the boss and disable it
        GameObject boss = Instantiate(bossPrefabs[index], spawnPosition, Quaternion.identity, transform);
        boss.GetComponent<BossBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        boss.tag = "Boss";
        enemyList.Add(boss);
        boss.SetActive(false);

        // Spawn the effect and destroy it after 2 seconds
        GameObject effect = Object.Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
        effect.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        Destroy(effect, 2f);

        // Wait 2 seconds before spawning the skill
        yield return new WaitForSeconds(2f);
        boss.SetActive(true);
    }

    public void DeleteEnemy(GameObject enemy)
    {
        YomiyaUltiMark YomiyaMark = enemy.GetComponent<YomiyaUltiMark>();
        if (YomiyaMark != null)
        {
            YomiyaMark.UseMarkSkill();
            Destroy(YomiyaMark.burstMark);
        }
        
        enemyList.Remove(enemy);
        Destroy(enemy);
    }

    public void DeleteAllEnemies()
    {
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }
        enemyList.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 position)
    {
        if (enemyList.Count == 0) return null;
        GameObject closestEnemy = enemyList[0];
        float closestDistance = Vector3.Distance(position, closestEnemy.transform.position);
        for (int i = 1; i < enemyList.Count; i++)
        {
            float distance = Vector2.Distance(position, enemyList[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyList[i];
            }
        }
        return closestEnemy;
    }

    void SaveGame()
    {
        // Save game
        float time = timer.GetComponent<TimerScript>().gameTimer;
        PlayerBehavior player = FindObjectOfType<PlayerBehavior>();
        int health = player.currentHealth;
        GameManager.Instance.saveManager.SaveGameBinary(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, time, health);
    }
}
