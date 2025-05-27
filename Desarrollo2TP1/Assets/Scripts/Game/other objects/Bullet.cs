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
    private float _damage;

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
    public void Fire(Transform wParent, Type opponentType, float damage)
    {
        _rb.AddForce(wParent.transform.forward * force * Time.fixedDeltaTime, ForceMode.Impulse);

        _damage = damage;
    }

    /// <summary>
    /// Handles the collision of the bullet with other objects.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        Character hitCharacter = collision.gameObject.GetComponent<Character>();

        if (hitCharacter != null)
        {
            if (hitCharacter is Enemy || hitCharacter is Player)
            {
                hitCharacter.TakeDamage(_damage);
                Debug.Log("Shot " + hitCharacter.name + " for " + _damage + " damage");
            }
        }

        Destroy(this.gameObject);
    }
}