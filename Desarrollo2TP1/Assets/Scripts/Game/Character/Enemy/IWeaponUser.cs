using UnityEngine;

/// <summary>
/// Interface for the weapon users
/// </summary>
public interface IWeaponUser : IDamageable
{
    /// <summary>
    /// Saves the weapon that the user is holding
    /// </summary>
    Weapon CurrentWeapon { get; set; }
    /// <summary>
    /// Saves the gameobject of the user
    /// </summary>
    GameObject UserObject { get; }
    /// <summary>
    /// Saves the damage of the user
    /// </summary>
    float Damage { get; }
}