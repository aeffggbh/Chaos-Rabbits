using UnityEngine;

public class EffectStartedEvent : IEvent
{
    private GameObject _triggeredByGO;

    public IEffectTrigger EffectTrigger { get; set; }
    public GameObject TriggeredByGO { get { return _triggeredByGO; } set { _triggeredByGO = value; } }

    public EffectStartedEvent(GameObject source, IEffectTrigger effectTrigger)
    {
        TriggeredByGO = source;
        EffectTrigger = effectTrigger;
    }
}