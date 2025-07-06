using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
[RequireComponent(typeof(Camera))]
public class CineMachineManager : MonoBehaviour
{
    public static CineMachineManager Instance;
    public CinemachineBrain cineMachineBrain;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        cineMachineBrain = this.GetComponent<CinemachineBrain>();
    }

}
