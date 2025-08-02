using UnityEngine;
/// <summary>
/// Handles the buttons in the menus
/// </summary>
public class MenuButtonHandler : MonoBehaviour
{
    private UIAudioHandler _audio;
    private MenuManager _menuManager;

    private void Start()
    {
        ServiceProvider.TryGetService<UIAudioHandler>(out _audio);
        ServiceProvider.TryGetService<MenuManager>(out _menuManager);
    }

    /// <summary>
    /// Transitions to game over menu
    /// </summary>
    public void ToGameOver()
    {
        _audio.PlayButtonSound();
        _menuManager.TransitionToState(new GameOverState());
    }
    /// <summary>
    /// Transitions to game win menu
    /// </summary>
    public void ToGameWin()
    {
        _audio.PlayButtonSound();
        _menuManager.TransitionToState(new GameWinState());
    }
    /// <summary>
    /// Transitions to credits menu
    /// </summary>
    public void ToCredits()
    {
        _audio.PlayButtonSound();
        _menuManager.TransitionToState(new CreditsState());
    }
    /// <summary>
    /// Transitions to check exit menu
    /// </summary>
    public void ToCheckExit()
    {
        _audio.PlayButtonSound();
        _menuManager.TransitionToState(new CheckExitState());
    }
    /// <summary>
    /// Transitions to main menu
    /// </summary>
    public void ToMainMenu()
    {
        _audio.PlayButtonSound();
        PlayerPreservedData.BlockSaving = true;
        _menuManager.TransitionToState(new MainMenuState());
    }
    /// <summary>
    /// Transitions to previous menu
    /// </summary>
    public void ToPreviousMenu()
    {
        _audio.PlayButtonSound();
        IMenuState previousState = _menuManager.PreviousState;
        _menuManager.TransitionToState(previousState);
    }
    /// <summary>
    /// Switches the scene to gameplay
    /// </summary>
    public void Unpause()
    {
        _audio.PlayButtonSound();
        EventTriggerManager.Trigger<IPauseEvent>(new PauseEvent(gameObject));
    }
    /// <summary>
    /// Switches the scene to level 1 specifically
    /// </summary>
    public void ToLevel1()
    {
        _audio.PlayButtonSound();
        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, false, GameplaySceneData.Level1Index));
    }
    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        _audio.PlayButtonSound();
        EventTriggerManager.Trigger<IExitGameEvent>(new ExitGameEvent(gameObject));
    }
}