using System;

public interface IEffectTrigger
{
    void TriggerEffect();
    bool IsEffectActive { get; }
    float EffectRange { get; set; }
}