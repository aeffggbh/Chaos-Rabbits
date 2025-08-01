using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataContainer", menuName = "ScriptableObjects/WeaponDataContainer")]
public class WeaponDataContainer : ScriptableObject
{
    [SerializeField] private List<WeaponData> _weapons;

    private Dictionary<string, WeaponData> _weaponContainer;

    public WeaponData GetWeapon(string id)
    {
        if (_weaponContainer == null)
        {
            _weaponContainer = new();

            foreach (var weapon in _weapons)
                _weaponContainer[weapon.id] = weapon;
        }

        return _weaponContainer.TryGetValue(id, out var data) ? data : null;
    }
}
