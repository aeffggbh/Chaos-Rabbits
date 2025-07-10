
using UnityEngine.InputSystem;

/// <summary>
/// Class to enable/disable player input
/// </summary>
public class PlayerInputEnabler : IPlayerInputEnabler
{
    private InputActionReference _moveAction;
    private InputActionReference _jumpAction;
    private InputActionReference _dropAction;
    private InputActionReference _grabAction;

    private PlayerMediator _controller;

    /// <summary>
    /// Initializes local input references
    /// </summary>
    public PlayerInputEnabler()
    {
        _controller = PlayerMediator.PlayerInstance;

        _moveAction = _controller.MoveAction;
        _jumpAction = _controller.JumpAction;
        _dropAction = _controller.DropAction;
        _grabAction = _controller.GrabAction;
    }

    public void Enable()
    {
        if (_moveAction)
        {
            _moveAction.action.performed += _controller.OnMove;
            _moveAction.action.canceled += _controller.OnCancelMove;
        }

        if (_jumpAction) 
            _jumpAction.action.started += _controller.OnJump;

        if (_dropAction)
            _dropAction.action.started += _controller.OnDropWeapon;

        if (_grabAction)
            _grabAction.action.started += _controller.OnGrabWeapon;
    }

    public void Disable()
    {
        if (_moveAction)
        {
            _moveAction.action.started -= _controller.OnMove;
            _moveAction.action.canceled -= _controller.OnCancelMove;
        }

        if (_jumpAction)
            _jumpAction.action.started -= _controller.OnJump;

        if (_dropAction)
            _dropAction.action.started -= _controller.OnDropWeapon;

        if (_grabAction)
            _grabAction.action.started -= _controller.OnGrabWeapon;
    }
}