using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interface for input processors
/// </summary>
public interface IInputProcessor
{
    /// <summary>
    /// Returns true if the input device can process input
    /// </summary>
    /// <param name="inputDevice"></param>
    /// <returns></returns>
    bool CanProcessInput(InputDevice inputDevice);
    /// <summary>
    /// Sensitivity multiplier for the input processor
    /// </summary>
    float SensitivityMultiplier { get; }
}
