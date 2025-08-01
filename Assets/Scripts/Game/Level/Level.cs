using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelObjectiveHandler))]
[RequireComponent(typeof(LevelTrigger))]
public class Level : MonoBehaviour, ILevel
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private Transform _levelSpawn;
    public List<LevelMechanicSO> Mechanics { get => _levelData.Mechanics; set => _levelData.Mechanics = value; }
    public int LevelIndex => _levelData.LevelIndex;

    private void Awake()
    {
        if (_levelData == null)
            Debug.LogError("No level data");

        foreach (var mechanic in Mechanics)
            (mechanic as IInitMechanic)?.Init();
    }

    private void Start()
    {
        if (PlayerMediator.PlayerInstance != null)
            PlayerMediator.PlayerInstance.transform.position = _levelSpawn.position;
    }

    private void Update()
    {
        foreach (var mechanic in Mechanics)
            (mechanic as IUpdateMechanic)?.Update();
    }

    public void Trigger()
    {
        if (LevelIndex == GameplaySceneData.FinalLevelIndex)
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameWinState(), gameObject));
        else if (AllObjectivesCompleted())
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, true));
        else
            EventTriggerManager.Trigger<ILogMessageEvent>(new LogMessageEvent(gameObject, "PENDING OBJECTIVES"));
    }

    public bool AllObjectivesCompleted()
    {
        foreach (var mechanic in Mechanics)
            if (!mechanic.ObjectiveCompleted)
                return false;

        return true;
    }
}
