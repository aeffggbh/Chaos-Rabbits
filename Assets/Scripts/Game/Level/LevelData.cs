using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject, ILevelData
{
    [SerializeField] List<LevelMechanicSO> _mechanicsList;
    [SerializeField] private int _levelIndex;
    /// <summary>
    /// Saves the mechanics that the level has
    /// </summary>
    public List<LevelMechanicSO> Mechanics { get => _mechanicsList; set => _mechanicsList = value; }
    /// <summary>
    /// Saves the level index
    /// </summary>
    public int LevelIndex => _levelIndex;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;

    private void OnValidate()
    {
        _levelIndex = SceneLoader.GetIndex(_sceneAsset);
    }
#endif
}
