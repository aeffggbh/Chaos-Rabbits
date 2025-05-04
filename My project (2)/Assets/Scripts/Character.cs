using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Moves the character, controls everything related to the world and the position.
/// </summary>
/// 
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    //private HashSet<ForceRequest> instantForceRequests;
    //private HashSet<ForceRequest> continuousForceRequests;
    private ForceRequest _forceRequest;
    private Rigidbody rb;
    private bool _isJumping;
    private float _jumpForce;
    private bool _grounded;
    private Vector3 _calculatedMovement;

    private void Awake()
    {
        _forceRequest = null;
        _jumpForce = 0f;
        _isJumping = false;
        _grounded = true;

        rb = GetComponent<Rigidbody>();
    }

    public void RequestForce(ForceRequest forceRequest)
    {
        //instantForceRequests.Add(forceRequest);
        _forceRequest = forceRequest;
    }

    public void RequestJumpInfo(bool isJumping, float jumpForce)
    {
        _isJumping = isJumping;
        _jumpForce = jumpForce;
    }

    public void RequestGroundedState(bool grounded)
    {
        _grounded = grounded;
    }

    public void RequestMovement(Vector3 calculatedMovement)
    {
        _calculatedMovement = calculatedMovement;
    }

    private void FixedUpdate()
    {
        //MOVEMENT
        {
            if (_forceRequest != null)
            {
                if (_forceRequest.direction != _calculatedMovement)
                    _forceRequest.direction = _calculatedMovement;

                if (_forceRequest.forceMode == ForceMode.Impulse)
                {
                    _forceRequest._counterMovement = new Vector3
                        (-rb.linearVelocity.x * _forceRequest._counterMovementForce,
                        0,
                        -rb.linearVelocity.z * _forceRequest._counterMovementForce);

                    rb.AddForce((_forceRequest.direction * _forceRequest.speed + _forceRequest._counterMovement) * Time.fixedDeltaTime,
                                ForceMode.Impulse);
                }
                else if (_forceRequest.forceMode == ForceMode.Force)
                {
                    //If i wanted a continous force I'd want to give it an input for direction only once. I wouldn't want to tell it to stop when the input is 0,0,0.
                    var speedPercentage = rb.linearVelocity.magnitude / _forceRequest.speed;
                    //limitar el valor entre 0 y 1
                    var remainingSpeedPercentage = Mathf.Clamp01(1f - speedPercentage);

                    //if (rb.linearVelocity.magnitude < continuousForceRequests.speed)
                    //El .force se va a multiplicar por fixeddelta internamente. el .impulse no.
                    rb.AddForce(_forceRequest.direction * _forceRequest.acceleration * remainingSpeedPercentage, ForceMode.Force);
                }
            }

        }

        //JUMPING
        {
            if (_isJumping)
            {
                if (_grounded)
                {
                    Jump();
                }

                _isJumping = false;
            }
        }
    }


    private void Jump()
    {
        _grounded = false;
        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }


}
