
using Unity.Cinemachine;

public class PlayerSceneHandler : IPlayerSceneHandler
{
    private CinemachineBrain _cineMachineBrain;
    private PlayerMediator _controller;

    public PlayerSceneHandler()
    {
        _cineMachineBrain = CineMachineManager.Instance.cineMachineBrain;

        if (ServiceProvider.TryGetService<PlayerMediator>(out var controller))
            _controller = controller;
    }

    public void CheckPlayerDestroy()
    {
        SceneController.CheckCurrentScene();
        if (!SceneController.IsGameplay(SceneController.currentScene))
            _controller?.Destroy();
    }
}