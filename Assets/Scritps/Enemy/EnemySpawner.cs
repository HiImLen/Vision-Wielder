using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    private GameObject player;
    private GameObject grassTilemap;
    [SerializeField] private List<GameObject> enemyList;
    public float spawnRadius = 15f;
    public float secondsBetweenSpawn = 0.2f;
    private float elapsedTime = 0.0f;
    Bounds bounds;

    void Awake()
    {
        enemyList = new List<GameObject>();
        player = GameObject.FindWithTag("Player");
        grassTilemap = GameObject.FindWithTag("GrassTilemap");
    }

    void Start()
    {
        bounds = grassTilemap.GetComponent<Tilemap>().localBounds;
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
        // Spawn the enemy at a random position on spawn radius within the bounds of the tilemap
        do {
            spawnPosition = player.transform.position + Random.onUnitSphere * spawnRadius;
        } while (Vector2.Distance(spawnPosition, player.transform.position) < spawnRadius || 
        spawnPosition.x < bounds.min.x + 1 || 
        spawnPosition.y < bounds.min.y + 1 || 
        spawnPosition.x > bounds.max.x - 1 || 
        spawnPosition.y > bounds.max.y - 1);

        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = this; // Set the enemySpawner of the enemy
        enemy.transform.parent = this.transform; // Set the parent of the enemy to the spawner
        enemy.tag = "Enemy";
        enemyList.Add(enemy);
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy);
    }
}
