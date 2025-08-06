using System.Collections;
using UnityEngine;

public class ChaseState : EnemyState
{
    private Coroutine coroutine;

    public ChaseState(Enemy enemy) : base(enemy)
    {}

    public override void CheckRange()
    {
        base.CheckRange();

        if (distanceFromPlayer <= enemy.Stats.AttackRange)
            enemy.ChangeState(new AttackState(enemy));

        if (distanceFromPlayer > enemy.Stats.ChaseRange)
            enemy.ChangeState(new PatrolState(enemy));
    }

    public override void Enter()
    {
        if (enemy is not IChaseBehavior)
            return;

        (enemy as IChaseBehavior)?.ActivateChase();
        coroutine = enemy.StartCoroutine(Execute());
    }

    public override IEnumerator Execute()
    {
        while (true)
        {
            enemy.Chase();
            yield return null;
        }
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