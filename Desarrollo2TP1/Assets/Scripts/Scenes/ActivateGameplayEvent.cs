using UnityEngine;

public class ActivateGameplayEvent : IActivateSceneEvent, IUnloadPreviousLevelCommand
{
    private static IScene.Index _index = GameplayScene.Level1Index;
    private static IScene.Index _levelToUnload = IScene.Index.NONE;
    private static IScene.Index _newLevel = IScene.Index.NONE;
    private bool _nextLevel = false;
    private GameObject _source;
    public IScene.Index SceneIndex => _index;
    public bool NewLevel { get => _nextLevel; }
    public IScene.Index NextLevel { get => _index != IScene.Index.FINAL_LEVEL ? _index + 1 : GameplayScene.Level1Index; }
    public GameObject TriggeredByGO { get => _source; }

    public ActivateGameplayEvent(GameObject source, bool nextLevel)
    {
        if (PauseManager.Paused)
            EventTriggerManager.Trigger<IPauseEvent>(new PauseEvent(source));

        MenuManager.Instance.HideAllPanels();
        _levelToUnload = _index;
        _newLevel = NextLevel;
        _nextLevel = nextLevel;
        _source = source;
    }

    public ActivateGameplayEvent(GameObject source, IScene.Index level)
    {
        if (PauseManager.Paused)
            PauseManager.Paused = false;

        MenuManager.Instance.HideAllPanels();
        _levelToUnload = _index;
        _newLevel = level;
        _nextLevel = true;
        _source = source;
    }

    public void UnloadPreviousLevel()
    {
        if (_nextLevel)
        {
            IScene.Index levelToUnload = _levelToUnload == IScene.Index.NONE ? _index : _levelToUnload;
            IScene.Index levelToLoad = _newLevel == IScene.Index.NONE ? NextLevel : _newLevel;

            _nextLevel = false;

            SceneLoader.Instance.UnloadScene(levelToUnload);

            _index = levelToLoad;

            _levelToUnload = IScene.Index.NONE;
            _newLevel = IScene.Index.NONE;
        }
    }
}
