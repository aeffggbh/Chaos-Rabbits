
using UnityEngine;

public interface IPlayerData : IHealthData, IWeaponUser, IPhysicsMovementData 
{
    Transform WeaponParent { get; set; }
}