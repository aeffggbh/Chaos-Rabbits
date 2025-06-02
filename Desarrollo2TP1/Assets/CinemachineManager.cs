using Unity.Cinemachine;
using UnityEngine;

//TODO: en vez de un singleton añadir a serviceprovider
public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instance;
    [SerializeField] CinemachineCamera _cinemachineCamera;
    private CinemachineBrain _cinemachineBrain;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _cinemachineBrain = this.GetComponent<CinemachineBrain>();
    }
        
}
