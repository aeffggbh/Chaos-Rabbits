using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneReferenceContainer", menuName = "ScriptableObjects/SceneReference")]
public class SceneAssetContainer : ScriptableObject
{
    [SerializeField] private int _bootSceneIndex;
    [SerializeField] private int _menuSceneIndex;
    [SerializeField] private int _level1SceneIndex;
    [SerializeField] private int _level2SceneIndex;
    [SerializeField] private int _finalLevelSceneIndex;

    public int BootScene { get => _bootSceneIndex; }
    public int MenuScene { get => _menuSceneIndex; }
    public int Level1Scene { get => _level1SceneIndex; }
    public int Level2Scene { get => _level2SceneIndex; }
    public int FinalLevelScene { get => _finalLevelSceneIndex; }



#if UNITY_EDITOR
    [SerializeField] private SceneAsset _bootScene;
    [SerializeField] private SceneAsset _menuScene;
    [SerializeField] private SceneAsset _level1Scene;
    [SerializeField] private SceneAsset _level2Scene;
    [SerializeField] private SceneAsset _finalLevelScene;

    private void OnValidate()
    {
        _bootSceneIndex = SceneLoader.GetIndex(_bootScene);
        _menuSceneIndex = SceneLoader.GetIndex(_menuScene);
        _level1SceneIndex = SceneLoader.GetIndex(_level1Scene);
        _level2SceneIndex = SceneLoader.GetIndex(_level2Scene);
        _finalLevelSceneIndex = SceneLoader.GetIndex(_finalLevelScene);
    }
#endif
}
