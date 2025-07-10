using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Handles the event system provided
/// </summary>
public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private GameObject _selectedButton => _eventSystem.currentSelectedGameObject;
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

    /// <summary>
    /// Ensures that there is always a button selected
    /// </summary>
    private void EnsureSelection()
    {
        if (_eventSystem.currentSelectedGameObject == null)
            _eventSystem.SetSelectedGameObject(_lastSelected);
    }

    /// <summary>
    /// When a menu enables, a different button has to be selected (since it means the menu has changed)
    /// </summary>
    /// <param name="menuEnableEvent"></param>
    private void CheckSelectedButton(IMenuEnableEvent menuEnableEvent)
    {
        MenuDataContainer menuDataContainer = menuEnableEvent.TriggeredByGO.GetComponent<MenuDataContainer>();

        if (menuDataContainer?.SelectedButton != _selectedButton)
        {
            _eventSystem.SetSelectedGameObject(menuDataContainer.SelectedButton);
            _lastSelected = _eventSystem.currentSelectedGameObject;
        }
    }
}
