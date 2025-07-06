using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages overal gameplay variables and replayability
/// </summary>
public static class GameManager
{
    //    public static bool initialized = false;
    //    public static Weapon defaultWeapon;
    //    public static bool paused = false;
    //    public static SoundManager soundManager;
    //public static PlayerMediator savedPlayer;

    /// <summary>
    /// Resets the game
    /// </summary>
    public static void ResetGame()
    {
        //if (initialized)
        //{
        if (CheatsController.Instance)
            CheatsController.Instance.Reset();

        if (PlayerMediator.PlayerInstance)
            PlayerMediator.PlayerInstance.Destroy();

        //if (savedPlayer != null)
        //{
        //    savedPlayer.Destroy();
        //    savedPlayer = null;
        //}
        //}
    }
}
