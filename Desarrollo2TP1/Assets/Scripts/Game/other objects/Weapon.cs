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
    [Header("User")]
    [SerializeField] public Character user;
    [SerializeField] private bool _debugUser;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private TrailRenderer _hitscanTrail;
    [Header("Setup")]
    [SerializeField] private int _weaponLayerIndex;
    [SerializeField] private GameObject _centerSpawnGO;
    [SerializeField] private GameObject _tipGO;
    [SerializeField] private Vector3 pickupScale = new(0.2f, 0.2f, 0.2f);
    [SerializeField] private Vector3 dropScale;
    private WeaponAnimationController _weaponAnimation;
    private AudioSource _audioSource;
    private ISoundPlayer _soundPlayer;

    private void OnDestroy()
    {
        if (_shootAction)
            _shootAction.action.started -= OnShoot;
    }

    private void OnEnable()
    {
        if (!_prefabBullet && !_usesHitscan)
            Debug.LogError(nameof(_prefabBullet) + " is null");
    }
    private void Start()
    {
        dropScale = transform.localScale;

        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);

        if (!_tipGO)
            Debug.LogError(nameof(_tipGO) + " is null.");

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

            else if (user.GetType() == typeof(Enemy))
                if (_usesHitscan)
                    Debug.LogError("Enemies cannot use hitscan. Deactivate the hitscan option!");
        }

        _weaponAnimation = GetComponent<WeaponAnimationController>();

        if (!_weaponAnimation)
            Debug.LogError("WeaponAnimationController is not assigned to " + name);

        if (_centerSpawnGO != null && user != null)
            if (user.GetType() == typeof(Player))
                Debug.LogError(nameof(_centerSpawnGO) + " is null.");
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

        _soundPlayer?.PlaySound(SFXType.SHOOT);

        if (!_usesHitscan)
        {
            GameObject spawn = _centerSpawnGO ? _centerSpawnGO : _tipGO;

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
    public void Hold(Transform _weaponParent)
    {
        if (GameManager.paused)
            return;

        Animate();

        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider)
            collider.enabled = false;

        DontDestroyOnLoad(this);

        transform.SetParent(_weaponParent);

        transform.localScale = pickupScale;

        transform.rotation = Quaternion.identity;

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

        transform.localScale = dropScale;

        user = null;
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();

        if (_tipGO)
            _centerSpawnGO = _tipGO;
        else
            Debug.LogError(nameof(_tipGO) + " is null");

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

        if (RayManager.PointingToObject(_centerSpawnGO.transform, hitDistance, out RaycastHit hitInfo))
            hit = hitInfo;

        TrailRenderer trail = Instantiate(_hitscanTrail, _tipGO.transform.position, Quaternion.identity);

        Rigidbody rb = trail.gameObject.AddComponent<Rigidbody>();
        rb.freezeRotation = true;

        float force = hitDistance * 2f;

        rb.AddForce(_centerSpawnGO.transform.forward * force, ForceMode.Impulse);

        Destroy(trail.gameObject, hitDistance / 500f);

        if (hit.HasValue)
        {
            Debug.Log(hit.Value.collider.gameObject.name + " hit by " + name + " at distance: " + hitDistance);

            if (hit != null)
            {
                Enemy enemy = hit.Value.collider.gameObject.GetComponent<Enemy>();

                if (enemy)
                    enemy.TakeDamage(user.Damage);
            }
        }
    }

    //TODO: fix the whole bullet spawn stuff
    public void SetBulletSpawn(GameObject spawn)
    {
        Debug.Log(spawn.name + " is now the bullet spawn for " + name);

        _centerSpawnGO = spawn;
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
