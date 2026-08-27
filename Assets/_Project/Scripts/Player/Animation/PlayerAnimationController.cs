using UnityEngine;

/// <summary>
/// Synchronizes the player's physical weight and movement state with the character's Animator.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public sealed class PlayerAnimationController : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    [Header("Dependencies")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float speedDampTime = 8f; // Smooths out sudden animation transitions
    [SerializeField] private float maxSprintSpeed = 6.5f; // Must match sprintSpeed in PlayerMovement

    private PlayerMovement _movement;
    private MovementState _lastEvaluatedState;
    private float _currentAnimSpeed;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError($"{nameof(PlayerAnimationController)} is missing an Animator reference.", this);
            enabled = false;
            return;
        }

        _lastEvaluatedState = _movement.CurrentState;
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        MovementState currentState = _movement.CurrentState;

        // 1. AAA DYNAMIC BLENDING: Read true physical speed and convert it to a 0-1 range
        float absolutePhysicalSpeed = _movement.CurrentRelativeForwardSpeed;
        float targetSpeedBlend = absolutePhysicalSpeed / maxSprintSpeed;

        // Smooth out the blend value so the character leans into walks and runs beautifully
        _currentAnimSpeed = Mathf.MoveTowards(_currentAnimSpeed, targetSpeedBlend, speedDampTime * Time.deltaTime);
        animator.SetFloat(SpeedHash, _currentAnimSpeed);

        // 2. Resolve Grounded state
        bool grounded = currentState != MovementState.Falling && currentState != MovementState.Jumping;
        animator.SetBool(GroundedHash, grounded);

        // 3. Handle Jump Trigger ONCE on state entry
        if (currentState == MovementState.Jumping && _lastEvaluatedState != MovementState.Jumping)
        {
            animator.SetTrigger(JumpHash);
        }

        _lastEvaluatedState = currentState;
    }
}
