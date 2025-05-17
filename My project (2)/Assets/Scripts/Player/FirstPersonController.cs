using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    //[SerializeField] private float sensitivity;
    //[SerializeField] private InputActionReference lookAtAction;
    //[SerializeField] private float minPitch;
    //[SerializeField] private float maxPitch;
    //Vector2 mouseDelta;

    //float pitch;
    //float yaw;

    //private void Awake()
    //{
    //    //mouse va a volver al centro cuando se deje de mover
    //    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    //    //el mouse no se va a ver
    //    Cursor.visible = false;
    //}

//TESTS
//    private void FixedUpdate()
//    {
//        mouseDelta = GetMouseDelta().normalized;


//        pitch = mouseDelta.y * sensitivity;
//        yaw = mouseDelta.x * sensitivity;

//        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

//        transform.eulerAngles += new Vector3
//            (-pitch * Time.deltaTime,
//            yaw * Time.deltaTime,
//            0);

//    }

//    private Vector2 GetMouseDelta()
//    {
//        if (lookAtAction.action == null)
//            return Vector2.zero;

//        return lookAtAction.action.ReadValue<Vector2>();
//    }
}
