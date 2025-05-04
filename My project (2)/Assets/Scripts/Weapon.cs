using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private Transform _tip;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private Transform _world;
    [SerializeField] private Transform _FPCamera;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private EnemyManager _enemyManager;
    private Vector3 _defaultPos;
    private Ray _ray;

    private void OnEnable()
    {
        ResetPos();

        transform.SetParent(_FPCamera);

        if (!_prefabBullet)
            Debug.LogError(nameof(_prefabBullet) + " is null");

        if (!_tip)
            Debug.LogError(nameof(_tip) + " is null");

        if (!_world)
            Debug.LogError(nameof(_world) + " is null");

        if (!_FPCamera)
            Debug.LogError(nameof(_FPCamera) + " is null");

        if (!_enemyManager)
            Debug.LogError(nameof(_enemyManager) + " is null");

        if (_shootAction)
            _shootAction.action.started += OnShoot;
        else
            Debug.LogError(nameof(_shootAction) + " is null");
    }

    void ResetPos()
    {
        _defaultPos = _FPCamera.transform.position;
        _defaultPos += _FPCamera.right;
        _defaultPos += -_FPCamera.up * 1.2f;
        _defaultPos += _FPCamera.forward * 2;
        transform.position = _defaultPos;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        FireInstance();
    }

    public void FireInstance()
    {
        if (!_usesHitscan)
        {
            var newBullet = Instantiate(_prefabBullet,
                                        _tip.position,
                                        _tip.rotation);
            newBullet.Fire(_FPCamera);
        }
        else
            HitscanShot();

    }

    public void Hold()
    {
        ResetPos();
        transform.eulerAngles = _FPCamera.eulerAngles;

        transform.SetParent(_FPCamera);
        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());
    }

    public void HitscanShot()
    {
        if (_enemyManager)
            for (int i = 0; i < _enemyManager._enemies.Count; i++)
                if (_enemyManager._enemies[i].GetComponent<MeshRenderer>().isVisible && _enemyManager._enemies[i].isActiveAndEnabled)
                {
                    if (PointingToEnemy(_enemyManager._enemies[i]))
                    {
                        _enemyManager._enemies[i].Die();
                        if (_enemyManager._enemies[i])
                            _enemyManager._enemies.RemoveAt(i);
                        break;
                    }
                }
    }

    public bool PointingToEnemy(Enemy enemy)
    {
        _ray.direction = _FPCamera.forward;
        _ray.origin = _FPCamera.position;

        //d = √ [(x2 – x1)2 + (y2 – y1)2 + (z2 – z1)2].
        Vector3 start = _FPCamera.transform.position;
        Vector3 end = enemy.transform.position;
        float diffX = end.x - start.x;
        float diffY = end.y - start.y;
        float diffZ = end.z - start.z;
        float distance = (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);

        Vector3 pointInView = _ray.origin + (_ray.direction * distance);

        //raySize = distance;

        BoxCollider boxCollider = enemy.GetComponent<BoxCollider>();

        if (boxCollider == null) return false;

        Vector3 max = boxCollider.bounds.max;
        Vector3 min = boxCollider.bounds.min;

        return (pointInView.x >= min.x && pointInView.x <= max.x &&
                pointInView.y >= min.y && pointInView.y <= max.y &&
                pointInView.z >= min.z && pointInView.z <= max.z);
    }

    //private void OnDrawGizmos()
    //{
    //    if (shotOnce)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * raySize);
    //    }
    //}

    public void Drop()
    {
        transform.SetParent(_world);
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
    }
}
