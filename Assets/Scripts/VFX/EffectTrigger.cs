using System.Collections;
using UnityEngine;

/// <summary>
/// Triggers effects
/// </summary>
public class EffectTrigger : MonoBehaviour, IEffectTrigger
{
    [SerializeField] private float _effectDelay = 0f;
    [SerializeField] private float _effectRange = 3f;

    private IEffect _effect;
    private bool _isEffectActive;
    private Coroutine _effectRoutine;

    public bool IsEffectActive => _isEffectActive;

    public float EffectRange { get { return _effectRange; } set { _effectRange = value; } }


    private void Awake()
    {
        _effect = GetComponent<IEffect>();
        if (_effect == null)
            Debug.LogError("No IEffect found");

        _effect.OnEffectComplete += HandleEffectComplete;
    }

    public void TriggerEffect()
    {
        if (_isEffectActive)
            return;

        _isEffectActive = true;

        if (_effectRoutine != null)
            StopCoroutine(_effectRoutine);

        _effectRoutine = StartCoroutine(EffectTriggerRoutine());
    }

    /// <summary>
    /// Triggers the effect with a delay set locally
    /// </summary>
    /// <returns></returns>
    private IEnumerator EffectTriggerRoutine()
    {
        yield return new WaitForSeconds(_effectDelay);

        EventTriggerManager.Trigger<EffectStartedEvent>(new(gameObject, this));

        _effect.Play();
    }

    /// <summary>
    /// Handles what happens when the effect is complete
    /// </summary>
    /// <param name="effect"></param>
    private void HandleEffectComplete(IEffect effect)
    {
        _isEffectActive = false;

        EventTriggerManager.Trigger<EffectCompletedEvent>(new(gameObject));
    }

    private void OnDisable()
    {
        if (_effectRoutine != null)
            StopCoroutine(_effectRoutine);
    }
}