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
    //[SerializeField] private Transform _world;
    [SerializeField] private Transform _FPCamera;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private EnemyManager _enemyManager;
    public BoxCollider _collider;
    private Vector3 _defaultPos;
    private Ray _ray;

    private void OnEnable()
    {
        _collider = gameObject.GetComponent<BoxCollider>();
        ResetPos();

        transform.SetParent(_FPCamera);

        if (!_prefabBullet)
            Debug.LogError(nameof(_prefabBullet) + " is null");

        if (!_tip)
            Debug.LogError(nameof(_tip) + " is null");

        //if (!_world)
        //    Debug.LogError(nameof(_world) + " is null");

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
        transform.localScale = Vector3.one;
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
    public void Drop()
    {
        transform.SetParent(null);
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
    }

    public void HitscanShot()
    {
        if (_enemyManager)
            for (int i = 0; i < _enemyManager._enemies.Count; i++)
            {
                if (_enemyManager._enemies[i].GetComponent<MeshRenderer>() != null)
                {
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
                else
                {
                    Debug.LogError("Enemy " + _enemyManager._enemies[i].name + " has no mesh renderer");
                }
            }

    }

    public bool PointingToEnemy(Enemy enemy)
    {
        RayManager pointDetector = new();

        return pointDetector.PointingToObject(_FPCamera, enemy.transform, enemy._collider);
    }

    //private void OnDrawGizmos()
    //{
    //    if (shotOnce)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * raySize);
    //    }
    //}

}
