using System.Collections;

public interface IEnemyState
{
    void Enter();
    IEnumerator Execute();
    void CheckRange();
    void Exit();
}
