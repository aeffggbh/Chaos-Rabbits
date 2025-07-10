using System;

/// <summary>
/// Interface for effects in the game
/// </summary>
public interface IEffect
{
    /// <summary>
    /// Plays the effect
    /// </summary>
    void Play();
    /// <summary>
    /// Completes the effect
    /// </summary>
    void Complete();
    /// <summary>
    /// If the effect is still playing, this returns true
    /// </summary>
    bool IsPlaying { get; }
    /// <summary>
    /// Duration of the effect
    /// </summary>
    float Duration { get; }
    /// <summary>
    /// Event that handles when the effect is complete
    /// TODO: Maybe... remove it.
    /// </summary>
    event Action<IEffect> OnEffectComplete;
}
