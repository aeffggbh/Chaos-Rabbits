using UnityEngine;

public class ActivateMenuEvent : IActivateSceneEvent
{
    private IMenuState _state;
    private GameObject _source;
    public IMenuState NewState { get => _state; set => _state = value; }
    public GameObject TriggeredByGO { get => _source; }
    public int Index => MenuSceneData.Index;

    public ActivateMenuEvent(IMenuState nextState, GameObject source)
    {
        _state = nextState;
        _source = source;

        if (MenuManager.Instance)
            MenuManager.Instance.TransitionToState(nextState);
    }

    public void GetIndex()
    {
        throw new System.NotImplementedException();
    }
}
