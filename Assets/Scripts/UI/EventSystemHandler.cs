using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// Handles the event system provided
/// </summary>
public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private GameObject SelectedButton => _eventSystem.currentSelectedGameObject;
    private GameObject _lastSelected;
    private GameObject _lastHovered;

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
        HoverCheck();

        if (_eventSystem.currentSelectedGameObject == null)
            _eventSystem.SetSelectedGameObject(_lastSelected);
        if (_eventSystem.currentSelectedGameObject == null)
            _eventSystem.SetSelectedGameObject(_lastHovered);
    }

    private void HoverCheck()
    {
        var hovered = GetHovered();
        if (hovered && hovered != _lastHovered)
        {
            var selectable = hovered.GetComponent<Selectable>();
            if (selectable)
                _lastHovered = hovered;
        }
    }

    private GameObject GetHovered()
    {
        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue(),
        };

        var res = new List<RaycastResult>();

        EventSystem.current?.RaycastAll(pointerEventData, res);

        return res.Count > 0 ? res[0].gameObject : null;
    }

    /// <summary>
    /// When a menu enables, a different button has to be selected (since it means the menu has changed)
    /// </summary>
    /// <param name="menuEnableEvent"></param>
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


