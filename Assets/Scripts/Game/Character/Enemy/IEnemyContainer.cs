using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for the enemy container
/// </summary>
public interface IEnemyContainer
{
    /// <summary>
    /// List of enemies
    /// </summary>
    List<Enemy> Enemies { get; }
}