using UnityEngine;

public interface IWeaponUser : IDamageable
{
    Weapon CurrentWeapon { get; set; }
    GameObject UserObject { get; }
    float Damage { get; }
}