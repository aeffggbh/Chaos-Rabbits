using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarInput))]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed = 600f;
    [SerializeField] private float _maxAcceleration = 30f;
    [SerializeField] private float _brakeForce = 300f;
    [SerializeField] private float _brakeAcceleration = 50f;
    [SerializeField] private Vector3 _centerOfMass;

    [SerializeField] private List<BaseWheel> _wheels;

    private ICarInput _carInput;

    private Rigidbody _carRb;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        Debug.Log(_carRb.centerOfMass);
        if (_centerOfMass != Vector3.zero)
            _carRb.centerOfMass = _centerOfMass;
        _carInput = GetComponent<CarInput>();
    }

    private void FixedUpdate()
    {
        AnimateWheels();
        Move();
        Steer();
        Brake();
    }

    private void Brake()
    {
        if (_carInput.BrakeInput || _carInput.MoveInput == 0)
        {
            foreach (var wheel in _wheels)
                wheel.Brake(_brakeForce, _brakeAcceleration);
        }
        else
        {
            foreach (var wheel in _wheels)
                wheel.StopBrake();
        }
    }

    private void Steer()
    {
        foreach (var wheel in _wheels)
            (wheel as ISteerWheel)?.Steer(_carInput.SteerInput);
    }

    private void Move()
    {
        if (!_carInput.BrakeInput)
            foreach (var wheel in _wheels)
                wheel.Move(_carInput.MoveInput, _speed, _maxAcceleration);
    }

    private void AnimateWheels()
    {
        foreach (var wheel in _wheels)
            wheel.Animate();
    }
}
