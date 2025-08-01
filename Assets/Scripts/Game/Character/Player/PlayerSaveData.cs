public class PlayerSaveData : IPlayerSaveData
{
    private float _currentHealth;
    private float _maxHealth;
    private float _currentSpeed;
    private WeaponData _weaponData;

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public WeaponData WeaponData { get => _weaponData; set => _weaponData = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public PlayerSaveData(float currentHealth, WeaponData weaponData, float currentSpeed)
    {
        _currentHealth = currentHealth;
        _weaponData = weaponData;
        _currentSpeed = currentSpeed;
        _maxHealth = 100f;
    }
}
