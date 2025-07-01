using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for bootstrapping the game and initializing the GameManager.
/// </summary>
// This class is responsible for initializing the game and setting up the GameManager at the start of the game
public class GameBoot : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseButton;
    private PlayerMediator _playerController;

    private void Start()
    {
        if (!GameManager.initialized)
            InitializeGameManager();
        else
            Debug.Log("Not resetting game");
    }

    /// <summary>
    /// Initializes the game by setting up the GameManager
    /// </summary>
    private void InitializeGameManager()
    {
        Debug.Log("Resetting game");

        if (ServiceProvider.TryGetService<PlayerMediator>(out var playerController))
            _playerController = playerController;

        if (!_pauseButton)
            Debug.LogError(nameof(_pauseButton) + " is null");

        GameManager.savedPlayer = _playerController;
        GameManager.pauseButton = _pauseButton;

        GameManager.initialized = true;
    }



}
