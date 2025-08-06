using System.Collections;
using UnityEngine;

public class IdleState : EnemyState
{
    private float idleTimer;
    private Coroutine coroutine;

    public IdleState(Enemy enemy) : base(enemy) { }

    public override void CheckRange()
    {
        if (distanceFromPlayer <= enemy.Stats.AttackRange)
            enemy.ChangeState(new AttackState(enemy));
        else if (distanceFromPlayer > enemy.Stats.AttackRange &&
            distanceFromPlayer <= enemy.Stats.ChaseRange)
            enemy.ChangeState(new ChaseState(enemy));
    }

    public override void Enter()
    {
        if (enemy as IIdleBehavior == null)
            return;

        (enemy as IIdleBehavior)?.ActivateIdle();
        enemy.CurrentSpeed = 0f;
        enemy.MoveDir = Vector3.zero;
        idleTimer = enemy.Stats.PatrolTimer * 3;

        coroutine = enemy.StartCoroutine(Execute());
    }

    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(idleTimer);
        enemy.ChangeState(new PatrolState(enemy));
    }

    public override void Exit()
    {
        if (coroutine != null)
        {
            enemy.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
