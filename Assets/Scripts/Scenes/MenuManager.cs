using UnityEngine;

/// <summary>
/// Manages the gameobjects that represent the menus
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Main menu gameobject
    /// </summary>
    public GameObject MainMenuPanel { get => _mainMenuPanel; private set => _mainMenuPanel = value; }
    /// <summary>
    /// Pause menu gameobject
    /// </summary>
    public GameObject PauseMenuPanel { get => _pauseMenuPanel; private set => _pauseMenuPanel = value; }
    /// <summary>
    /// Game over gameobject
    /// </summary>
    public GameObject GameOverPanel { get => _gameOverPanel; private set => _gameOverPanel = value; }
    /// <summary>
    /// Credits menu gameobject
    /// </summary>
    public GameObject CreditsMenuPanel { get => _creditsMenuPanel; private set => _creditsMenuPanel = value; }
    /// <summary>
    /// Check exit menu gameobject
    /// </summary>
    public GameObject CheckExitMenuPanel { get => _checkExitMenuPanel; private set => _checkExitMenuPanel = value; }
    /// <summary>
    /// Game win menu gameobject
    /// </summary>
    public GameObject GameWinPanel { get => _gameWinPanel; private set => _gameWinPanel = value; }

    private IMenuState _currentState = null;
    private IMenuState _previousState = null;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _creditsMenuPanel;
    [SerializeField] private GameObject _checkExitMenuPanel;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _gameOverPanel;

    /// <summary>
    /// Returns the previous menu state
    /// </summary>
    public IMenuState PreviousState { get { return _previousState; } set { _previousState = value; } }

    private void Awake()
    {
        ServiceProvider.SetService<MenuManager>(this);
    }

    private void Start()
    {
        HideAllPanels();

        if (_currentState == null)
            TransitionToState(new MainMenuState());
    }

    /// <summary>
    /// Transitions to a provided state
    /// </summary>
    /// <param name="menuState"></param>
    public void TransitionToState(IMenuState menuState)
    {
        if (_currentState == menuState)
            return;

        if (_currentState != null && _currentState is IExitStateCommand)
            (_currentState as IExitStateCommand)?.ExitState();

        _previousState = _currentState;
        _currentState = menuState;
        HideAllPanels();
        _currentState.EnterState(this);
    }

    /// <summary>
    /// Disables all the menu gameobjects
    /// </summary>
    public void HideAllPanels()
    {
        MainMenuPanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        CreditsMenuPanel.SetActive(false);
        CheckExitMenuPanel.SetActive(false);
    }

    /// <summary>
    /// Enables the given gameobject
    /// </summary>
    /// <param name="panel"></param>
    public void ShowPanel(GameObject panel)
    {
        HideAllPanels();
        panel.SetActive(true);
        EventTriggerer.Trigger<IMenuShownEvent>(new MenuShownEvent(panel, panel.transform));
    }

    /// <summary>
    /// Some menu states will call this to reset the game,
    /// involving unloading all gameplay and enabling the UI audio listener.
    /// </summary>
    public static void ResetGame()
    {
        PauseManager.Paused = false;

        ServiceProvider.TryGetService<GameSceneController>(out var controller);
        controller.UnloadGameplay();

        GameManager.ResetGame();
    }
}
