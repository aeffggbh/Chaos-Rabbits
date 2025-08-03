using TMPro;
using UnityEngine;

/// <summary>
/// Triggers the chronometer
/// </summary>
public class ChronometerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventTriggerer.Trigger<IChronometerTriggerEvent>(new ChronometerTriggerEvent(other, gameObject));
    }
}