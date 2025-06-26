using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents a weapon that can be held by a character.
/// </summary>
[Serializable]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(WeaponAnimationController))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private Transform _weaponParent;
    [Header("User")]
    [SerializeField] public Character user;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private bool _debugUser;
    [SerializeField] private int _weaponLayerIndex;
    [SerializeField] private GameObject _centerSpawn;
    [SerializeField] private GameObject _tip;
    [SerializeField] private TrailRenderer _hitscanTrail;
    [SerializeField] private SoundManager _soundManager;
    private WeaponAnimationController _weaponAnimation;
    private AudioSource _audioSource;

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
        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;

        _audioSource = GetComponent<AudioSource>();

        if (!_tip)
            Debug.LogError(nameof(_tip) + " is null.");

        if (!_shootAction)
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
                Hold();
            else if (user.GetType() == typeof(Enemy))
                if (_usesHitscan)
                    Debug.LogError("Enemies cannot use hitscan. Deactivate the hitscan option!");
        }

        _weaponAnimation = GetComponent<WeaponAnimationController>();

        if (!_weaponAnimation)
            Debug.LogError("WeaponAnimationController is not assigned to " + name);

        if (!_centerSpawn)
            if (user.GetType() == typeof(Player))
                Debug.LogError(nameof(_centerSpawn) + " is null.");
    }

    private void Update()
    {
        CheckExistence();

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
        if (!user && (int)SceneController.currentScene != (int)SceneController.GameState.LEVEL1)
            Destroy(gameObject);
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

        if (_soundManager)
            _soundManager.PlaySound(SFXType.SHOOT, _audioSource);
        else
            Debug.LogError("SoundManager does not exist");

        if (!_usesHitscan)
        {
            GameObject spawn = _centerSpawn ? _centerSpawn : _tip;

            var newBullet = Instantiate(_prefabBullet,
                                        spawn.transform.position,
                                        spawn.transform.rotation);
            if (newBullet)
                newBullet.Fire(spawn.transform, user, this);
            else
                Debug.LogError("Bullet prefab is null or not set correctly.");
        }
        else if (user.GetType() != typeof(Enemy))
        {
            HitscanShot();
        }
        else
            Debug.LogError("Enemy cannot use hitscan. Deactivate the hitscan option!");

        _weaponAnimation.Shoot();
    }

    /// <summary>
    /// Holds the weapon, making it a child of the weapon parent and resetting its position and rotation.
    /// </summary>
    public void Hold()
    {
        if (GameManager.paused)
            return;

        Animate();

        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider)
            collider.enabled = false;

        DontDestroyOnLoad(this);

        transform.eulerAngles = _weaponParent.eulerAngles;

        transform.SetParent(_weaponParent);
        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());

        gameObject.layer = _weaponLayerIndex;
    }

    /// <summary>
    /// Drops the weapon, removing it from the weapon parent and allowing it to fall with physics.
    /// </summary>
    public void Drop()
    {
        if (GameManager.paused)
            return;

        Deanimate();

        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider)
            collider.enabled = true;

        transform.SetParent(null);
        user = null;
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();

        if (_tip)
            _centerSpawn = _tip;
        else
            Debug.LogError(nameof(_tip) + " is null");

        gameObject.layer = 0;

    }

    /// <summary>
    /// Performs a hitscan shot, checking if the player is pointing to an enemy and applying damage if so.
    /// </summary>
    public void HitscanShot()
    {
        if (GameManager.paused)
            return;

        RaycastHit? hit = null;

        float hitDistance = 100f;

        if (RayManager.PointingToObject(_centerSpawn.transform, hitDistance, out RaycastHit hitInfo))
        {
            hit = hitInfo;

            TrailRenderer trail = Instantiate(_hitscanTrail, _tip.transform.position, Quaternion.identity);

            Rigidbody rb = trail.gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;

            float force = hitDistance * 2f;

            rb.AddForce(_centerSpawn.transform.forward * force, ForceMode.Impulse);

            Destroy(trail.gameObject, hitDistance / 500f);
        }

        Debug.Log(hit.Value.collider.gameObject.name + " hit by " + name + " at distance: " + hitDistance);

        if (hit != null)
        {
            Enemy enemy = hit.Value.collider.gameObject.GetComponent<Enemy>();

            if (enemy)
                enemy.TakeDamage(user.damage);
        }


    }

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
    }

    //TODO: fix the whole bullet spawn stuff
    public void SetBulletSpawn(GameObject spawn)
    {
        Debug.Log(spawn.name + " is now the bullet spawn for " + name);

        _centerSpawn = spawn;
    }

    public void Animate()
    {
        if (!_weaponAnimation)
            _weaponAnimation = GetComponent<WeaponAnimationController>();
        else
            _weaponAnimation.ActivateAnimation();
    }

    public void Deanimate()
    {
        if (!_weaponAnimation)
            _weaponAnimation = GetComponent<WeaponAnimationController>();
        else
            _weaponAnimation.DeactivateAnimation();
    }
}
