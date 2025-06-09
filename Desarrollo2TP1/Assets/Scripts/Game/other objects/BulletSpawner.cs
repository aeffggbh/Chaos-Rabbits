using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
