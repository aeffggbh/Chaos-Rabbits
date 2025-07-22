using UnityEngine;

//TODO: Ask for the scene loader as a serialized field instead of making it a singleton.

/// <summary>
/// Controls which scenes are active, according to what's going on in the game
/// </summary>
public class GameSceneController : MonoBehaviour
{
    /// <summary>
    /// Saves the scene asset container (the scriptable object)
    /// </summary>
    [SerializeField] public SceneAssetContainer sceneReferenceContainer;
    /// <summary>
    /// Instance of the game scene controller
    /// </summary>
    public static GameSceneController Instance;

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

    /// <summary>
    /// If the indicated type of scene is loaded, then it returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsTypeLoaded<T>() where T : ISceneData
    {
        return SceneLoader.Instance.IsTypeLoaded<T>();
    }

    /// <summary>
    /// Sets an active scene given a scene event
    /// </summary>
    /// <param name="scene"></param>
    public void SetActiveScene(IActivateSceneEvent scene)
    {
        (scene as IUnloadPreviousLevelCommand)?.UnloadPreviousLevel();

        int index = scene.Index;

        var activeScene = SceneLoader.Instance.GetActiveScene();

        if (activeScene == index)
        {
            Debug.LogWarning($"tried to activate a scene that is already active. Did not do it. Index: {activeScene} Source: {scene.TriggeredByGO.name}");
            return;
        }

        if (!IsSceneLoaded(index))
            SceneLoader.Instance.LoadScene(index);
        else
            SceneLoader.Instance.SetActiveScene(index);
    }

    /// <summary>
    /// Checks if a scene is loaded given an index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsSceneLoaded(int index)
    {
        return SceneLoader.Instance.IsSceneLoaded(index);
    }

    /// <summary>
    /// Checks if the cursor is visible and locked given a scene event.
    /// </summary>
    /// <param name="sceneEvent"></param>
    private void CheckCursor(IActivateSceneEvent sceneEvent)
    {
        var index = sceneEvent.Index;

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

    /// <summary>
    /// Unloads all the gameplay levels
    /// </summary>
    public void UnloadGameplay()
    {
        SceneLoader.Instance.UnloadGameplay();
    }

    /// <summary>
    /// If the scene in the index is gameplay, it returns true
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsGameplay(int index)
    {
        return SceneLoader.Instance.IsGameplay(index);
    }
}
