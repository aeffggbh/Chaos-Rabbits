using UnityEngine;

internal class DeleteUserEvent : IDeleteUserEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public DeleteUserEvent(GameObject triggeredByGO)
    {
        _triggeredByGO = triggeredByGO;
    }
}