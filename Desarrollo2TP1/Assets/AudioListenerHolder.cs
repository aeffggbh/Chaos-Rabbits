//using System;
//using Unity.VisualScripting;
//using UnityEngine;

//[RequireComponent(typeof(AudioListener))]
//public class AudioListenerHolder : MonoBehaviour
//{
//    private Transform _parent;
//    private AudioListener _listener;

//    public static AudioListenerHolder Instance { get; private set; }

//    public void ResetParent()
//    {
//        _parent = null;
//        transform.SetParent(null);
//        transform.position = Vector3.zero;
//    }

//    private void Awake()
//    {
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//        _listener = GetComponent<AudioListener>();
//    }

//    private void OnDisable()
//    {
//        this.enabled = true;
//        _listener.enabled = true;
//    }

//    private void Update()
//    {
//        if (PlayerMediator.PlayerInstance && !_parent)
//        {
//            _parent = PlayerMediator.PlayerInstance.transform;
//            transform.SetParent(_parent);
//            transform.position = Vector3.zero;
//        }
//    }
//}
