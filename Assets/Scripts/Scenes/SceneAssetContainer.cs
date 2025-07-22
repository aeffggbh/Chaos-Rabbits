using UnityEditor;
using UnityEngine;

/// <summary>
/// Saves the assets of all the scenes in the game.
/// </summary>
[CreateAssetMenu(fileName = "SceneReferenceContainer", menuName = "ScriptableObjects/SceneReference")]
public class SceneAssetContainer : ScriptableObject
{
    [SerializeField] private int _bootSceneIndex;
    [SerializeField] private int _menuSceneIndex;
    [SerializeField] private int _level1SceneIndex;
    [SerializeField] private int _level2SceneIndex;
    [SerializeField] private int _finalLevelSceneIndex;

    /// <summary>
    /// Saves the boot scene's index
    /// </summary>
    public int BootScene { get => _bootSceneIndex; }
    /// <summary>
    /// Saves the menu scene's index
    /// </summary>
    public int MenuScene { get => _menuSceneIndex; }
    /// <summary>
    /// Saves the level 1 scene index
    /// </summary>
    public int Level1Scene { get => _level1SceneIndex; }
    /// <summary>
    /// Saves the level 2 scene index
    /// </summary>
    public int Level2Scene { get => _level2SceneIndex; }
    /// <summary>
    /// Saves de final level scene index
    /// </summary>
    public int FinalLevelScene { get => _finalLevelSceneIndex; }


#if UNITY_EDITOR
    [SerializeField] private SceneAsset _bootScene;
    [SerializeField] private SceneAsset _menuScene;
    [SerializeField] private SceneAsset _level1Scene;
    [SerializeField] private SceneAsset _level2Scene;
    [SerializeField] private SceneAsset _finalLevelScene;

    /// <summary>
    /// Saves the indexes of the provided scenes
    /// </summary>
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
