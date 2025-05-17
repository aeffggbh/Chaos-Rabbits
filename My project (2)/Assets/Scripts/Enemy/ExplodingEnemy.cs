using UnityEngine;

public class ExplodingEnemy : Enemy
{
    AngryAnimationController angryAnimation;

    protected override void Start()
    {
        base.Start();
        animationController = gameObject.AddComponent<AngryAnimationController>();
        angryAnimation = animationController as AngryAnimationController;
    }

    protected override void ActivateChase()
    { }

    protected override void ActivateIdle()
    { }

    protected override void Attack()
    { }

    protected override void ActivateAttack()
    {
        angryAnimation.Attack();
    }

    protected override void Move()
    { }
}
