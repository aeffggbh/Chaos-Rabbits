using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private float _secondsForExplosion;
    [SerializeField] private GameObject _explosionPrefab;
    private bool explosionImminent = false;
    private bool exploded = false;
    private float _timeForExplosion;

    private void Start()
    {
        _timeForExplosion = 0;
        _explosionPrefab.SetActive(false);
    }

    private void Update()
    {
        if (explosionImminent)
        {
            _timeForExplosion+= Time.deltaTime;

            if (_timeForExplosion >= _secondsForExplosion)
            {
                _explosionPrefab.SetActive(true);
                explosionImminent = false;
                _timeForExplosion = 0;
                exploded = true;
            }
        }
    }

    /// <summary>
    /// Returns true if the explosion has already happened.
    /// </summary>
    /// <returns></returns>
    public bool Exploded()
    {
        return exploded;
    }

    public void StartExplotion()
    {
        explosionImminent = true;
    }
}
