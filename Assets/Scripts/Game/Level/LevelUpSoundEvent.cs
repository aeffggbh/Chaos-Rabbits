using UnityEngine;

/// <summary>
/// Is called when the level up sound should be played
/// </summary>
public class LevelUpSoundEvent : ILevelUpSoundEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public LevelUpSoundEvent(GameObject TriggeredByGO)
    {
        _triggeredByGO = TriggeredByGO;
    }
}