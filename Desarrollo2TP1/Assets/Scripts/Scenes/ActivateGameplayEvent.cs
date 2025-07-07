using UnityEditor;
using UnityEngine;

public class ActivateGameplayEvent : IActivateSceneEvent, IUnloadPreviousLevelCommand
{
    private static int _currentIndex = -1;
    private static int _levelToUnloadIndex = -1;
    private static int _newLevelIndex = -1;
    private bool _nextLevel = false;
    private GameObject _source;
    public int Index => _currentIndex;
    public bool NewLevel { get => _nextLevel; }
    public int NextLevel { get => _currentIndex != GameplaySceneData.FinalLevelIndex ? _currentIndex + 1 : GameplaySceneData.Level1Index; }
    public GameObject TriggeredByGO { get => _source; }

    public ActivateGameplayEvent(GameObject source, bool nextLevel)
    {
        if (PauseManager.Paused)
            EventTriggerManager.Trigger<IPauseEvent>(new PauseEvent(source));

        MenuManager.Instance.HideAllPanels();
        _levelToUnloadIndex = _currentIndex;
        _newLevelIndex = NextLevel;
        _nextLevel = nextLevel;
        _source = source;
    }

    public ActivateGameplayEvent(GameObject source, int levelIndex)
    {
        if (PauseManager.Paused)
            PauseManager.Paused = false;

        MenuManager.Instance.HideAllPanels();
        _levelToUnloadIndex = _currentIndex;
        _newLevelIndex = levelIndex;
        _nextLevel = true;
        _source = source;
    }

    public void UnloadPreviousLevel()
    {
        if (_nextLevel)
        {

            int levelToUnloadIndex = _levelToUnloadIndex == -1 ? _currentIndex : _levelToUnloadIndex;
            int levelToLoadIndex = _newLevelIndex == -1 ? NextLevel : _newLevelIndex;

            _nextLevel = false;

            SceneLoader.Instance.UnloadScene(levelToUnloadIndex);

            _currentIndex = levelToLoadIndex;

            if (levelToLoadIndex != GameplaySceneData.Level1Index)
                UIAudioHandler.Instance.PlayLevelUpSound();

            _levelToUnloadIndex = -1;
            _newLevelIndex = -1;
        }
    }

    public void GetIndex()
    {
        throw new System.NotImplementedException();
    }
}
