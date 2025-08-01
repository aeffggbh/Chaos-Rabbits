using System;
using UnityEngine;

[Serializable]
public class BaseWheel : MonoBehaviour, IWheel
{
    [SerializeField] protected WheelCollider wheelCollider;
    [SerializeField] protected GameObject model;
    public WheelCollider Collider => wheelCollider;
    public GameObject Model { get => model; set => model = value; }

    private void Start()
    {
        if (!wheelCollider)
            wheelCollider = GetComponent<WheelCollider>();
    }

    public void Brake(float brakeForce, float bakeAcceleration)
    {
        wheelCollider.brakeTorque = brakeForce * bakeAcceleration * Time.fixedDeltaTime;
    }

    public void Move(float input, float speed, float acceleration)
    {
        wheelCollider.motorTorque = input * speed * acceleration * Time.fixedDeltaTime;
    }

    public void StopBrake()
    {
        wheelCollider.brakeTorque = 0;
    }

    public void Animate()
    {
        wheelCollider.GetWorldPose(out var pos, out var rot);
        model.transform.SetPositionAndRotation(pos, rot);
    }
}
