using System.Collections.Generic;
using UnityEngine;

public interface IEnemyContainer
{
    /// <summary>
    /// List of enemies
    /// </summary>
    List<Enemy> Enemies { get; }
    /// <summary>
    /// Adds an enemy to the enemy list
    /// </summary>
    /// <param name="enemy"></param>
    void Add(Enemy enemy);
    /// <summary>
    /// Removes an enemy
    /// </summary>
    /// <param name="enemy"></param>
    void Remove(Enemy enemy);
}