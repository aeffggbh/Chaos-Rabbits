using UnityEngine;

public enum SoundType
{
    SHOOT,
    EXPLOSION,
    JUMP,
    FOOTSTEPS,
    CONFIRM,
    LEVEL_UP
}


[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManger")]
public class SoundManager : ScriptableObject
{
    [SerializeField] private AudioClip[] soundClips;

    public void PlaySound(SoundType sound, AudioSource source, float volume = 1f)
    {
        source.PlayOneShot(soundClips[(int)sound], volume);
    }
}
