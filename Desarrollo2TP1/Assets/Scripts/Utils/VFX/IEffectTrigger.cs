using System;

public interface IEffectTrigger
{
    void TriggerEffect();
    bool IsEffectActive { get; }
    float EffectRange { get; set; }

    event Action OnEffectComplete;
    event Action OnEffectStart;
}