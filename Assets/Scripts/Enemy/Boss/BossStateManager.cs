using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttackState : BossBaseState
{
    public override abstract void UpdateState(BossStateManager boss);
    public override abstract void FixedUpdateState(BossStateManager boss);
}

public class BossStateManager : MonoBehaviour
{
    public GameObject skillPrefab;
    public GameObject effectPrefab;

    BossBaseState currentState;
    public BossIdleState idleState = new BossIdleState();
    public BossChargeState chargeState = new BossChargeState();
    public BossAttackState attackState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState (BossBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
