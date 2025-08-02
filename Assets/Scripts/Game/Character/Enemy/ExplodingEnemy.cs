using System;
using UnityEngine;

/// <summary>
/// Represents an enemy that explodes when it gets close to the player.
/// </summary>
[RequireComponent(typeof(ExplosionEffect))]
[RequireComponent(typeof(EffectTrigger))]
public partial class ExplodingEnemy : Enemy
{
    private AngryAnimationController _angryAnimation;
    private IEffectTrigger _effectTrigger;
    private bool _explosionTriggered;

    protected override void Start()
    {
        base.Start();

        _explosionTriggered = false;

        _effectTrigger = GetComponent<IEffectTrigger>();

        animationController = gameObject.AddComponent<AngryAnimationController>();
        _angryAnimation = animationController as AngryAnimationController;

        _effectTrigger.EffectRange = attackRange;

        if (_angryAnimation == null)
            Debug.LogError("animation is null for " + gameObject.name);

        EventProvider.Subscribe<EffectCompletedEvent>(OnExplosionComplete);
        EventProvider.Subscribe<EffectStartedEvent>(HandleEffectStart);
    }

    private void OnDestroy()
    {
        if (_effectTrigger != null)
        {
            EventProvider.Unsubscribe<EffectStartedEvent>(HandleEffectStart);
            EventProvider.Unsubscribe<EffectCompletedEvent>(OnExplosionComplete);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_explosionTriggered && GetPlayerDistance() < attackRange)
            StartExplosion();

    }

    /// <summary>
    /// Handles the start of the explosion effect, causing the player to take damage if triggered by this enemy.
    /// </summary>
    private void HandleEffectStart(EffectStartedEvent myEvent)
    {
        if (myEvent.TriggeredByGO == gameObject)
        {
            EventProvider.Unsubscribe<EffectStartedEvent>(HandleEffectStart);
            if (GetPlayerDistance() < _effectTrigger.EffectRange)
            {
                ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
                mediator.TakeDamage(Damage);
            }
        }
    }

    /// <summary>
    /// Initiates the explosion sequence, triggering the attack animation and effect.
    /// </summary>
    private void StartExplosion()
    {
        _explosionTriggered = true;
        _angryAnimation.AnimateAttack();
        _effectTrigger.TriggerEffect();
    }

    /// <summary>
    /// Handles the completion of the explosion effect and kills the enemy if triggered by this enemy.
    /// </summary>
    private void OnExplosionComplete(EffectCompletedEvent myEvent)
    {
        if (myEvent.TriggeredByGO == gameObject)
        {
            EventProvider.Unsubscribe<EffectCompletedEvent>(OnExplosionComplete);
            Die();
        }

    }
}
