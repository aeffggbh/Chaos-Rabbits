using System;
using UnityEngine;

public interface IWheel : IModelData
{
    WheelCollider Collider { get; }
    void Move(float input, float speed, float acceleration);
    void Brake(float brakeForce, float bakeAcceleration);
    void StopBrake();
    void Animate();
}
