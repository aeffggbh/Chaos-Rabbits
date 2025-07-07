using System.Collections;
using UnityEngine;

public class EffectTrigger : MonoBehaviour, IEffectTrigger
{
    [SerializeField] private float _effectDelay = 0f;
    [SerializeField] private float _effectRange = 3f;

    private IEffect _effect;
    private bool _isEffectActive;
    Coroutine _effectRoutine;

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

    private IEnumerator EffectTriggerRoutine()
    {
        yield return new WaitForSeconds(_effectDelay);

        EventTriggerManager.Trigger<EffectStartedEvent>(new(gameObject, this));

        _effect.Play();
    }

    private void HandleEffectComplete(IEffect effect)
    {
        _isEffectActive = false;

        effect.OnEffectComplete -= HandleEffectComplete;

        EventTriggerManager.Trigger<EffectCompletedEvent>(new(gameObject, effect));
    }

    private void OnDisable()
    {
        if (_effectRoutine != null)
            StopCoroutine(_effectRoutine);
    }
}