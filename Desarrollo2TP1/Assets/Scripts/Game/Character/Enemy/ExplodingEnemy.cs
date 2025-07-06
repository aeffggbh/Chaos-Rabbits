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
    //private ExplosionManager _explosionManager;
    private IEffectTrigger _effectTrigger;
    private bool _explosionTriggered;

    protected override void Start()
    {
        base.Start();

        _explosionTriggered = false;

        _effectTrigger = GetComponent<IEffectTrigger>();

        animationController = gameObject.AddComponent<AngryAnimationController>();
        _angryAnimation = animationController as AngryAnimationController;

        _effectTrigger.EffectRange = _attackRange;

        //_explosionManager = GetComponent<ExplosionManager>();
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

    private void HandleEffectStart(EffectStartedEvent myEvent)
    {
        if (myEvent.TriggeredByGO == gameObject)
        {
            EventProvider.Unsubscribe<EffectStartedEvent>(HandleEffectStart);
            PlayerMediator.PlayerInstance.TakeDamage(Damage);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_explosionTriggered && GetPlayerDistance() < _attackRange)
            StartExplosion();

    }

    private void StartExplosion()
    {
        _explosionTriggered = true;
        _angryAnimation.Attack();
        _effectTrigger.TriggerEffect();
    }

    private void OnExplosionComplete(EffectCompletedEvent myEvent)
    {
        if (myEvent.TriggeredByGO == gameObject)
        {
            EventProvider.Unsubscribe<EffectCompletedEvent>(OnExplosionComplete);
            Die();
        }

    }
}
