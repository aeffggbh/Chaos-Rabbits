//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
//public class SceneChangeButtonHandler : MonoBehaviour
//{
//    [SerializeField] IScene.Index state = IScene.Index.LEVEL1;
//    private Button _button;

//    private void Reset()
//    {
//        _button = GetComponent<Button>();
//    }

//    private void Awake()
//    {
//        _button = GetComponent<Button>();
//    }

//    private void OnEnable()
//    {
//        _button.onClick.AddListener(HandleClick);
//    }

//    private void OnDisable()
//    {
//        _button.onClick.RemoveListener(HandleClick);
//    }

//    private void HandleClick()
//    {
//        UIAudioHandler.Instance.PlayButtonSound();
//        //SceneLoader.Instance.LoadScene(state);
//        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateSceneEvent(state, gameObject));
//    }
//}
