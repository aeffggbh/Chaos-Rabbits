using System;
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
    [Header("User")]
    [SerializeField] public Character user;
    [Header("Hitscan")]
    [SerializeField] private bool _usesHitscan;
    [SerializeField] private TrailRenderer _hitscanTrail;
    [Header("Setup")]
    [SerializeField] private int _weaponLayerIndex;
    [SerializeField] private GameObject _centerSpawnGO;
    [SerializeField] private GameObject _tipGO;
    [SerializeField] private Vector3 pickupScale = new(0.2f, 0.2f, 0.2f);
    [SerializeField] private Vector3 dropScale;
    [SerializeField] WeaponData _weaponData;
    private AudioSource _audioSource;
    private ISoundPlayer _soundPlayer;

    public WeaponData WeaponData { get => _weaponData; set => _weaponData = value; }
    public GameObject BulletSpawnGO { get => _centerSpawnGO; set => _centerSpawnGO = value; }

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

    private void Awake()
    {
        dropScale = transform.localScale;
    }

    private void Start()
    {
        EventProvider.Subscribe<IActivateSceneEvent>(CheckExistence);

        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);

        if (!_tipGO)
            Debug.LogError(nameof(_tipGO) + " is null. for " + this.gameObject);

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
            if (user is not IWeaponUser)
                Debug.LogError(nameof(user) + " is not a weapon user");

            else if (user.GetType() == typeof(Enemy))
                if (_usesHitscan)
                    Debug.LogError("Enemies cannot use hitscan. Deactivate the hitscan option!");
        }
    }

    /// <summary>
    /// Destroys weapons left at level 1 when player gets to the next level
    /// </summary>
    private void CheckExistence(IActivateSceneEvent sceneEvent)
    {
        int index = sceneEvent.Index;

        ServiceProvider.TryGetService<GameSceneController>(out var controller);
        bool IsGameplay = controller.IsGameplay(index);

        if (!user && index != GameplaySceneData.Level1Index && IsGameplay && this != null)
            Destroy(gameObject);
    }

    /// <summary>
    /// Handles the shoot action when the input action is triggered.
    /// </summary>
    /// <param name="context"></param>
    private void OnShoot(InputAction.CallbackContext context)
    {
        if (user?.GetType() == typeof(Player))
        {
            ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
            mediator.Player.PlayerAnimation.AnimateShoot();
            Fire();
        }
    }

    /// <summary>
    /// Fires the weapon, creating a bullet or performing a hitscan shot based on the weapon's configuration.
    /// </summary>
    public void Fire()
    {
        if (PauseManager.Paused)
            return;

        _soundPlayer?.PlaySound(SFXType.SHOOT);

        if (!_usesHitscan)
        {
            GameObject spawn = BulletSpawnGO ? BulletSpawnGO : _tipGO;

            var newBullet = Instantiate(_prefabBullet,
                                        spawn.transform.position,
                                        spawn.transform.rotation);
            if (newBullet)
                newBullet.Fire(spawn.transform, user as IWeaponUser);
            else
                Debug.LogError("Bullet prefab is null or not set correctly.");
        }
        else if (user.GetType() != typeof(Enemy))
        {
            HitscanShot();
        }
        else
            Debug.LogError("Enemy cannot use hitscan. Deactivate the hitscan option!");

    }

    /// <summary>
    /// Holds the weapon, making it a child of the weapon parent and resetting its position and rotation.
    /// </summary>
    public void Hold(Transform _weaponParent)
    {
        if (PauseManager.Paused)
            return;

        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider)
            collider.enabled = false;

        transform.SetParent(_weaponParent);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = pickupScale;

        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());

        gameObject.layer = _weaponLayerIndex;
    }

    /// <summary>
    /// Drops the weapon, removing it from the weapon parent and allowing it to fall with physics.
    /// </summary>
    public void Drop()
    {
        if (PauseManager.Paused)
            return;

        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider)
            collider.enabled = true;

        transform.SetParent(null);

        transform.localScale = dropScale;

        user = null;
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();

        if (_tipGO)
            BulletSpawnGO = _tipGO;
        else
            Debug.LogError(nameof(_tipGO) + " is null");

        gameObject.layer = 0;


    }

    /// <summary>
    /// Performs a hitscan shot, checking if the player is pointing to an enemy and applying damage if so.
    /// </summary>
    public void HitscanShot()
    {
        if (PauseManager.Paused)
            return;

        RaycastHit? hit = null;

        float hitDistance = 100f;

        if (RayManager.PointingToObject(BulletSpawnGO.transform, hitDistance, out RaycastHit hitInfo))
            hit = hitInfo;

        TrailRenderer trail = Instantiate(_hitscanTrail, _tipGO.transform.position, Quaternion.identity);

        Rigidbody rb = trail.gameObject.AddComponent<Rigidbody>();
        rb.freezeRotation = true;

        float force = hitDistance * 2f;

        rb.AddForce(BulletSpawnGO.transform.forward * force, ForceMode.Impulse);

        Destroy(trail.gameObject, hitDistance / 500f);

        if (hit.HasValue)
        {
            if (hit != null)
            {
                Enemy enemy = hit.Value.collider.gameObject.GetComponent<Enemy>();

                if (enemy)
                    enemy.TakeDamage(user.Damage);
            }
        }
    }

}
