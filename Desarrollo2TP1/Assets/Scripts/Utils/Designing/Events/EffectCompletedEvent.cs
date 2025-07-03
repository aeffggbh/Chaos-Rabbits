using UnityEngine;

public class EffectCompletedEvent : IEvent
{
    private GameObject _triggeredByGO;

    public IEffect EffectTrigger { get; set; }
    public GameObject TriggeredByGO { get { return _triggeredByGO; } set { _triggeredByGO = value; } }

    public EffectCompletedEvent(GameObject source, IEffect effectTrigger)
    {
        TriggeredByGO = source;
        EffectTrigger = effectTrigger;
    }
}