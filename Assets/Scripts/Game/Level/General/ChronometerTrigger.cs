using TMPro;
using UnityEngine;

/// <summary>
/// Triggers the chronometer
/// </summary>
public class ChronometerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventTriggerManager.Trigger<IChronometerTriggerEvent>(new ChronometerTriggerEvent(other, gameObject));
    }
}