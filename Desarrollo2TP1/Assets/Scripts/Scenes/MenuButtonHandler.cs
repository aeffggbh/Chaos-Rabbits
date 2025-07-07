using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{
    public void ToGameOver()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new GameOverState());
    }

    public void ToGameWin()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new GameWinState());
    }

    public void ToCredits()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new CreditsState());
    }

    public void ToCheckExit()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new CheckExitState());
    }

    public void ToMainMenu()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        MenuManager.Instance.TransitionToState(new MainMenuState());
    }

    public void ToPreviousMenu()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        IMenuState previousState = MenuManager.Instance.PreviousState;
        MenuManager.Instance.TransitionToState(previousState);
    }

    public void ToGameplay()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, false));
    }

    public void ToLevel1()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, GameplaySceneData.Level1Index));
    }

    public void ExitGame()
    {
        UIAudioHandler.Instance.PlayButtonSound();
        EventTriggerManager.Trigger<IExitGameEvent>(new ExitGameEvent(gameObject));
    }
}