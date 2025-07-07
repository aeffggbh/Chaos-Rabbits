using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Handles the event system provided
/// </summary>
public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private GameObject SelectedButton => _eventSystem.currentSelectedGameObject;
    private GameObject _lastSelected;

    private void Awake()
    {
        if (_eventSystem == null)
            Debug.LogWarning("No event system found");

        EventProvider.Subscribe<IMenuEnableEvent>(CheckSelectedButton);

    }

    private void LateUpdate()
    {
        EnsureSelection();
    }

    private void EnsureSelection()
    {
        if (_eventSystem.currentSelectedGameObject == null)
            _eventSystem.SetSelectedGameObject(_lastSelected);
    }

    private void CheckSelectedButton(IMenuEnableEvent menuEnableEvent)
    {
        MenuDataContainer menuDataContainer = menuEnableEvent.TriggeredByGO.GetComponent<MenuDataContainer>();

        if (menuDataContainer?.SelectedButton != SelectedButton)
        {
            _eventSystem.SetSelectedGameObject(menuDataContainer.SelectedButton);
            _lastSelected = _eventSystem.currentSelectedGameObject;
        }
    }
}
