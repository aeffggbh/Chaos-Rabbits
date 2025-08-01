public interface IPlayerSaveData : IData
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }
    float CurrentSpeed { get; set; }
    public WeaponData WeaponData { get; set; }
}
