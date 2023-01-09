using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnEffect;
    public float distanceToPlayer = 10f;
    public float spawnRadius = 5f;
    public float spawnTime = 3f;
    public float timeToSpawn = 1.5f;
    public int enemyToSpawn = 5;
    public float circleScale = 1f;
    private GameObject player;
    private EnemySpawner enemySpawner;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("Spawn", 0f, spawnTime);
    }

    void Spawn()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToPlayer)
        {
            // Freeze the position
            StartCoroutine(FreezePosition());

            // Calculate the angle between each enemy
            float angleStep = 360f / enemyToSpawn;

            // Spawn the enemies
            for (int i = 0; i < enemyToSpawn; i++)
            {
                // Calculate the angle of each enemy
                float angle = angleStep * i;

                // Calculate the position of each enemy
                Vector3 pos = transform.position;
                pos.x += Mathf.Sin((angle * Mathf.PI) / 180) * spawnRadius;
                pos.y += Mathf.Cos((angle * Mathf.PI) / 180) * spawnRadius;

                // Spawn the enemy
                StartCoroutine(SpawnEnemy(pos));
            }
        }
    }

    IEnumerator SpawnEnemy(Vector3 spawnPos)
    {
        // Spawn the effect and destroy it after 2 seconds
        GameObject effect = Instantiate(spawnEffect, spawnPos, Quaternion.identity);
        effect.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        Destroy(effect, timeToSpawn);
        // Wait 2 seconds before spawning the enemy
        yield return new WaitForSeconds(timeToSpawn);

        // Spawn the enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemySpawner.enemyList.Add(enemy);
        enemy.GetComponent<EnemyBehavior>().enemySpawner = enemySpawner;
    }

    IEnumerator FreezePosition()
    {
        // Freeze the position
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(timeToSpawn);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
