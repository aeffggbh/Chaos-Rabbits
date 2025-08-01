
public class LevelCarMechanic : ILevelMechanic, IMechanicTextInfo
{
    public bool ObjectiveCompleted => true;

    public string GetObjectiveText()
    {
        return "Wow, you have a car? You should drive it to the end!";
    }
}