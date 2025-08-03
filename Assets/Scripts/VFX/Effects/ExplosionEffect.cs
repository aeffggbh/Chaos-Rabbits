using System.Collections;
using UnityEngine;

/// <summary>
/// The explosion effect
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ExplosionEffect : BaseEffect
{
    [SerializeField] private GameObject _effectGO;
    [SerializeField] private ISoundPlayer _soundPlayer;

    private void Awake()
    {
        if (_effectGO == null)
            Debug.LogError("No effect object");

        _effectGO.SetActive(false);

        _soundPlayer = new SoundPlayer(GetComponent<AudioSource>());

    }

    /// <summary>
    /// Plays the explosion
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator EffectRoutine()
    {
        Debug.Log("EXPLODING");
        _effectGO.SetActive(true);

        _soundPlayer.PlaySound("explosion");

        yield return new WaitForSeconds(effectDuration);

        _effectGO.SetActive(false);

        Complete();
    }
}
