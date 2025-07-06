public static class PlayerDataProvider
{
    private static IPlayerData _defaultPlayer;

    public static IPlayerData DefaultPlayerData { get => _defaultPlayer; set => SetDefaultPlayer(value); }
    public static IPlayerData PlayerData { get; set; }

    public static void SetDefaultPlayer(IPlayerData playerData)
    {
        _defaultPlayer = new Player(playerData);
    }

    public static void SavePlayerData(IPlayerData playerData)
    {
        PlayerData = new Player(playerData);
    }

    public static IPlayerData GetSavedPlayerData()
    {
        return new Player(PlayerData);
    }

    public static IPlayerData GetSavedData()
    {
        return PlayerData;
    }
}
