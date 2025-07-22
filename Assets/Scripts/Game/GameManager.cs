using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages overal gameplay variables and replayability
/// </summary>
public static class GameManager
{
    /// <summary>
    /// Resets the game
    /// </summary>
    public static void ResetGame()
    {
        if (CheatsController.Instance)
            CheatsController.Instance.Reset();

        if (PlayerMediator.PlayerInstance)
            PlayerMediator.PlayerInstance.Destroy();
    }
}
