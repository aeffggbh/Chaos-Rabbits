using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject, ILevelData
{
    [SerializeField] List<LevelMechanicSO> _mechanicsList;
    [SerializeField] private int _levelIndex;
    public List<LevelMechanicSO> Mechanics { get => _mechanicsList; set => _mechanicsList = value; }
    public int LevelIndex => _levelIndex;

}
