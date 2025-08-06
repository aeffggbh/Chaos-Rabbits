using UnityEngine;

/// <summary>
/// Event that is called when the game is paused
/// </summary>
public class PauseEvent : IPauseEvent
{
    private GameObject _source;
    public GameObject TriggeredByGO => _source;

    public PauseEvent(GameObject source)
    {
        _source = source;
    }
}
