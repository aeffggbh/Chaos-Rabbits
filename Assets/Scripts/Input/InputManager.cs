using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// An input manager that saves a list of all the active input processors
/// </summary>
public class InputManager
{
    private List<IInputProcessor> _inputProcessors = new();
    private IInputProcessor _activeProcessor;

    /// <summary>
    /// Registers a new processor
    /// </summary>
    /// <param name="processor"></param>
    public void RegisterProcessor(IInputProcessor processor)
    {
        _inputProcessors.Add(processor);
    }

    /// <summary>
    /// Returns a processor provided a device
    /// </summary>
    /// <param name="inputDevice"></param>
    /// <returns></returns>
    public IInputProcessor GetProcessorForDevice(InputDevice inputDevice)
    {
        return _inputProcessors.FirstOrDefault(p => p.CanProcessInput(inputDevice));
    }
}