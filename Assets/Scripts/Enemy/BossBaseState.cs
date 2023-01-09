using UnityEngine;

public abstract class BossBaseStat
{
    protected BossStateManager bossStateManager;

    public BossBaseStat(BossStateManager bossStateManager)
    {
        this.bossStateManager = bossStateManager;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
