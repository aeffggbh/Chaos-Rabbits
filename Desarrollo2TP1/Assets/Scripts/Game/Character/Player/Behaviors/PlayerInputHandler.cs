
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : IPlayerInputHandler
{
    private InputActionReference _moveAction;
    private InputActionReference _jumpAction;
    private InputActionReference _dropAction;
    private InputActionReference _grabAction;

    private PlayerMediator _controller;

    public PlayerInputHandler()
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