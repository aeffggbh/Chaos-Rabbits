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
        //defaultPos.x += 1;
        //defaultPos.y -= 1;
        //defaultPos.z += 2;

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
            newBullet.Fire();
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
            for (int i = 0; i < _enemyManager.enemies.Count; i++)
                if (_enemyManager.enemies[i].GetComponent<MeshRenderer>().isVisible)
                {
                    Debug.Log("Enemy is on screen uwu");
                    if (PointingToEnemy(_enemyManager.enemies[i]))
                    {
                        //it damages the enemy. Or it dies, I guess.
                        //insta death be like
                        _enemyManager.enemies.RemoveAt(i);
                        break;
                    }
                }
    }

    public bool PointingToEnemy(Enemy enemy)
    {
        RaycastHit hit;
        if (Physics.Raycast(_FPCamera.position, _FPCamera.forward, out hit, 100f))
        {
            return hit.transform == enemy.transform || hit.transform.IsChildOf(enemy.transform);
        }
        return false;

        //_ray.direction = _FPCamera.forward;
        //_ray.origin = _FPCamera.position;

        ////d = √ [(x2 – x1)2 + (y2 – y1)2 + (z2 – z1)2].
        //Vector3 start = transform.position;
        //Vector3 end = enemy.transform.position;
        //float diffX = end.x - start.x;
        //float diffY = end.y - start.y;
        //float diffZ = end.z - start.z;
        //float distance = (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);

        //Vector3 pointInView = _ray.origin + (_ray.direction * distance);

        //BoxCollider boxCollider = enemy.GetComponent<BoxCollider>();

        //if (boxCollider == null) return false;

        //Vector3 max = boxCollider.bounds.max;
        //Vector3 min = boxCollider.bounds.min;

        ////max.x += weaponBoxIncrease;
        ////max.y += weaponBoxIncrease;
        ////max.z += weaponBoxIncrease;
        ////min.x -= weaponBoxIncrease;
        ////min.y -= weaponBoxIncrease;
        ////min.z -= weaponBoxIncrease;

        ////Debug.Log("Min: " + min);
        ////Debug.Log("Max: " + max);

        //return (pointInView.x >= min.x && pointInView.x <= max.x &&
        //        pointInView.y >= min.y && pointInView.y <= max.y &&
        //        pointInView.z >= min.z && pointInView.z <= max.z);
        //        //&&
        //        //!_holdingWeapon
        //        //&&
        //        //distance <= maxWeaponDistance;
    }

    public void Drop()
    {
        transform.SetParent(_world);
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
    }
}
