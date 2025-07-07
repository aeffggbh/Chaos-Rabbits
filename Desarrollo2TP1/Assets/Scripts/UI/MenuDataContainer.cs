using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data of a menu. Such as the buttons and the text
/// </summary>
public class MenuDataContainer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _textsGO;
    [SerializeField] private List<GameObject> buttonsGO;
    [SerializeField] public GameObject SelectedButton => buttonsGO[0];
    [SerializeField] public GameObject Title => _textsGO[0];

    private void OnEnable()
    {
        EventTriggerManager.Trigger<IMenuEnableEvent>(new MenuEnableEvent(gameObject));
    }
}
