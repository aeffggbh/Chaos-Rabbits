using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] public List<Weapon> weapons;

    public static WeaponManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        if (weapons.Count == 0)
            Debug.LogWarning("No weapons added!");
    }
}
