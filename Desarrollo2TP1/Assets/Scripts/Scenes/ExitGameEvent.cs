using UnityEngine;

public class ExitGameEvent : IExitGameEvent
{
    private GameObject _gameObject;
    public GameObject TriggeredByGO => _gameObject;

    public ExitGameEvent(GameObject gameObject)
    {
        _gameObject = gameObject;
    }
}
