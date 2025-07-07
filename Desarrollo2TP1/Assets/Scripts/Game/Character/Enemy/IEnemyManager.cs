using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager
{
    public List<Enemy> Enemies { get; }
    public float AttackTimer { get; set; }
    public float WalkRange { get; set; }
    public float AttackRange { get; set; }
    public float ChaseRange { get; set; }
    public float ChasingSpeed { get; set; }
    public float PatrolSpeed { get; set; }
    public float PatrolTimer { get; set; } 
    public float CounterMovementForce { get; set; }
}