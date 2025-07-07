using UnityEngine;

public class GameplayScene : IScene
{
    private IScene.Index _levelState;

    public static IScene.Index Level1Index => IScene.Index.LEVEL1;
    public static IScene.Index Level2Index => IScene.Index.LEVEL2;
    public static IScene.Index FinalLevelIndex => IScene.Index.FINAL_LEVEL;

    public IScene.Index buildIndex => _levelState;

    public bool IsPersistent => false;

    public GameplayScene(IScene.Index level)
    {
        if (level < IScene.Index.LEVEL1 || level > IScene.Index.FINAL_LEVEL)
        {
            Debug.LogError("tried to load a level out of bounds");
            return;
        }

        _levelState = level;
    }
}
