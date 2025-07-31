using UnityEngine;

public class ChronometerTriggerEvent : IChronometerTriggerEvent
{
    private Collider _other;
    private GameObject _triggeredByGO;
    public Collider Other => _other;
    public GameObject TriggeredByGO => _triggeredByGO;

    public ChronometerTriggerEvent(Collider other, GameObject triggeredByGO)
    {
        _other = other;
        _triggeredByGO = triggeredByGO;
    }
}