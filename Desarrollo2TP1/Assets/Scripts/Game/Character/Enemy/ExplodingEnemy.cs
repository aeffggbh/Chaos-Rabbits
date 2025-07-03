using UnityEngine;

/// <summary>
/// Represents an enemy that explodes when it gets close to the player.
/// </summary>
public partial class ExplodingEnemy : Enemy, IAttackBehavior
{
    private ExplodingEnemyAnimationController _angryAnimation;
    private ExplosionManager _explosionManager;
    private bool _exploded;

    protected override void Start()
    {
        base.Start();
        _exploded = false;
        animationController = gameObject.AddComponent<ExplodingEnemyAnimationController>();
        _angryAnimation = animationController as ExplodingEnemyAnimationController;
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
                GameManager.savedPlayer.TakeDamage(Damage);

            Die();
            _exploded = true;
        }
    }

    public void ActivateAttack()
    {
        _angryAnimation.Attack();
        _explosionManager.StartExplosion();
    }

    //TODO: also this... I cant do anything about it!!!
    public void Attack()
    {
        Debug.Log(nameof(ExplodingEnemy) + " is about to EXPLODE");
    }
}
