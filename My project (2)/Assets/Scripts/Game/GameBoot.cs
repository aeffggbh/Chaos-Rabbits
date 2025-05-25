using UnityEngine;
using UnityEngine.InputSystem;

// This class is responsible for initializing the game and setting up the GameManager at the start of the game
public class GameBoot : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Weapon _defaultWeapon;
    [SerializeField] private InputActionReference _pauseButton;

    void Awake()
    {
        if (!GameManager.initialized)
        {
            Debug.Log("Resetting game");
            if (!_playerController)
                Debug.LogError(nameof(_playerController) + " is null");

            if (!_defaultWeapon)
                _defaultWeapon = _playerController.currentWeapon;

            if (!_pauseButton)
                Debug.LogError(nameof(_pauseButton) + " is null");

            GameManager.defaultWeapon = _defaultWeapon;
            GameManager.savedPlayer = _playerController;
            GameManager.pauseButton = _pauseButton;


            GameManager.initialized = true;
        }
        else
            Debug.Log("Not resetting game");
    }

    private void Start()
    {
        if (GameManager.cheatsController == null)
            GameManager.cheatsController = CheatsController.instance;
    }
}
