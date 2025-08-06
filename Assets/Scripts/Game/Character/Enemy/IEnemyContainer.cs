using System.Collections.Generic;
using UnityEngine;

public interface IEnemyContainer
{
    /// <summary>
    /// List of enemies
    /// </summary>
    List<Enemy> Enemies { get; }
}