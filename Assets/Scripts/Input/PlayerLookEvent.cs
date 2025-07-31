using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookEvent : IPlayerLookEvent
{
    private Vector2 _lookDir;
    private readonly GameObject _triggeredByGO;
    private readonly InputDevice _inputDevice;
    public Vector2 LookDir => _lookDir;
    public GameObject TriggeredByGO => _triggeredByGO;
    public InputDevice InputDevice => _inputDevice;

    public PlayerLookEvent(GameObject triggeredByGO, InputAction.CallbackContext ctx)
    {
        _triggeredByGO = triggeredByGO;
        _lookDir = ctx.ReadValue<Vector2>();
        _inputDevice = ctx.control.device;
    }
}