using System.Collections;
using UnityEngine;

//las publicas van sin guion bajo
public class LookAtTarget : MonoBehaviour
{
    public float speed = 2f;

    private Coroutine _lookCoroutine;

    public void Look(Vector3 target)
    {
        Quaternion initialRotation = transform.rotation;
        target.y = transform.position.y; 
        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);
        float time = Time.deltaTime * speed;

        transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

        return;
        //if (_lookCoroutine != null)
        //    StopCoroutine(_lookCoroutine);

        //_lookCoroutine = StartCoroutine(LookAt(target));
    }

    private IEnumerator LookAt(Vector3 target)
    {
        int i = Random.Range(0, 1000);
        Debug.Log("LookAt: " + i);

        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);

        float time = 0;

        Quaternion initialRotation = transform.rotation;
        while (time < 1)
        {
            Debug.Log("Rotando: " + i);

            transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);

            time += Time.deltaTime * speed;

            yield return null;
        }
    }
}
