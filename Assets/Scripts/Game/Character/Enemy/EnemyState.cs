using System.Collections;

public abstract class EnemyState : IEnemyState
{
    protected Enemy enemy;
    protected float distanceFromPlayer = 1000;

    protected EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public virtual void CheckRange()
    {
        if (enemy.PlayerMediator == null)
            return;

        distanceFromPlayer = enemy.GetPlayerDistance();
    }

    public abstract void Enter();

    public abstract IEnumerator Execute();

    public abstract void Exit();
}
