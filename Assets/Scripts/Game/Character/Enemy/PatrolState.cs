using System.Collections;
using UnityEngine;

/// <summary>
/// Patrol state
/// </summary>
public class PatrolState : EnemyState
{
    private float patrolTimer;
    private Vector3 targetWalk;
    private Coroutine coroutine;

    public PatrolState(Enemy enemy) : base(enemy)
    {
        patrolTimer = enemy.Stats.PatrolTimer;
    }

    public override void CheckRange()
    {
        base.CheckRange();

        if (distanceFromPlayer <= enemy.Stats.AttackRange)
            enemy.ChangeState(new AttackState(enemy));
        else if (distanceFromPlayer > enemy.Stats.AttackRange &&
            distanceFromPlayer <= enemy.Stats.ChaseRange)
            enemy.ChangeState(new ChaseState(enemy));
    }

    public override void Enter()
    {
        if (enemy as IPatrolBehavior == null)
            return;
        enemy.CurrentSpeed = enemy.Stats.PatrolSpeed;
        coroutine = enemy.StartCoroutine(Execute());
    }

    public override IEnumerator Execute()
    {
        float randomZ = Random.Range(-enemy.Stats.WalkRange, enemy.Stats.WalkRange);
        float randomX = Random.Range(-enemy.Stats.WalkRange, enemy.Stats.WalkRange);

        Vector3 dir = new(randomX, 0, randomZ);
        dir = dir.normalized;

        float distance = enemy.Stats.WalkRange;

        if (RayManager.PointingToObject(enemy.transform, distance, out RaycastHit hitInfo, dir))
            distance = hitInfo.distance * 0.7f;

        targetWalk = enemy.transform.position + (dir * distance);

        enemy.TargetLook = targetWalk;
        enemy.MoveDir = new(dir.x, 0, dir.z);
        (enemy as IPatrolBehavior)?.ActivatePatrol();

        float start = Time.time;

        float distanceThreshold = 0.3f;

        while ((Time.time - start) < patrolTimer)
        {
            float distanceToTarget = Vector3.Distance(
                new(enemy.transform.position.x, 0, enemy.transform.position.z),
                new(targetWalk.x, 0, targetWalk.z));

            if (distanceToTarget < distanceThreshold)
            {
                enemy.ChangeState(new IdleState(enemy));
                yield break;
            }

            yield return null;
        }

        enemy.ChangeState(new IdleState(enemy));
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
