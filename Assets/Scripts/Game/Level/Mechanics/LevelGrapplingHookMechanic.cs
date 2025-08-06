using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Grappling hook mechanic
/// </summary>
[CreateAssetMenu(fileName = "GrapplingHookMechanic", menuName = "ScriptableObjects/LevelMechanics/GrapplingHookMechanic")]
public class LevelGrapplingHookMechanic : LevelMechanicSO, IMechanicTextInfo, IMechanicInstantiateUser, IMechanicAddComponent
{
    [Header("Level")]
    [SerializeField] private GameObject _playerPrefab;
    [Header("Grappling Hook")]
    [SerializeField] private InputActionReference _grappleAction;
    [SerializeField] private GameObject _ropePrefab;
    [SerializeField] private LayerMask _whatToGrapple;
    public override bool ObjectiveCompleted => true;
    public GameObject UserPrefab => _playerPrefab;

    public void AddNeededComponent(GameObject userObj)
    { 
        var input = userObj?.AddComponent<GrapplingHookInput>();
        input.GrappleAction = _grappleAction;
        var mechanic = userObj?.AddComponent<GrapplingHook>();
        mechanic.GrappleRendererGO = GameObject.Instantiate(_ropePrefab);
        mechanic.WhatToGrapple = _whatToGrapple;
        input.GrapplingHook = mechanic;
        input.Init();
    }

    public string GetObjectiveText()
    {
        return "You now have a grappling hook! Try to use it on the logs. (Press G / L1)";
    }
}
