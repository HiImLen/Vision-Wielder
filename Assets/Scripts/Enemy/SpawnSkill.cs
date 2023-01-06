using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : MonoBehaviour
{
    public GameObject skillPrefab;
    public GameObject spawnEffect;
    public float distanceToPlayer = 10f;
    public float spawnTime = 6f;
    public float timeToSpawn = 1.5f;
    public int skillToSpawn = 3;
    public float skillDelay = 1f;
    public float circleScale = 1f;
    private GameObject player;
    private EnemyBehavior enemyBehavior;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyBehavior = GetComponent<EnemyBehavior>();
        InvokeRepeating("Spawn", 0f, spawnTime);
    }

    void Spawn()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToPlayer)
        {
            StartCoroutine(StartSkill());
        }
    }

    IEnumerator StartSkill()
    {
        for (int i = 0; i < skillToSpawn; i++)
        {
            Vector2 pos = player.transform.position;
            StartCoroutine(SpawnSkills(pos));
            yield return new WaitForSeconds(skillDelay);
        }
    }

    IEnumerator SpawnSkills(Vector2 spawnPos)
    {
        // Spawn the effect and destroy it after 2 seconds
        GameObject effect = Instantiate(spawnEffect, spawnPos, Quaternion.identity);
        effect.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        Destroy(effect, timeToSpawn);
        // Wait 2 seconds before spawning the skill
        yield return new WaitForSeconds(timeToSpawn);
        // Spawn the skill
        GameObject skill = Instantiate(skillPrefab, spawnPos, Quaternion.identity);
        skill.GetComponent<EnemySkillZone>().enemyBehavior = enemyBehavior;
    }
}
