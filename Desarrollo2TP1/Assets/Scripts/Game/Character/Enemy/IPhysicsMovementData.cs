using UnityEngine;

public interface IPhysicsMovementData
{
    /// <summary>
    /// Current speed
    /// </summary>
    public float CurrentSpeed { get; set; }
    /// <summary>
    /// Acceleration
    /// </summary>
    public float Acceleration { get; set; }
    /// <summary>
    /// Counter movement force
    /// </summary>
    public float CounterMovementForce { get; set; }
    /// <summary>
    /// Running speed
    /// </summary>
    public float RunSpeed { get; set; }
    /// <summary>
    /// Walking speed
    /// </summary>
    public float WalkSpeed { get; set; }
    /// <summary>
    /// The rigidbody of the object
    /// </summary>
    public Rigidbody Rb { get; set; }
}
