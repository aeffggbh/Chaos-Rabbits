using UnityEngine;

/// <summary>
/// Represents the player character in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(PlayerAnimationController))]
public class Player : Character, IPlayerData
{
    private IPlayerMovement _movement;
    private IPlayerJump _jump;
    private IPlayerWeaponHandler _weaponHandler;
    private ISoundPlayer _soundPlayer;

    /// <summary>
    /// saves the data of the player movement
    /// </summary>
    public IPlayerMovement Movement { get { return _movement; } }

    private Rigidbody _rb;
    [Header("Character")]
    [SerializeField] private float _damage;
    [Header("Movement")]
    private float _currentSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _counterMovementForce;
    private Vector2 currentDirection;
    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [Header("Weapon")]
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Transform _weaponParent;
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    private IPlayerMovementCalculator _playerCalculator;
    private WeaponData _savedWeaponData;
    private PlayerAnimationController _playerAnimationController;

    public Weapon CurrentWeapon { get => _currentWeapon; set { _currentWeapon = value; } }
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public float CounterMovementForce { get => _counterMovementForce; set => _counterMovementForce = value; }
    public float RunSpeed { get => _runSpeed; set => _runSpeed = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }
    public Transform WeaponParent { get => _weaponParent; set => _weaponParent = value; }
    public GameObject UserObject => gameObject;
    public WeaponData SavedWeaponData => _savedWeaponData;
    public PlayerAnimationController PlayerAnimation => _playerAnimationController;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        Damage = _damage;

        _savedWeaponData = null;

        if (!PlayerPreservedData.IsEmpty())
        {
            IPlayerSaveData data = PlayerPreservedData.RetrieveData<IPlayerSaveData>();

            if (data != null)
            {
                _currentSpeed = data.CurrentSpeed;
                CurrentHealth = data.CurrentHealth;
                _savedWeaponData = data.WeaponData;
            }
        }

        _playerAnimationController = GetComponent<PlayerAnimationController>();
    }
    private void OnDestroy()
    {
        PlayerPreservedData.SaveData<IPlayerSaveData>(new PlayerSaveData(CurrentHealth, CurrentWeapon?.WeaponData, CurrentSpeed));
    }

    protected override void Start()
    {
        base.Start();

        _movement = new PlayerMovement(this);
        _soundPlayer = new SoundPlayer(_audioSource);
        _jump = new PlayerJump(_rb, _soundPlayer, GetComponent<CapsuleCollider>(), _playerAnimationController);
        _playerCalculator = new PlayerMovementCalculator();
        _soundPlayer.SetAudioSource(GetComponent<AudioSource>());
    }


    /// <summary>
    /// Requests the movement direction and saves it
    /// </summary>
    /// <param name="direction"></param>
    public void RequestMovementDirection(Vector2 direction)
    {
        currentDirection = direction;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 calculatedMovement = _playerCalculator.GetDirection(currentDirection);
        _movement.Move(calculatedMovement);

        _jump.UpdateGroundState();
    }

    public override void Die()
    {
        if (!PlayerMediator.PlayerInstance.CheatsController.IsGodMode())
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameOverState(), gameObject));
    }

    public override void TakeDamage(float damage)
    {
        if (!PlayerMediator.PlayerInstance.CheatsController.IsGodMode())
        {
            base.TakeDamage(damage);
            _soundPlayer.PlaySound(SFXType.TAKE_HIT);
        }
        else
            Debug.Log("Cannot take damage!");
    }

    /// <summary>
    /// Returns the jump force
    /// </summary>
    /// <returns></returns>
    public float GetJumpForce()
    {
        return _jumpForce;
    }

    public void Jump()
    {
        _jump.Jump(_jumpForce);
    }

}
