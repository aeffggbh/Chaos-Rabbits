using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] public List<Weapon> weapons;

    private void OnEnable()
    {
        if (weapons.Count == 0)
            Debug.LogWarning("No weapons added!");
    }
}
