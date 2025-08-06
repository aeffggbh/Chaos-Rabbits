public interface IEnemyDespawnEvent : IEvent
{
    /// <summary>
    /// The enemy that has been despawned.
    /// </summary>
    Enemy Enemy { get; }
    IEnemyContainer EnemyManager { get; }
}
