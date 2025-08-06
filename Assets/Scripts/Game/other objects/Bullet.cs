using System;
using UnityEngine;

/// <summary>
/// Represents a bullet that can be fired by a weapon.
/// </summary>
[RequireComponent(typeof(BulletPhysics))]
internal class Bullet : MonoBehaviour
{
    private IWeaponUser _whoIsFiring;
    private BulletPhysics _bulletPhysics;
    private GameObject _originWeapon;

    private void Awake()
    {
        _bulletPhysics = GetComponent<BulletPhysics>();
    }

    /// <summary>
    /// Fires the bullet from a specified parent transform towards an opponent type with a specified damage value.
    /// </summary>
    /// <param name="wParent"></param>
    /// <param name="opponentType"></param>
    /// <param name="damage"></param>
    public void Fire(Transform wParent, IWeaponUser whoIsFiring)
    {
        Debug.Log("Firing bullet from " + wParent.name + " by " + whoIsFiring);

        _whoIsFiring = whoIsFiring;
        if (_whoIsFiring.CurrentWeapon != null)
            _originWeapon = whoIsFiring.CurrentWeapon.gameObject;
        else
        {
            Debug.LogError("user has no weapon to fire with..");
            return;
        }

        _bulletPhysics.Fire(wParent);
    }

    /// <summary>
    /// Handles the collision of the bullet with other objects.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("LevelTrigger"))
            HandleCollision(collision.gameObject);
    }

    /// <summary>
    /// Handles the bullet collision
    /// </summary>
    /// <param name="collision"></param>
    private void HandleCollision(GameObject collision)
    {
        if (ShouldIgnoreCollision(collision))
            return;

        TryApplyDamage(collision);

        Destroy(gameObject);
    }

    /// <summary>
    /// Tries to apply damage to the collision
    /// </summary>
    /// <param name="collision"></param>
    private void TryApplyDamage(GameObject collision)
    {
        var damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeDamage(_whoIsFiring.Damage);
    }

    /// <summary>
    /// Returns true if the bullet should ignore the incoming collision
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    private bool ShouldIgnoreCollision(GameObject collision)
    {
        if (collision != null && _originWeapon != null && _whoIsFiring != null)
            return collision == _originWeapon || collision == _whoIsFiring.UserObject;

        return true;
    }
}