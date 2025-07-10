/// <summary>
/// Interface to trigger effects
/// </summary>
public interface IEffectTrigger
{
    /// <summary>
    /// Triggers the effect 
    /// </summary>
    void TriggerEffect();
    /// <summary>
    /// If the effect is playing, this is true
    /// </summary>
    bool IsEffectActive { get; }
    /// <summary>
    /// The range of the effect to trigger
    /// </summary>
    float EffectRange { get; set; }
}