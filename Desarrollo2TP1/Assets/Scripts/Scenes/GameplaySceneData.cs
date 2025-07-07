using UnityEngine;

public class GameplaySceneData : ISceneData
{
    private static int _levelStateIndex;
    public static int Level1Index => GameSceneController.Instance.sceneReferenceContainer.Level1Scene;
    public static int Level2Index => GameSceneController.Instance.sceneReferenceContainer.Level2Scene;
    public static int FinalLevelIndex => GameSceneController.Instance.sceneReferenceContainer.FinalLevelScene;
    public static int Index => _levelStateIndex;
}
