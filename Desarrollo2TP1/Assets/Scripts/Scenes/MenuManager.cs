using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel { get => _mainMenuPanel; private set => _mainMenuPanel = value; }
    public GameObject PauseMenuPanel { get => _pauseMenuPanel; private set => _pauseMenuPanel = value; }
    public GameObject GameOverPanel { get => _gameOverPanel; private set => _gameOverPanel = value; }
    public GameObject CreditsMenuPanel { get => _creditsMenuPanel; private set => _creditsMenuPanel = value; }
    public GameObject CheckExitMenuPanel { get => _checkExitMenuPanel; private set => _checkExitMenuPanel = value; }
    public GameObject GameWinPanel { get => _gameWinPanel; private set => _gameWinPanel = value; }

    private IMenuState _currentState = null;
    private IMenuState _previousState = null;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _creditsMenuPanel;
    [SerializeField] private GameObject _checkExitMenuPanel;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _gameOverPanel;

    public IMenuState PreviousState { get { return _previousState; } set { _previousState = value; } }
    public IMenuState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        HideAllPanels();

        if (_currentState == null)
            TransitionToState(new MainMenuState());
    }

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

    public void TransitionToPrevious()
    {
        TransitionToState(PreviousState);
    }

    public void HideAllPanels()
    {
        MainMenuPanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        CreditsMenuPanel.SetActive(false);
        CheckExitMenuPanel.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        HideAllPanels();
        panel.SetActive(true);
    }

    //TODO: clean "internal" stuff. Maybe I missed something
}
