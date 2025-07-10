using UnityEngine;

/// <summary>
/// Event called when the effect is complete
/// </summary>
public class EffectCompletedEvent : IEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO { get { return _triggeredByGO; } set { _triggeredByGO = value; } }

    public EffectCompletedEvent(GameObject source)
    {
        TriggeredByGO = source;
    }
}