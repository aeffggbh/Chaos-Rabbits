
/// <summary>
/// Saves the gameplay scene data
/// </summary>
public class GameplaySceneData : ISceneData
{
    /// <summary>
    /// Saves the index of level 1
    /// </summary>
        
    public static int Level1Index => ServiceProvider.TryGetService<GameSceneController>(out var controller) ? 
        controller.sceneReferenceContainer.Level1Scene : 0;
    /// <summary>
    /// Saves the index of level 2
    /// </summary>
    public static int Level2Index => ServiceProvider.TryGetService<GameSceneController>(out var controller) ? 
        controller.sceneReferenceContainer.Level2Scene : 0;
    /// <summary>
    /// Saves the index of the final level
    /// </summary>
    public static int FinalLevelIndex => ServiceProvider.TryGetService<GameSceneController>(out var controller) ? 
        controller.sceneReferenceContainer.FinalLevelScene : 0;
    public static int Index => Level1Index;
}
