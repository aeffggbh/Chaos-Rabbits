using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    private Coroutine coroutine;

    public AttackState(Enemy enemy) : base(enemy)
    {}

    public override void CheckRange()
    {
        base.CheckRange();
        
        if (distanceFromPlayer > enemy.Stats.AttackRange)
            enemy.ChangeState(new ChaseState(enemy));
    }

    public override void Enter()
    {
        if (enemy is not IAttackBehavior)
            return;

        (enemy as IAttackActivationBehavior)?.ActivateAttack();

        enemy.TimeSinceAttacked = 0;

        coroutine = enemy.StartCoroutine(Execute());
    }

    public override IEnumerator Execute()
    {
        while (true)
        {
            if (enemy.PlayerMediator)
            {
                enemy.TargetLook = enemy.PlayerMediator.transform.position;
                (enemy as IAttackBehavior)?.Attack();
                enemy.TimeSinceAttacked += Time.deltaTime;
            }

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