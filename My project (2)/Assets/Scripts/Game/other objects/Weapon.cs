using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private Transform _tip;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] public EnemyManager enemyManager;
    [Header("User")]
    [SerializeField] public Character user;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private bool _debugUser;

    private Vector3 _defaultPos;
    private Type _opponentType;

    private void OnEnable()
    {
        if (!_prefabBullet && !_usesHitscan)
            Debug.LogError(nameof(_prefabBullet) + " is null");

        if (!_tip)
            Debug.LogError(nameof(_tip) + " is null");

        if (!_weaponParent)
        {
            Debug.LogWarning(nameof(_weaponParent) + " is null");
            //asume que es un arma droppeada.
            Drop();
        }
        else
            transform.SetParent(_weaponParent);
    }


    private void Start()
    {
        LookForEnemyManager();
        if (!_shootAction)
            //es un warning porque no nos va a importar si es un enemigo
            Debug.LogWarning(nameof(_shootAction) + " is null");
        else
            _shootAction.action.started += OnShoot;

        if (user == null)
        {
            Debug.LogWarning(nameof(user) + " is null");
            //asume que es un arma droppeada.
            Drop();
        }
        else
        {
            if (!user.IsWeaponUser)
                Debug.LogError(nameof(user) + " is not a weapon user");

            if (user.GetType() == typeof(Player))
            {
                if (!enemyManager)
                    Debug.LogError(nameof(enemyManager) + " is null");

                ResetPos();
            }
            else if (user.GetType() == typeof(Enemy))
                if (_usesHitscan)
                    Debug.LogError("Enemies cannot use hitscan. Deactivate the hitscan option!");

            SetOpponent();

        }

    }

    private void Update()
    {
        CheckExistence();
        LookForEnemyManager();

        if (_debugUser)
        {
            if (user)
                Debug.Log("(" + name + ")" + "user: " + user.name);
            else
                Debug.LogWarning("No user (" + name + ") assigned.");
        }
    }
    private void CheckExistence()
    {
        //in case DontDestroyOnLoad was called
        if (!user && (int)SceneController.currentScene != (int)SceneController.Scenes.LEVEL1)
            Destroy(gameObject);
    }

    private void SetOpponent()
    {
        if (user.GetType() == typeof(Enemy))
            _opponentType = typeof(Player);
        else
            _opponentType = typeof(Enemy);
    }

    private void LookForEnemyManager()
    {
        if (!enemyManager)
            enemyManager = EnemyManager.instance;

    }

    private void ResetPos()
    {
        _defaultPos = _weaponParent.transform.position;
        _defaultPos += _weaponParent.right;
        _defaultPos += -_weaponParent.up * 1.2f;
        _defaultPos += _weaponParent.forward * 2;
        transform.position = _defaultPos;
        transform.localScale = Vector3.one;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (_debugUser)
            Debug.Log(user);
        if (user)
            Fire();
    }

    public void Fire()
    {
        if (GameManager.paused)
            return;

        Debug.Log("instance");
        if (!_usesHitscan)
        {
            Debug.Log("bullet");
            var newBullet = Instantiate(_prefabBullet,
                                        _tip.position,
                                        _tip.rotation);
            newBullet.Fire(_weaponParent, _opponentType, user.damage);
        }
        else if (user.GetType() != typeof(Enemy))
        {
            Debug.Log("hitscan");

            HitscanShot();
        }
        else
            Debug.LogError("Enemy cannot use hitscan. Deactivate the hitscan option!");

    }

    public void Hold()
    {
        if (GameManager.paused)
            return;

        DontDestroyOnLoad(this);

        ResetPos();
        transform.eulerAngles = _weaponParent.eulerAngles;

        transform.SetParent(_weaponParent);
        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());
    }

    public void Drop()
    {
        if (GameManager.paused)
            return;

        transform.SetParent(null);
        user = null;
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
    }

    public void HitscanShot()
    {
        if (GameManager.paused)
            return;

        if (enemyManager)
            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
                if (enemyManager.enemies[i].GetComponent<MeshRenderer>() != null)
                {
                    if (enemyManager.enemies[i].GetComponent<MeshRenderer>().isVisible)
                        if (PointingToEnemy(enemyManager.enemies[i]))
                        {
                            enemyManager.enemies[i].TakeDamage(user.damage);
                            break;
                        }
                }
                else
                {
                    Debug.LogError("Enemy " + enemyManager.enemies[i].name + " has no mesh renderer");
                }
            }

    }

    public bool PointingToEnemy(Enemy enemy)
    {
        RayManager pointDetector = new();

        return pointDetector.PointingToObject(_weaponParent, enemy.transform, enemy._collider);
    }

    public void SetUser(Character character, Type characterType)
    {
        if (characterType == typeof(Player))
            user = character as Player;
        else if (characterType == typeof(Enemy))
            user = character as Enemy;

        SetOpponent();
    }
}
