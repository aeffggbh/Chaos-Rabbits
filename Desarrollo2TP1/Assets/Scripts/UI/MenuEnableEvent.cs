using UnityEngine;

public class MenuEnableEvent : IMenuEnableEvent
{
    GameObject _source;
    public GameObject TriggeredByGO => _source;

    public MenuEnableEvent(GameObject source)
    {
        _source = source;
    }
}