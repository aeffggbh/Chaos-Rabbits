using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GrapplingHookInput))]
public class GrapplingHook : MonoBehaviour
{
    private GameObject _grappleRendererGO;
    private LineRenderer _grappleRenderer;
    private Transform _camera;
    private Transform _origin;
    private Vector3 _grapplePoint;
    private LayerMask _whatToGrapple;
    private SpringJoint _springJoint;
    public GameObject GrappleRendererGO { get => _grappleRendererGO; set => _grappleRendererGO = value; }
    public LayerMask WhatToGrapple { get => _whatToGrapple; set => _whatToGrapple = value; }

    private void Start()
    {
        var playerMediator = GetComponent<PlayerMediator>();
        _camera = playerMediator?.Camera?.transform;
        _origin = playerMediator?.Player?.CurrentWeapon?.TipGO.transform;
        _grappleRenderer = GrappleRendererGO.GetComponent<LineRenderer>();
        _grappleRenderer.positionCount = 0;
    }

    public void OnGrapple(InputAction.CallbackContext context)
    {
        if (RayManager.PointingToObject(_camera, 100f, out var hit, _whatToGrapple))
        {
            _grapplePoint = hit.point;
            _springJoint = gameObject.AddComponent<SpringJoint>();

            _springJoint.maxDistance = hit.distance * 0.5f;
            _springJoint.minDistance = hit.distance * 0.4f;
            _grappleRenderer.positionCount = 2;

            _springJoint.autoConfigureConnectedAnchor = false;
            _springJoint.connectedAnchor = _grapplePoint;
            _springJoint.spring = 7f;
            _springJoint.damper = 7f;
            _springJoint.massScale = 4.5f;
        }
    }

    private void LateUpdate()
    {
        if (!_springJoint)
            return;

        DrawRope();
    }

    private void DrawRope()
    {
        _grappleRenderer.SetPosition(0, _origin.position);
        _grappleRenderer.SetPosition(1, _grapplePoint);
    }

    internal void OnGrappleCancel(InputAction.CallbackContext context)
    {
        _grappleRenderer.positionCount = 0;
        Destroy(_springJoint);
        Debug.Log("DESTROY");
    }

}