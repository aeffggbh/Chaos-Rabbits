
/// <summary>
/// Saves data for the menu scene
/// </summary>
public class MenuSceneData : ISceneData
{
    public static int Index => ServiceProvider.TryGetService<GameSceneController>(out var controller) ? controller.sceneReferenceContainer.MenuScene : 0;
}
