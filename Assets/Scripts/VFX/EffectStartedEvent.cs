using UnityEngine;

/// <summary>
/// Event called when the effect is started
/// </summary>
public class EffectStartedEvent : IEvent
{
    private GameObject _triggeredByGO;
    private IEffectTrigger _effectTrigger { get; set; }
    public GameObject TriggeredByGO { get { return _triggeredByGO; } set { _triggeredByGO = value; } }

    public EffectStartedEvent(GameObject source, IEffectTrigger effectTrigger)
    {
        TriggeredByGO = source;
        _effectTrigger = effectTrigger;
    }
}