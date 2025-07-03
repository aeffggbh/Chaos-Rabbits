using UnityEngine;
using UnityEngine.UI;
using static SceneController;

[RequireComponent(typeof(Button))]
public class SceneLoaderButtonHandler : MonoBehaviour
{
    [SerializeField] GameState state;
    private Button _button;

    private void Reset()
    {
        _button = GetComponent<Button>();
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        UIAudioHandler.Instance.PlaySound();

        SceneController.GoToScene(state);
    }

}
