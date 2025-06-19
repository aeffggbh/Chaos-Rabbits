using UnityEngine;

/// <summary>
/// Base class for animation controllers.
/// </summary>
public abstract class AnimationController : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
    }

    protected void CheckAnimator()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator component not found on " + gameObject.name);
    }

}
