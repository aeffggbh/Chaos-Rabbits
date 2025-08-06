
using UnityEngine;

/// <summary>
/// Interface for user instantiation in level
/// </summary>
public interface IMechanicInstantiateUser
{
    /// <summary>
    /// Saves the prefab of the user to instantiate
    /// </summary>
    GameObject UserPrefab { get; }
}
