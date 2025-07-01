using UnityEngine;

public interface IPlayerWeaponHandler
{
    Weapon CurrentWeapon { get; set; }
    void GrabWeapon(Weapon weapon);
    void DropWeapon();
    Weapon GetPointedWeapon(Camera camera);
    void GrabPointedWeapon(Camera camera);
}
