using UnityEngine;

/// <summary>
/// Event that is called whenever a menu enables
/// </summary>
public class MenuEnableEvent : IMenuEnableEvent
{
    private GameObject _source;
    public GameObject TriggeredByGO => _source;

    public MenuEnableEvent(GameObject source)
    {
        _source = source;
    }
}