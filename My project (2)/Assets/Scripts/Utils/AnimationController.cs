using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    protected Animator animator;

    protected void Start()
    {
        if (GetComponent<Animator>() == null)
            Debug.LogError("Animator component not found on " + gameObject.name);
        else
            animator = GetComponent<Animator>();
    }
}
