using UnityEngine;

public class BossIdleState : BossBaseState
{
    private float elapsedTime;
    Rigidbody2D rigidbody2d;
    float movementSpeed;
    GameObject player;

    public override void EnterState(BossStateManager boss)
    {
        elapsedTime = 0f;
        rigidbody2d = boss.GetComponent<BossBehavior>().rigidbody2d;
        movementSpeed = boss.GetComponent<BossBehavior>().speed;
        player = boss.GetComponent<BossBehavior>().player;
    }

    public override void UpdateState(BossStateManager boss)
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 3f)
        {
            // Switch to the a random state
            int randomState = Random.Range(0, 3);
            switch (randomState)
            {
                case 0:
                    boss.SwitchState(boss.attackState);
                    break;
                case 1:
                    boss.SwitchState(boss.chargeState);
                    break;
                case 2:
                    boss.SwitchState(boss.idleState);
                    break;
            }
        }

        // Flip the enemy if the direction is left
        if (player.transform.position.x < boss.transform.position.x)
            boss.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (player.transform.position.x > boss.transform.position.x)
            boss.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void FixedUpdateState(BossStateManager boss)
    {
        // Move toward player
        Vector2 position = rigidbody2d.position;
        position = Vector2.MoveTowards(position, player.transform.position, movementSpeed * Time.deltaTime);
        rigidbody2d.MovePosition(position);
    }
}
