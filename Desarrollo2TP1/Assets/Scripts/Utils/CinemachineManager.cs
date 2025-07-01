using Unity.Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance;
    [SerializeField] CinemachineCamera _cinemachineCamera;
    private CinemachineBrain _cinemachineBrain;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _cinemachineBrain = this.GetComponent<CinemachineBrain>();
    }
        
}
