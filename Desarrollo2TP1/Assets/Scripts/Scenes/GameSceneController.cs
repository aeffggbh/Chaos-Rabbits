using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls which scenes are active, according to what's going on in the game
/// </summary>
public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Instance;
    private IScene.Index _previousScene;
    public IScene.Index PreviousScene => _previousScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        EventProvider.Subscribe<IActivateSceneEvent>(SetActiveScene);
        EventProvider.Subscribe<IActivateSceneEvent>(CheckCursor);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IActivateSceneEvent>(SetActiveScene);
        EventProvider.Unsubscribe<IActivateSceneEvent>(CheckCursor);
    }

    public bool IsTypeLoaded<T>() where T : IScene
    {
        return SceneLoader.Instance.IsTypeLoaded<T>();
    }

    public void SetActiveScene(IActivateSceneEvent scene)
    {
        IScene.Index index = scene.SceneIndex;

        var activeScene = SceneLoader.Instance.GetActiveScene();

        if (activeScene == index)
        {
            Debug.LogWarning($"tried to activate a scene that is already active. Did not do it. Index: {activeScene} Source: {scene.TriggeredByGO.name}");
            return;
        }

        (scene as IUnloadPreviousLevelCommand)?.UnloadPreviousLevel();

        index = scene.SceneIndex;

        if (!IsSceneLoaded(index))
            SceneLoader.Instance.LoadScene(index);

        if (SceneLoader.Instance.SetActiveScene(index))
        {
            _previousScene = activeScene;
        }
        else
            Debug.LogWarning($"Could not activate {index} Source: {scene.TriggeredByGO.name} (perhaps it's still loading)");

    }

    public bool IsSceneLoaded(IScene.Index index)
    {
        return SceneLoader.Instance.IsSceneLoaded(index);
    }

    private void CheckCursor(IActivateSceneEvent sceneEvent)
    {
        var index = sceneEvent.SceneIndex;

        if (IsGameplay(index))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    internal void UnloadGameplay()
    {
        SceneLoader.Instance.UnloadGameplay();
    }

    public bool IsGameplay(IScene.Index index)
    {
        return SceneLoader.Instance.IsGameplay((int)index);
    }
}
