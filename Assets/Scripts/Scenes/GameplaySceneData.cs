
/// <summary>
/// Saves the gameplay scene data
/// </summary>
public class GameplaySceneData : ISceneData
{
    /// <summary>
    /// Saves the index of level 1
    /// </summary>
    public static int Level1Index => GameSceneController.Instance.sceneReferenceContainer.Level1Scene;
    /// <summary>
    /// Saves the index of level 2
    /// </summary>
    public static int Level2Index => GameSceneController.Instance.sceneReferenceContainer.Level2Scene;
    /// <summary>
    /// Saves the index of the final level
    /// </summary>
    public static int FinalLevelIndex => GameSceneController.Instance.sceneReferenceContainer.FinalLevelScene;
    public static int Index => Level1Index;
}
