using System;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Handles the buttons in the menus
/// </summary>
public class MenuButtonHandler : MonoBehaviour
{
    private MenuManager _menuManager;

    private void Start()
    {
        ServiceProvider.TryGetService<MenuManager>(out _menuManager);
    }

    /// <summary>
    /// Transitions to game over menu
    /// </summary>
    public void ToGameOver()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        _menuManager.TransitionToState(new GameOverState());
    }
    /// <summary>
    /// Transitions to game win menu
    /// </summary>
    public void ToGameWin()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        _menuManager.TransitionToState(new GameWinState());
    }
    /// <summary>
    /// Transitions to credits menu
    /// </summary>
    public void ToCredits()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        _menuManager.TransitionToState(new CreditsState());
    }
    /// <summary>
    /// Transitions to check exit menu
    /// </summary>
    public void ToCheckExit()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        _menuManager.TransitionToState(new CheckExitState());
    }
    /// <summary>
    /// Transitions to main menu
    /// </summary>
    public void ToMainMenu()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        PlayerPreservedData.BlockSaving = true;
        _menuManager.TransitionToState(new MainMenuState());
    }
    /// <summary>
    /// Transitions to previous menu
    /// </summary>
    public void ToPreviousMenu()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        IMenuState previousState = _menuManager.PreviousState;
        _menuManager.TransitionToState(previousState);
    }
    /// <summary>
    /// Switches the scene to gameplay
    /// </summary>
    public void Unpause()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent(gameObject));
    }
    /// <summary>
    /// Switches the scene to level 1 specifically
    /// </summary>
    public void ToLevel1()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, false, GameplaySceneData.Level1Index));
    }
    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        EventTriggerer.Trigger<IButtonClickEvent>(new ButtonClickEvent(gameObject));
        EventTriggerer.Trigger<IExitGameEvent>(new ExitGameEvent(gameObject));
    }
}
