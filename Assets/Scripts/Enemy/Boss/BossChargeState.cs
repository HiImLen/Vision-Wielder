using UnityEngine;

public class BossChargeState : BossBaseState
{
    private float elapsedTime;
    GameObject player;
    Rigidbody2D rigidbody2d;
    float movementSpeed;
    Vector2 direction;

    public override void EnterState(BossStateManager boss)
    {
        elapsedTime = 0f;
        player = boss.GetComponent<BossBehavior>().player;
        rigidbody2d = boss.GetComponent<BossBehavior>().rigidbody2d;
        movementSpeed = boss.GetComponent<BossBehavior>().speed;

        if (player.transform.position.x < boss.transform.position.x)
            boss.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (player.transform.position.x > boss.transform.position.x)
            boss.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void UpdateState(BossStateManager boss)
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        if (elapsedTime <= 1f)
            direction = (player.transform.position - boss.transform.position).normalized;

        if (elapsedTime >= 3f)
        {
            // Switch to the a random state
            int randomState = Random.Range(0, 2);
            switch (randomState)
            {
                case 0:
                    boss.SwitchState(boss.chargeState);
                    break;
                case 1:
                    boss.SwitchState(boss.idleState);
                    break;
            }
        }
    }

    public override void FixedUpdateState(BossStateManager boss)
    {
        // Move the boss to the new position
        if (elapsedTime >= 1f)
            rigidbody2d.MovePosition(rigidbody2d.position + direction * movementSpeed * 5f * Time.deltaTime);
    }
}
