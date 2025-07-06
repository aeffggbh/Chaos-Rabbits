using UnityEngine;

public interface IPhysicsMovementData
{
    float CurrentSpeed { get; set; }
    float Acceleration { get; set; }
    float CounterMovementForce { get; set; }
    float RunSpeed { get; set; }
    float WalkSpeed { get; set; }
    Rigidbody Rb {  get; set; }
}
