using System;
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

    public PlayerWeaponHandler(GameObject bulletSpawn, float maxWeaponDistance, PlayerAnimationController playerAnimation, Transform weaponParent, GameObject defaultWeaponPrefab)
    {
        _bulletSpawnGO = bulletSpawn;
        _maxWeaponDistance = maxWeaponDistance;
        _animationController = playerAnimation;
        _weaponParent = weaponParent;

        _playerController = PlayerMediator.PlayerInstance;

        CurrentWeapon = _playerController.Player.CurrentWeapon;

        if (CurrentWeapon != null)
            GrabWeapon(CurrentWeapon);

        _defaultWeaponPrefab = defaultWeaponPrefab;
        _defaultWeapon = defaultWeaponPrefab.GetComponent<Weapon>();

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
    public Weapon GetPointedWeapon(Camera camera)
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

    public void GrabPointedWeapon(Camera camera)
    {
        Weapon pointedWeapon = GetPointedWeapon(camera);
        if (pointedWeapon)
            GrabWeapon(pointedWeapon);
    }

    public void GrabWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
            DropWeapon();

        CurrentWeapon = weapon;
        CurrentWeapon.user = PlayerMediator.PlayerInstance.Player;
        CurrentWeapon.SetBulletSpawn(_bulletSpawnGO);
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