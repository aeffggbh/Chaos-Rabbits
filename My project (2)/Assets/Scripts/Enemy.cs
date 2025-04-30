using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyManager manager;
    public GameObject originalObject;
    public bool _onScreen;

    private void Awake()
    {
        if (manager != null)
            manager.enemies.Add(this);
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        originalObject = gameObject;
    }

    private void OnBecameVisible()
    {
        Debug.Log("I EXIST");
        _onScreen = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("I DONT EXIST");
        _onScreen = false;
    }

}
