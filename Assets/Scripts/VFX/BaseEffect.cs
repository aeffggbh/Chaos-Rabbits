
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A base class for all the effects in the game
/// </summary>
public abstract class BaseEffect : MonoBehaviour, IEffect
{
    [SerializeField] protected float effectDuration = 1f;

    protected float effectTimer = 1f;

    protected bool isPlaying;

    public bool IsPlaying => isPlaying;

    public float Duration => effectDuration;

    public event Action<IEffect> OnEffectComplete;

    public void Play()
    {
        if (isPlaying) 
            return;

        isPlaying = true;

        effectTimer = 0f;

        StartCoroutine(EffectRoutine());
    }

    /// <summary>
    /// Plays the effect logic
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator EffectRoutine();

    public void Complete()
    {
        isPlaying = false;
        OnEffectComplete?.Invoke(this);
        effectTimer = 0f;
    }

    protected virtual void Update()
    {
        if (IsPlaying)
        {
            effectTimer += Time.deltaTime;

            if (effectTimer > effectDuration)
                Complete();
        }
    }
}
