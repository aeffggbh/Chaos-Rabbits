using UnityEngine;

internal class NewLevelEvent : INewLevelEvent
{
    private GameObject _triggeredByGO;
    private int _previousLevelIndex;
    public GameObject TriggeredByGO => _triggeredByGO;
    public int PreviousLevelIndex => _previousLevelIndex;
    
    public NewLevelEvent(GameObject triggeredByGO, int levelToUnloadIndex)
    {
        _triggeredByGO = triggeredByGO;
        _previousLevelIndex = levelToUnloadIndex;
    }
}