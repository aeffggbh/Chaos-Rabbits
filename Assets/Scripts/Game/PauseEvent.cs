using UnityEngine;

public class PauseEvent : IPauseEvent
{
    private GameObject _source;
    public GameObject TriggeredByGO => _source;

    public PauseEvent(GameObject source)
    {
        _source = source;
    }
}
