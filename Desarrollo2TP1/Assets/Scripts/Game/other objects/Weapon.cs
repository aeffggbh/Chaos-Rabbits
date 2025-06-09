using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents a weapon that can be held by a character.
/// </summary>
[Serializable]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private EnemyManager _enemyManager;
    [Header("User")]
    [SerializeField] public Character user;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private bool _debugUser;
    [SerializeField] private int weaponLayerIndex;
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private TrailRenderer _hitscanTrail;
    [SerializeField] private SoundManager _soundManager;
    private AudioSource _audioSource;
    private Vector3 _defaultPos;
    private Type _opponentType;

    private void OnDestroy()
    {
        if (_shootAction)
            _shootAction.action.started -= OnShoot;
    }

    private void OnEnable()
    {
        if (!_prefabBullet && !_usesHitscan)
            Debug.LogError(nameof(_prefabBullet) + " is null");

        if (!_weaponParent)
        {
            Debug.LogWarning(nameof(_weaponParent) + " is null");
            Drop();
        }
    }

    private void Start()
    {
        if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
            _enemyManager = enemyManager;

        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;

        _audioSource = GetComponent<AudioSource>();

        _bulletSpawner = BulletSpawner.instance;

        if (!_shootAction)
            //es un warning porque no nos va a importar si es un enemigo
            Debug.LogWarning(nameof(_shootAction) + " is null");
        else
            _shootAction.action.started += OnShoot;

        if (user == null)
        {
            Debug.LogWarning(nameof(user) + " is null");
            Drop();
        }
        else
        {
            if (!user.IsWeaponUser)
                Debug.LogError(nameof(user) + " is not a weapon user");

            if (user.GetType() == typeof(Player))
            {
                if (!_enemyManager)
                    Debug.LogError(nameof(_enemyManager) + " is null");

                Hold();
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

        //if (!_enemyManager)
        //    if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
        //        _enemyManager = enemyManager;

        if (_debugUser)
        {
            if (user)
                Debug.Log("(" + name + ")" + "user: " + user.name);
            else
                Debug.LogWarning("No user (" + name + ") assigned.");
        }
    }

    /// <summary>
    /// Destroys weapons left at level 1 when player gets to the next level
    /// </summary>
    private void CheckExistence()
    {
        SceneController.CheckCurrentScene();
        if (!user && (int)SceneController.currentScene != (int)SceneController.GameStates.LEVEL1)
            Destroy(gameObject);
    }

    /// <summary>
    /// Sets the opponent type based on the user type.
    /// </summary>
    private void SetOpponent()
    {
        if (user.GetType() == typeof(Enemy))
            _opponentType = typeof(Player);
        else
            _opponentType = typeof(Enemy);
    }

    /// <summary>
    /// Resets the position of the weapon to a default position relative to the weapon parent.
    /// </summary>
    private void ResetPos()
    {
        _defaultPos = _weaponParent.transform.position;
        _defaultPos += _weaponParent.right;
        _defaultPos += -_weaponParent.up * 1.2f;
        _defaultPos += _weaponParent.forward * 2;
        transform.position = _defaultPos;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Handles the shoot action when the input action is triggered.
    /// </summary>
    /// <param name="context"></param>
    private void OnShoot(InputAction.CallbackContext context)
    {
        if (_debugUser)
            Debug.Log(user);
        if (user)
            Fire();
    }

    /// <summary>
    /// Fires the weapon, creating a bullet or performing a hitscan shot based on the weapon's configuration.
    /// </summary>
    public void Fire()
    {
        if (GameManager.paused)
            return;

        _soundManager.PlaySound(SoundType.SHOOT, )

        Debug.Log("instance");
        if (!_usesHitscan)
        {
            Debug.Log("bullet");
            var newBullet = Instantiate(_prefabBullet,
                                        _bulletSpawner.transform.position,
                                        _bulletSpawner.transform.rotation);
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

    /// <summary>
    /// Holds the weapon, making it a child of the weapon parent and resetting its position and rotation.
    /// </summary>
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

        gameObject.layer = weaponLayerIndex;
    }

    /// <summary>
    /// Drops the weapon, removing it from the weapon parent and allowing it to fall with physics.
    /// </summary>
    public void Drop()
    {
        if (GameManager.paused)
            return;

        transform.SetParent(null);
        user = null;
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();

        gameObject.layer = 0;
    }

    /// <summary>
    /// Performs a hitscan shot, checking if the player is pointing to an enemy and applying damage if so.
    /// </summary>
    public void HitscanShot()
    {
        if (GameManager.paused)
            return;

        RayManager hitDetector = new();
        RaycastHit? hit = null; // Use a nullable RaycastHit

        float hitDistance = 100f;

        if (hitDetector.PointingToObject(_weaponParent, hitDistance, out RaycastHit hitInfo))
        {
            hit = hitInfo;

            TrailRenderer trail = Instantiate(_hitscanTrail, _weaponParent.position, Quaternion.identity);

            Rigidbody rb = trail.gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;

            rb.AddForce(_weaponParent.transform.forward * 1000f, ForceMode.Impulse);
        }

        if (_enemyManager)
            for (int i = 0; i < _enemyManager.enemies.Count; i++)
            {
                if (_enemyManager.enemies[i].GetComponent<MeshRenderer>() != null)
                {
                    if (_enemyManager.enemies[i].GetComponent<MeshRenderer>().isVisible)
                        if (hit != null)
                            if (hit.Value.collider == _enemyManager.enemies[i]._collider)
                            {
                                _enemyManager.enemies[i].TakeDamage(user.damage);

                                break;
                            }
                }
                else
                {
                    Debug.LogError("Enemy " + _enemyManager.enemies[i].name + " has no mesh renderer");
                }
            }
    }

    //private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    //{
    //    float time = 0f;

    //    Vector3 startPos = trail.transform.position;

    //    while (time < 1)
    //    {
    //        trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
    //        time += Time.deltaTime / trail.time;
    //        yield return null;
    //    }

    //    trail.transform.position = hit.point;
    //    Destroy(trail.gameObject, trail.time);
    //}

    /// <summary>
    /// Sets the user of the weapon based on the character type provided.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="characterType"></param>
    public void SetUser(Character character, Type characterType)
    {
        if (characterType == typeof(Player))
            user = character as Player;
        else if (characterType == typeof(Enemy))
            user = character as Enemy;

        SetOpponent();
    }
}
