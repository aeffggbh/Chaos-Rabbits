using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    private List<IInputProcessor> _inputProcessors = new();
    private IInputProcessor _activeProcessor;

    public void RegisterProcessor(IInputProcessor processor)
    {
        _inputProcessors.Add(processor);
    }

    public IInputProcessor GetProcessorForDevice(InputDevice inputDevice)
    {
        return _inputProcessors.FirstOrDefault(p => p.CanProcessInput(inputDevice));
    }
}