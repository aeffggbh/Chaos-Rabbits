using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves all the sounds in the game
/// </summary>
[CreateAssetMenu(fileName = "SoundContainer", menuName = "ScriptableObjects/SoundContainer")]
public class SoundContainer : ScriptableObject
{
    [SerializeField] private List<SoundData> _sounds;

    private void OnDestroy()
    {
        ServiceProvider.SetService<SoundContainer>(null, true);
    }

    /// <summary>
    /// Returns the value of the key
    /// </summary>
    /// <param name="soundKey"></param>
    /// <param name="source"></param>
    /// <param name="volume"></param>
    public AudioClip GetClip(string soundKey)
    {
        foreach (var sound in _sounds)
            if (sound.Key == soundKey)
            {
                return sound.Clip;
            }

        return default;
    }
}

[Serializable]
public class SoundData
{
    /// <summary>
    /// Key to the audioclip
    /// </summary>
    [SerializeField] public string Key;
    /// <summary>
    /// Actual audioclip
    /// </summary>
    [SerializeField] public AudioClip Clip;
}