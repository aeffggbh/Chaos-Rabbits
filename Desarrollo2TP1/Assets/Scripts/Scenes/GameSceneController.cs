using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls which scenes are active, according to what's going on in the game
/// </summary>
public class GameSceneController : MonoBehaviour
{
    [SerializeField] public SceneAssetContainer sceneReferenceContainer;
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

    public bool IsTypeLoaded<T>() where T : ISceneData
    {
        return SceneLoader.Instance.IsTypeLoaded<T>();
    }

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

    public bool IsSceneLoaded(int index)
    {
        return SceneLoader.Instance.IsSceneLoaded(index);
    }

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

    internal void UnloadGameplay()
    {
        SceneLoader.Instance.UnloadGameplay();
    }

    public bool IsGameplay(int index)
    {
        return SceneLoader.Instance.IsGameplay(index);
    }
}
