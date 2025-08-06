using UnityEngine;

/// <summary>
/// Event that is called when a user is deleted
/// </summary>
public class DeleteUserEvent : IDeleteUserEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public DeleteUserEvent(GameObject triggeredByGO)
    {
        _triggeredByGO = triggeredByGO;
    }
}