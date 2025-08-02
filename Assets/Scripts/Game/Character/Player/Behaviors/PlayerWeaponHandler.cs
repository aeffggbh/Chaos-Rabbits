using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerWeaponHandler : IPlayerWeaponHandler
{
    public Weapon CurrentWeapon { get; set; }

    private Transform _weaponParent;
    private GameObject _bulletSpawnGO;
    private float _maxWeaponDistance;
    PlayerAnimationController _animationController;
    PlayerMediator _playerController;
    GameObject _defaultWeaponPrefab;
    Weapon _defaultWeapon;

    public PlayerWeaponHandler(
        GameObject bulletSpawn,
        float maxWeaponDistance,
        PlayerAnimationController playerAnimation,
        Transform weaponParent,
        GameObject defaultWeaponPrefab,
        WeaponData weaponToRestore = null)
    {
        _bulletSpawnGO = bulletSpawn;
        _maxWeaponDistance = maxWeaponDistance;
        _animationController = playerAnimation;
        _weaponParent = weaponParent;

        ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
        _playerController = mediator;

        CurrentWeapon = _playerController.Player.CurrentWeapon;

        Weapon weaponToGrab = null;

        if (weaponToRestore && weaponToRestore.prefab && !CurrentWeapon)
        {
            GameObject weaponGO = GameObject.Instantiate(weaponToRestore.prefab);
            Weapon weapon = weaponGO.GetComponent<Weapon>();

            if (weapon)
            {
                weapon.WeaponData = weaponToRestore;
                weaponToGrab = weapon;
            }
            else
                Debug.LogError("Object " + weaponGO + " has no weapon component");
        }
        else if (!CurrentWeapon)
        {
            GameObject fallback = GameObject.Instantiate(defaultWeaponPrefab);

            Weapon fall = fallback.GetComponent<Weapon>();

            weaponToGrab = fall;
        }
        else
            weaponToGrab = CurrentWeapon;

        GrabWeapon(weaponToGrab);

        EventProvider.Subscribe<INewLevelEvent>(OnNextLevel);
    }

    private void OnNextLevel(INewLevelEvent levelEvent)
    {
        if (CurrentWeapon == null)
        {
            GameObject.Instantiate(_defaultWeaponPrefab);
            GrabWeapon(_defaultWeapon);
        }
    }

    /// <summary>
    /// Returns the weapon that the player is currently pointing to, if any.
    /// </summary>
    /// <returns></returns>
    public Weapon GetPointedWeapon(CinemachineCamera camera)
    {
        RaycastHit? hit = null;

        if (RayManager.PointingToObject(camera.transform, _maxWeaponDistance, out RaycastHit hitInfo))
            hit = hitInfo;

        if (hit != null)
        {
            Weapon weapon = hit.Value.collider.gameObject.GetComponent<Weapon>();

            if (weapon)
                return weapon;
        }

        return null;

    }

    public void GrabPointedWeapon(CinemachineCamera camera)
    {
        Weapon pointedWeapon = GetPointedWeapon(camera);
        if (pointedWeapon)
            GrabWeapon(pointedWeapon);
    }

    public void GrabWeapon(Weapon weapon)
    {
        if (!weapon)
            return;

        if (CurrentWeapon != null)
            DropWeapon();

        CurrentWeapon = weapon;
        ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
        CurrentWeapon.user = mediator.Player;
        CurrentWeapon.BulletSpawnGO = _bulletSpawnGO;
        CurrentWeapon.Hold(_weaponParent);
        _playerController.Player.CurrentWeapon = CurrentWeapon;
        _animationController?.AnimateGrabWeapon();
    }

    public void DropWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.user = null;
            CurrentWeapon.Drop();
            CurrentWeapon = null;
            _animationController?.AnimateDropWeapon();
        }
    }
}