using UnityEngine;

public class FrontWheel : BaseWheel, ISteerWheel
{
    private float _turnSensibility;
    private float _maxSteerAngle;

    public FrontWheel(WheelCollider collider, 
        float maxSteerAngle = 30f, 
        float turnSensitivity = 1f)
    {
        base.wheelCollider = collider;
        _turnSensibility = turnSensitivity;
        _maxSteerAngle = maxSteerAngle;
    }

    private void Start()
    {
        if (_turnSensibility <= 0.1f)
            _turnSensibility = 1f;

        if (_maxSteerAngle <= 0.1f)
            _maxSteerAngle = 30f;
    }

    public void Steer(float steerInput)
    {
        var steerAngle = steerInput * _turnSensibility * _maxSteerAngle;
        wheelCollider.steerAngle = Mathf.Lerp(wheelCollider.steerAngle, steerAngle, 0.6f);
    }
}
