using UnityEngine;
//TODO: improve this

/// <summary>
/// Handles the buttons in the menus
/// </summary>
public class MenuButtonHandler : MonoBehaviour
{
    /// <summary>
    /// Transitions to game over menu
    /// </summary>
    public void ToGameOver()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new GameOverState());
    }
    /// <summary>
    /// Transitions to game win menu
    /// </summary>
    public void ToGameWin()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new GameWinState());
    }
    /// <summary>
    /// Transitions to credits menu
    /// </summary>
    public void ToCredits()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new CreditsState());
    }
    /// <summary>
    /// Transitions to check exit menu
    /// </summary>
    public void ToCheckExit()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new CheckExitState());
    }
    /// <summary>
    /// Transitions to main menu
    /// </summary>
    public void ToMainMenu()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new MainMenuState());
    }
    /// <summary>
    /// Transitions to previous menu
    /// </summary>
    public void ToPreviousMenu()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        IMenuState previousState = MenuManager.Instance.PreviousState;
        MenuManager.Instance.TransitionToState(previousState);
    }
    /// <summary>
    /// Switches the scene to gameplay
    /// </summary>
    public void Unpause()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IPauseEvent>(new PauseEvent(gameObject));
    }
    /// <summary>
    /// Switches the scene to level 1 specifically
    /// </summary>
    public void ToLevel1()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, false, GameplaySceneData.Level1Index));
    }
    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IExitGameEvent>(new ExitGameEvent(gameObject));
    }
}