public interface IEnemySpawnEvent : IEvent
{
    /// <summary>
    /// The enemy that has been despawned.
    /// </summary>
    Enemy Enemy { get; }
    IEnemyManager EnemyManager { get; }
}