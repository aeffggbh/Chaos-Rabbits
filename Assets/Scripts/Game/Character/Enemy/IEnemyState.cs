using System.Collections;

public interface IEnemyState
{
    /// <summary>
    /// Enters the state
    /// </summary>
    void Enter();
    /// <summary>
    /// Executes the state logic
    /// </summary>
    /// <returns></returns>
    IEnumerator Execute();
    /// <summary>
    /// Checks the distance range of the player
    /// </summary>
    void CheckRange();
    /// <summary>
    /// Exits the state
    /// </summary>
    void Exit();
}
