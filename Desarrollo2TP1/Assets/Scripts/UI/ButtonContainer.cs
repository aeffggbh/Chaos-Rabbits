using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Contains the buttons of a menu
/// </summary>
public class ButtonContainer : MonoBehaviour
{
	[SerializeField] private List<Button> buttons;
    private Button button;

    private void Awake()
    {
        if (buttons != null && buttons.Count > 0)
            button = buttons[0];
    }
}