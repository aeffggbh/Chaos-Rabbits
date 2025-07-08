using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager
{
    /// <summary>
    /// List of enemies
    /// </summary>
    public List<Enemy> Enemies { get; }

    /// <summary>
    /// Time between attacks
    /// </summary>
    public float AttackTimer { get; set; }
    /// <summary>
    /// How much an enemy walks
    /// </summary>
    public float WalkRange { get ; set; }
    /// <summary>
    /// At what distance the enemy attacks the player
    /// </summary>
    public float AttackRange { get; set; }
    /// <summary>
    /// At what distance the enemy chases the player
    /// </summary>
    public float ChaseRange { get; set; }
    /// <summary>
    /// The speed at which it chases the player
    /// </summary>
    public float ChasingSpeed { get; set; }
    /// <summary>
    /// The speed at which it patrols
    /// </summary>
    public float PatrolSpeed { get; set; }
    /// <summary>
    /// Timer between patrols
    /// </summary>
    public float PatrolTimer { get; set; }
    /// <summary>
    /// How much is the enemy stopped from moving
    /// </summary>
    public float CounterMovementForce { get; set; }
}