using System.Collections;
using UnityEngine;

public class DvalinAttackState : BossAttackState
{
    private float elapsedTime;
    private int skillToSpawn;
    private float skillDelay;
    private float circleScale;
    private float timeToSpawn;
    private GameObject spawnEffect;
    private GameObject skillPrefab;
    GameObject player;
    BossStateManager bossStateManager;
    BossBehavior bossBehavior;

    public override void EnterState(BossStateManager boss)
    {
        elapsedTime = 0f;
        player = boss.GetComponent<BossBehavior>().player;
        bossBehavior = boss.GetComponent<BossBehavior>();
        spawnEffect = boss.GetComponent<BossStateManager>().effectPrefab;
        skillPrefab = boss.GetComponent<BossStateManager>().skillPrefab;
        bossStateManager = boss;
        spawn();
    }

    public override void UpdateState(BossStateManager boss)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= skillToSpawn * skillDelay + timeToSpawn)
        {
            boss.SwitchState(boss.idleState);
        }
    }

    public override void FixedUpdateState(BossStateManager boss)
    {
        
    }

    void spawn()
    {
        skillToSpawn = 3;
        skillDelay = 0.5f;
        circleScale = 0.1f;
        timeToSpawn = 2f;
        bossStateManager.StartCoroutine(StartSkill());
    }

    IEnumerator StartSkill()
    {
        for (int i = 0; i < skillToSpawn; i++)
        {
            Vector2 pos = new Vector2(player.transform.position.x + Random.Range(-5f, 5f), player.transform.position.y + Random.Range(-5f, 5f));
            bossStateManager.StartCoroutine(SpawnSkills(pos));
            yield return new WaitForSeconds(skillDelay);
        }
    }

    IEnumerator SpawnSkills(Vector2 spawnPos)
    {
        // Spawn the effect and destroy it after 2 seconds
        GameObject effect = (GameObject)Object.Instantiate(spawnEffect, spawnPos, Quaternion.identity);
        effect.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        Object.Destroy(effect, timeToSpawn);
        // Wait 2 seconds before spawning the skill
        yield return new WaitForSeconds(timeToSpawn);
        // Spawn the skill
        GameObject skill = (GameObject)Object.Instantiate(skillPrefab, spawnPos, Quaternion.identity);
        skill.GetComponent<Tornado>().bossBehavior = bossBehavior;
    }
}

