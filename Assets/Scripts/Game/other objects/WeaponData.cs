using UnityEngine;

/// <summary>
/// Saves the data of a weapon
/// </summary>
[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string id;
    public GameObject prefab;
}
