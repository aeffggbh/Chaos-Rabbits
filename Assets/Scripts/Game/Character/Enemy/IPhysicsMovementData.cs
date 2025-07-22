using UnityEngine;

public interface IPhysicsMovementData
{
    /// <summary>
    /// Current speed
    /// </summary>
    float CurrentSpeed { get; set; }
    /// <summary>
    /// Acceleration
    /// </summary>
    float Acceleration { get; set; }
    /// <summary>
    /// Counter movement force
    /// </summary>
    float CounterMovementForce { get; set; }
    /// <summary>
    /// Running speed
    /// </summary>
    float RunSpeed { get; set; }
    /// <summary>
    /// Walking speed
    /// </summary>
    float WalkSpeed { get; set; }
    /// <summary>
    /// The rigidbody of the object
    /// </summary>
    Rigidbody Rb { get; set; }
}
