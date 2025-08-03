using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data of a menu. Such as the buttons and the text
/// </summary>
public class MenuDataContainer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _textsGO;
    [SerializeField] private List<GameObject> buttonsGO;
    /// <summary>
    /// Saves the button that should be the selected one
    /// </summary>
    [SerializeField] public GameObject SelectedButton => buttonsGO[0];
    /// <summary>
    /// Saves the title of the menu
    /// </summary>
    [SerializeField] public GameObject Title => _textsGO[0];

    private void OnEnable()
    {
        EventTriggerer.Trigger<IMenuEnableEvent>(new MenuEnableEvent(gameObject));
    }
}
