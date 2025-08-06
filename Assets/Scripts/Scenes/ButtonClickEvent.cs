using UnityEngine;

/// <summary>
/// Event that is called when a button is clicked
/// </summary>
public class ButtonClickEvent : IButtonClickEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public ButtonClickEvent(GameObject triggeredByGO)
    {
        _triggeredByGO = triggeredByGO;
    }
}