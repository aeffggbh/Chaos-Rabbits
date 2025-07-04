using UnityEngine;

public interface IWeaponUser : IDamageable
{
    Weapon CurrentWeapon { get; set; }
    GameObject gameObject { get; }
    float Damage { get; }
}