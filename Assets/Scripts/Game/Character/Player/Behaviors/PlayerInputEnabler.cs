
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
    private InputActionReference _lookAction;

    private PlayerMediator _playerMediator;

    /// <summary>
    /// Initializes local input references
    /// </summary>
    public PlayerInputEnabler()
    {
        ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
        _playerMediator = mediator;

        _moveAction = _playerMediator.MoveAction;
        _jumpAction = _playerMediator.JumpAction;
        _dropAction = _playerMediator.DropAction;
        _grabAction = _playerMediator.GrabAction;
        _lookAction = _playerMediator.LookAction;
    }

    public void Enable()
    {
        if (_moveAction)
        {
            _moveAction.action.performed += _playerMediator.OnMove;
            _moveAction.action.canceled += _playerMediator.OnCancelMove;
        }

        if (_jumpAction) 
            _jumpAction.action.started += _playerMediator.OnJump;

        if (_dropAction)
            _dropAction.action.started += _playerMediator.OnDropWeapon;

        if (_grabAction)
            _grabAction.action.started += _playerMediator.OnGrabWeapon;

        if (_lookAction)
        {
            _lookAction.action.started += _playerMediator.OnLook;
            _lookAction.action.performed += _playerMediator.OnLook;
        }
    }

    public void Disable()
    {
        if (_moveAction)
        {
            _moveAction.action.started -= _playerMediator.OnMove;
            _moveAction.action.canceled -= _playerMediator.OnCancelMove;
        }

        if (_jumpAction)
            _jumpAction.action.started -= _playerMediator.OnJump;

        if (_dropAction)
            _dropAction.action.started -= _playerMediator.OnDropWeapon;

        if (_grabAction)
            _grabAction.action.started -= _playerMediator.OnGrabWeapon;
    }
}