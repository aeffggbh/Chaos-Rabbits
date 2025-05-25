using UnityEngine;

public class ExplodingEnemy : Enemy
{
    private AngryAnimationController _angryAnimation;
    private ExplosionManager _explosionManager;
    private bool _exploded;

    protected override void Start()
    {
        base.Start();
        _exploded = false;
        animationController = gameObject.AddComponent<AngryAnimationController>();
        _angryAnimation = animationController as AngryAnimationController;
        _explosionManager = GetComponent<ExplosionManager>();
        if (_explosionManager == null)
            Debug.LogError("ExplosionManager is null for " + gameObject.name);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_explosionManager.Exploded() && !_exploded)
        {
            if (GetPlayerDistance() < _explosionManager._explosionRange)
                GameManager.savedPlayer.TakeDamage(damage);

            Die();
            _exploded = true;
        }
    }

    protected override void ActivateChase()
    { }

    protected override void ActivateIdle()
    { }

    protected override void Attack()
    { }

    protected override void ActivateAttack()
    {
        _angryAnimation.Attack();
        _explosionManager.StartExplotion();
    }

    protected override void Move()
    { }
}
