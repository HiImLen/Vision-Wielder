using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyList;
    public float spawnRadius = 15f;
    private GameObject levelManager;
    private GameObject[] enemyPrefabs;
    private float[] enemySpawnRate;
    private GameObject[] miniBossPrefabs;
    private GameObject[] bossPrefabs;
    private GameObject player;
    private GameObject timer;
    private int index;
    private bool isSpawning = false;
    private float spawnRate;
    private float elapsedTime = 0.0f;

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
        StartCoroutine(SpawnFourthWave());
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnRate && isSpawning)
        {
            elapsedTime = 0.0f;
            SpawnMob(index);
        }
    }

    IEnumerator SpawnFirstWave()
    {
        isSpawning = true;
        spawnRate = enemySpawnRate[0];
        index = 0;
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 60.0f);
    }

    IEnumerator SpawnSecondWave()
    {
        yield return SpawnFirstWave();
        isSpawning = true;
        spawnRate = enemySpawnRate[1];
        index = 1;
        yield return new WaitUntil(() => timer.GetComponent<TimerScript>().gameTimer >= 120.0f);
    }

    IEnumerator SpawnFirstBoss()
    {
        yield return SpawnSecondWave();
        isSpawning = false;
        timer.GetComponent<TimerScript>().StopTimer();
        timer.SetActive(false);
        SpawnBoss(0);
    }

    IEnumerator SpawnThirdWave()
    {
        yield return SpawnFirstBoss();
    }

    IEnumerator SpawnFourthWave()
    {
        yield return SpawnThirdWave();
    }

    IEnumerator SpawnSecondBoss()
    {
        yield return SpawnFourthWave();
    }

    void SpawnMiniBoss(int index)
    {
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
        GameObject enemy = Instantiate(miniBossPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.tag = "Enemy";
        enemyList.Add(enemy);
    }

    void SpawnMob(int index)
    {
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
        GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.tag = "Enemy";
        enemyList.Add(enemy);
    }

    void SpawnMobs(int[] index)
    {
        foreach (int i in index)
        {
            SpawnMob(i);
        }
    }

    void SpawnBoss(int index)
    {
        if (enemyList.Count > 0) DeleteAllEnemies();
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + new Vector3(0, 5f, 0);
        GameObject enemy = Instantiate(bossPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<BossBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.tag = "Boss";
        enemyList.Add(enemy);
    }

    public void DeleteEnemy(GameObject enemy)
    {
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
            float distance = Vector3.Distance(position, enemyList[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyList[i];
            }
        }
        return closestEnemy;
    }
}
