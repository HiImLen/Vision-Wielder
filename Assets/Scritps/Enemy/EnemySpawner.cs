using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    private GameObject player;
    [SerializeField] public List<GameObject> enemyList;
    public float spawnRadius = 15f;
    public float secondsBetweenSpawn = 0.2f;
    private float elapsedTime = 0.0f;

    void Awake()
    {
        enemyList = new List<GameObject>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= secondsBetweenSpawn)
        {
            elapsedTime = 0.0f;
            SpawnMobs();
        }
    }

    void SpawnMobs()
    {
        Vector3 spawnPosition;
        spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.tag = "Enemy";
        enemyList.Add(enemy);
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy);
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
