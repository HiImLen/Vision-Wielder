using System.Collections;
using UnityEngine;

public class RuinHunterAttackState : BossAttackState
{
    private float speed;
    private float lifeTime;
    private int fireCount = 5; // number of projectiles fired at once
    private float fireDelay = 0.2f;
    private float elapsedTime;
    private float fireTime = 0.0f;
    GameObject player;
    BossStateManager bossStateManager;
    BossBehavior bossBehavior;

    public override void EnterState(BossStateManager boss)
    {
        elapsedTime = 0f;
        speed = 350f;
        lifeTime = 5f;
        player = boss.GetComponent<BossBehavior>().player;
        bossBehavior = boss.GetComponent<BossBehavior>();
        bossStateManager = boss;
        boss.StartCoroutine(StartFire());
    }

    public override void UpdateState(BossStateManager boss)
    {
        elapsedTime += Time.deltaTime;
        fireTime += Time.deltaTime;

        if (elapsedTime >= fireCount * fireDelay)
        {
            boss.SwitchState(boss.idleState);
        }
    }

    public override void FixedUpdateState(BossStateManager boss)
    {

    }

    IEnumerator StartFire()
    {
        for (int i = 0; i < fireCount; i++)
        {
            if (player == null) yield return null;
            GameObject projectile = (GameObject)Object.Instantiate(bossStateManager.skillPrefab, bossStateManager.transform.position, Quaternion.identity);
            projectile.GetComponent<RuinHunterMissle>().Launch(player.transform.position, speed, lifeTime);
            projectile.GetComponent<RuinHunterMissle>().bossBehavior = bossBehavior;
            yield return new WaitForSeconds(fireDelay);
        }
    }
}
