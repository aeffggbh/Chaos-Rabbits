using System;
using UnityEngine;

/// <summary>
/// Represents a bullet that can be fired by a weapon.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
internal class Bullet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Rigidbody _rb;
    private Character _whoIsFiring;
    private Weapon _originWeapon;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Fires the bullet from a specified parent transform towards an opponent type with a specified damage value.
    /// </summary>
    /// <param name="wParent"></param>
    /// <param name="opponentType"></param>
    /// <param name="damage"></param>
    public void Fire(Transform wParent, Character whoIsFiring, Weapon weapon)
    {
        Debug.Log("Firing bullet from " + wParent.name + " with damage: " + whoIsFiring.damage);

        _rb.AddForce(force * Time.fixedDeltaTime * wParent.forward, ForceMode.Impulse);

        _whoIsFiring = whoIsFiring;

        _originWeapon = weapon;
    }

    /// <summary>
    /// Handles the collision of the bullet with other objects.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        Character hitCharacter = collision.gameObject.GetComponent<Character>();

        if (hitCharacter != null && hitCharacter != _whoIsFiring)
        {
            hitCharacter.TakeDamage(_whoIsFiring.damage);
            Debug.Log("Shot " + hitCharacter.name + " for " + _whoIsFiring.damage + " damage");
        }

        if (collision.gameObject != _originWeapon && collision.gameObject != _whoIsFiring.gameObject)
            Destroy(gameObject);
    }
}