
using System;
using System.Collections;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour, IEffect
{
    [SerializeField] protected float _effectDuration = 1f;

    protected float _effectTimer = 1f;

    protected bool _isPlaying;

    public bool IsPlaying => _isPlaying;

    public float Duration => _effectDuration;

    public event Action<IEffect> OnEffectComplete;

    public void Play()
    {
        if (_isPlaying) 
            return;

        _isPlaying = true;

        _effectTimer = 0f;

        StartCoroutine(EffectRoutine());
    }

    protected abstract IEnumerator EffectRoutine();


    public void Stop()
    {
        _isPlaying = false;
        OnEffectComplete?.Invoke(this);
        _effectTimer = 0f;
    }


    protected virtual void Update()
    {
        if (IsPlaying)
        {
            _effectTimer += Time.deltaTime;

            if (_effectTimer > _effectDuration)
                Stop();
        }
    }
}
