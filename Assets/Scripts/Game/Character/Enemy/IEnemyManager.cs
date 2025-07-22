using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager
{
    /// <summary>
    /// List of enemies
    /// </summary>
     List<Enemy> Enemies { get; }

    /// <summary>
    /// Time between attacks
    /// </summary>
    float AttackTimer { get; set; }
    /// <summary>
    /// How much an enemy walks
    /// </summary>
    float WalkRange { get ; set; }
    /// <summary>
    /// At what distance the enemy attacks the player
    /// </summary>
    float AttackRange { get; set; }
    /// <summary>
    /// At what distance the enemy chases the player
    /// </summary>
    float ChaseRange { get; set; }
    /// <summary>
    /// The speed at which it chases the player
    /// </summary>
    float ChasingSpeed { get; set; }
    /// <summary>
    /// The speed at which it patrols
    /// </summary>
    float PatrolSpeed { get; set; }
    /// <summary>
    /// Timer between patrols
    /// </summary>
    float PatrolTimer { get; set; }
    /// <summary>
    /// How much is the enemy stopped from moving
    /// </summary>
    float CounterMovementForce { get; set; }
}