using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField]private Animator animator;

    [SerializeField] private InputAction attackAction;
    [SerializeField] private InputAction crouchAction;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        attackAction.Enable();
        crouchAction.Enable();
    }

    void OnDisable()
    {
        attackAction.Disable();
        crouchAction.Disable();
    }

    void Update()
    {
        if (attackAction.triggered)
        {
            animator.SetTrigger("FightAnimation");
        }

        if (crouchAction.triggered)
        {
            animator.SetTrigger("DanceAnimation");
        }
    }
}