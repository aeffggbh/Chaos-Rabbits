public interface IScene
{
    public enum Index
    {
        BOOT,
        MENU,
        LEVEL1,
        LEVEL2,
        FINAL_LEVEL,
        EXIT,
        LAST_GAMEPLAY,
        NONE
    }

    public Index buildIndex { get; }
}
