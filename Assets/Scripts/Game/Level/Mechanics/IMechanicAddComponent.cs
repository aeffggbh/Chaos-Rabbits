
using UnityEngine;

/// <summary>
/// Interface for mechanics that need to add components
/// </summary>
public interface IMechanicAddComponent
{
    /// <summary>
    /// Adds the needed components to the user object
    /// </summary>
    /// <param name="userObj"></param>
    void AddNeededComponent(GameObject userObj);
}
