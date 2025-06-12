using System;
using UnityEngine;

/// <summary>
/// Manages the explosion effect in the game.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private float _secondsForExplosion;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionDuration;
    [SerializeField] public float _explosionRange;
    private float _currentExplosionDuration;
    private bool _explosionImminent = false;
    private bool _exploded = false;
    private float _timeForExplosion;
    private AudioSource _audioSource;
    private SoundManager _soundManager;

    private void Awake()
    {
        _timeForExplosion = 0;
        _explosionPrefab.SetActive(false);
        _currentExplosionDuration = 0;
        if (_explosionRange < 0.1f)
            Debug.LogError("Explosion range was not defined or it's too low");
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;
    }

    private void Update()
    {
        if (_explosionImminent)
        {
            _timeForExplosion += Time.deltaTime;

            if (_timeForExplosion >= _secondsForExplosion)
            {
                Explode();
                
            }
        }

        if (_exploded)
        {
            _currentExplosionDuration += Time.deltaTime;

            if (_currentExplosionDuration > _explosionDuration)
            {
                _explosionPrefab.SetActive(false);
                _currentExplosionDuration = 0;
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        _soundManager.PlaySound(SoundType.EXPLOSION, _audioSource);
        _explosionPrefab.SetActive(true);
        _explosionImminent = false;
        _timeForExplosion = 0;
        _exploded = true;
    }

    /// <summary>
    /// Returns true if the explosion has already happened.
    /// </summary>
    /// <returns></returns>
    public bool Exploded()
    {
        return _exploded;
    }

    public void StartExplotion()
    {
        _explosionImminent = true;
    }
}
