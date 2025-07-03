using System;

public interface IEffect
{
    void Play();
    void Stop();
    bool IsPlaying { get; }
    float Duration { get; }
    event Action<IEffect> OnEffectComplete;
}
