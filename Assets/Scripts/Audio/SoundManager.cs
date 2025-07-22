using UnityEngine;
//TODO: Improve this.
// - Not single respons
// - Its using an enum >:c

/// <summary>
/// Each sound type is saved here
/// </summary>
public enum SFXType
{
    SHOOT,
    EXPLOSION,
    JUMP,
    FOOTSTEPS,
    CONFIRM,
    LEVEL_UP,
    TAKE_HIT
}

/// <summary>
/// Manages all the sounds in the game
/// </summary>
[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManger")]
public class SoundManager : ScriptableObject
{
    [SerializeField] private AudioClip[] soundClips;

    private void OnDestroy()
    {
        ServiceProvider.SetService<SoundManager>(null, true);
    }

    /// <summary>
    /// Plays the indicated sound
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="source"></param>
    /// <param name="volume"></param>
    public void PlaySound(SFXType sound, AudioSource source, float volume = 1f)
    {
        source.PlayOneShot(soundClips[(int)sound], volume);
    }
}
