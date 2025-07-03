using UnityEngine;

/// <summary>
/// Manages the explosion effect in the game.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private float _secondsForExplosion;
    [SerializeField] private GameObject _explosionPrefabGO;
    [SerializeField] private float _explosionDuration;
    [SerializeField] public float _explosionRange;
    private float _currentExplosionDuration;
    private bool _explosionImminent = false;
    private bool _exploded = false;
    private float _timeForExplosion;
    private AudioSource _audioSource;
    private ISoundPlayer _soundPlayer;

    private void Awake()
    {
        _timeForExplosion = 0;
        _explosionPrefabGO.SetActive(false);
        _currentExplosionDuration = 0;
        if (_explosionRange < 0.1f)
            Debug.LogError("Explosion range was not defined or it's too low");
        _audioSource = GetComponent<AudioSource>();
        _soundPlayer = new SoundPlayer(_audioSource);
    }

    private void Update()
    {
        if (_explosionImminent)
        {
            _timeForExplosion += Time.deltaTime;

            if (_timeForExplosion >= _secondsForExplosion)
                Explode();
        }

        if (_exploded)
        {
            _currentExplosionDuration += Time.deltaTime;

            if (_currentExplosionDuration > _explosionDuration)
            {
                _explosionPrefabGO.SetActive(false);
                _currentExplosionDuration = 0;
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        _soundPlayer.PlaySound(SFXType.EXPLOSION);
        _explosionPrefabGO.SetActive(true);
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

    public void StartExplosion()
    {
        _explosionImminent = true;
    }
}
