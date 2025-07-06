
using System;

public class MenuScene : IScene
{
    public MenuScene(IMenuState menuEnter)
    {
        if (MenuManager.Instance)
            MenuManager.Instance.TransitionToState(menuEnter);
        else
            MenuManager.Instance.TransitionToState(new MainMenuState());
    }

    public bool IsPersistent => false;

    public static IScene.Index MenuIndex => IScene.Index.MENU;
    public IScene.Index buildIndex => MenuIndex;
}
