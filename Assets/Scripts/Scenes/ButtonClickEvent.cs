using UnityEngine;

internal class ButtonClickEvent : IButtonClickEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public ButtonClickEvent(GameObject triggeredByGO)
    {
        _triggeredByGO = triggeredByGO;
    }
}