using UnityEngine;

/// <summary>
/// Synchronizes the player's movement state with
/// the character's Animator.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public sealed class PlayerAnimationController : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int GroundedHash =
        Animator.StringToHash("Grounded");

    private static readonly int JumpHash =
        Animator.StringToHash("Jump");

    [SerializeField]
    private Animator animator;

    private PlayerMovement _movement;

    private void Awake()
    {
        _movement =
            GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        float speed = 0f;

        switch (_movement.CurrentState)
        {
            case MovementState.Walking:
                speed = 0.5f;
                break;

            case MovementState.Running:
                speed = 1f;
                break;
        }

        animator.SetFloat(
            SpeedHash,
            speed
        );

        bool grounded =
            _movement.CurrentState !=
            MovementState.Falling &&
            _movement.CurrentState !=
            MovementState.Jumping;

        animator.SetBool(
            GroundedHash,
            grounded
        );

        if (_movement.CurrentState ==
            MovementState.Jumping)
        {
            animator.SetTrigger(JumpHash);
        }
    }
}