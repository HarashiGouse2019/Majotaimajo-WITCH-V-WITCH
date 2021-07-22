using UnityEngine;

public class GraphicAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public void Animate(bool active)
    {
        animator.SetBool("active", active);
    }

    public void SetInactive() => gameObject.SetActive(false);
}
