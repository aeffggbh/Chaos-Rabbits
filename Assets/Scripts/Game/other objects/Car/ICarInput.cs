public interface ICarInput
{
    /// <summary>
    /// Saves the move input
    /// </summary>
    float MoveInput { get;}
    /// <summary>
    /// Saves the steer input
    /// </summary>
    float SteerInput { get;}
    /// <summary>
    /// Saves the brake input
    /// </summary>
    bool BrakeInput { get;}
}
