using UnityEngine;

public class BossIdleState : BossBaseState
{
    private float elapsedTime;
    Vector2 randomDirection;
    Rigidbody2D rigidbody2d;
    float movementSpeed;
    Vector2 oldPosition;
    Vector2 newPosition;

    public override void EnterState(BossStateManager boss)
    {
        elapsedTime = 0f;
        randomDirection = Random.insideUnitCircle.normalized;
        oldPosition = boss.transform.position;
        newPosition = oldPosition + randomDirection;
        rigidbody2d = boss.GetComponent<BossBehavior>().rigidbody2d;
        movementSpeed = boss.GetComponent<BossBehavior>().speed;

        // Flip the enemy if the random direction is < old position
        if (newPosition.x < oldPosition.x)
            boss.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (newPosition.x > oldPosition.x)
            boss.transform.rotation = Quaternion.Euler(0, 0, 0);
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
    }

    public override void FixedUpdateState(BossStateManager boss)
    {
        // Move the boss to the new position
        rigidbody2d.MovePosition(rigidbody2d.position + randomDirection * movementSpeed * Time.deltaTime);
    }
}
