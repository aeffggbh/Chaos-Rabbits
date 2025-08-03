using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuAudioRouter : MonoBehaviour
{
    private LocalAudioHandler _audioHandler;
    private GameObject _lastSelected;
    private GameObject _lastHovered;

    private GameObject _currentSelected => EventSystem.current?.currentSelectedGameObject;

    private void Start()
    {
        ServiceProvider.TryGetService<LocalAudioHandler>(out _audioHandler);

        EventProvider.Subscribe<IButtonClickEvent>(OnConfirm);

        if (_currentSelected)
            _lastSelected = _currentSelected;
    }

    private void Update()
    {
        var hovered = GetHovered();
        if (hovered && hovered != _lastHovered)
        {
            var selectable = hovered.GetComponent<Selectable>();
            if (selectable)
            {
                _audioHandler?.PlayNavigationSound();
                _lastHovered = hovered;
            }
        }
    }

    private void LateUpdate()
    {
        if (_currentSelected)
            if (_currentSelected != _lastSelected)
            {
                _audioHandler?.PlayNavigationSound();
                _lastSelected = _currentSelected;
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

    private void OnConfirm(IButtonClickEvent @event)
    {
        if (!IsInMenu())
            return;
        if (_currentSelected)
            if (_currentSelected == _lastSelected)
                _audioHandler?.PlayButtonSound();
    }

    private bool IsInMenu()
    {
        ServiceProvider.TryGetService<GameSceneController>(out var sceneController);

        if (sceneController)
            return PauseManager.Paused || !sceneController.IsTypeLoaded<GameplaySceneData>();
        return false;
    }
}