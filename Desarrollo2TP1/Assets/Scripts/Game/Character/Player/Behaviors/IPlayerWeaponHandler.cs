using UnityEngine;

/// <summary>
/// Handles the weapon of the player
/// </summary>
public interface IPlayerWeaponHandler
{
    /// <summary>
    /// Saves the weapon that the player is holding
    /// </summary>
    Weapon CurrentWeapon { get; set; }
    /// <summary>
    /// Grabs the weapon provided by parameter
    /// </summary>
    /// <param name="weapon"></param>
    void GrabWeapon(Weapon weapon);
    /// <summary>
    /// Drops the weapon that you're currently holding
    /// </summary>
    void DropWeapon();
    /// <summary>
    /// Returns the weapon that you're pointing at
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    Weapon GetPointedWeapon(Camera camera);
    /// <summary>
    /// Grabs the weapon that the camera is pointing at
    /// </summary>
    /// <param name="camera"></param>
    void GrabPointedWeapon(Camera camera);
}
