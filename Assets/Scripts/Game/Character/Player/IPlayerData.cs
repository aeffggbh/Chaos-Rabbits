
using UnityEngine;

/// <summary>
/// Saves all the data of the player
/// </summary>
public interface IPlayerData : IHealthData, IWeaponUser, IPhysicsMovementData 
{
    /// <summary>
    /// Saves the weapon parent
    /// </summary>
    Transform WeaponParent { get; set; }
}