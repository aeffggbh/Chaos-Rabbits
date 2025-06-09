using UnityEngine;
using UnityEngine.UI;
using static SceneController;

[RequireComponent(typeof(Button))]
public class SceneLoaderButtonHandler : MonoBehaviour
{
    [SerializeField] GameStates state;
    private Button button;
    private void Reset()
    {
        button = GetComponent<Button>();
    }

    private void Awake()
    {
        button ??= GetComponent<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        SceneController.GoToScene(state);
    }
}
