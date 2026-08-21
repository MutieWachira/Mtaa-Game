using UnityEngine;

/// <summary>
/// Handles player locomotion using input supplied by PlayerInputReader.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed = 3f;

    [SerializeField]
    private float runSpeed = 6f;

    [Header("Jump")]
    [SerializeField]
    private float jumpHeight = 1.5f;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 10f;

    [Header("Camera")]
    [SerializeField]
    private Transform cameraTransform;

    [Header("Gravity")]
    [SerializeField]
    private float gravity = -20f;

    private CharacterController _characterController;
    private PlayerInputReader _inputReader;

    private float _verticalVelocity;

    /// <summary>
    /// Gets the player's current movement state.
    /// </summary>
    public MovementState CurrentState { get; private set; }
    private MovementState _previousState;

    private void Awake()
    {
        _characterController =
            GetComponent<CharacterController>();

        _inputReader =
            GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        Vector3 movement = CalculateMovementDirection();

        UpdateMovementState(movement);

        HandleMovement(movement);

        HandleJump();

        ApplyGravity();

        Debug.Log(CurrentState);
    }

    private Vector3 CalculateMovementDirection()
    {
        Vector2 input = _inputReader.MoveInput;

        if (input.sqrMagnitude <= 0.001f)
        {
            return Vector3.zero;
        }

        Vector3 cameraForward =
            cameraTransform.forward;

        Vector3 cameraRight =
            cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement =
            cameraForward * input.y +
            cameraRight * input.x;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        return movement;
    }

    private void HandleMovement(Vector3 movement)
    {
        if (movement.sqrMagnitude <= 0.001f)
        {
            return;
        }

        float currentSpeed =
            _inputReader.SprintInput
                ? runSpeed
                : walkSpeed;

        RotateTowardsMovement(movement);

        _characterController.Move(
            movement *
            currentSpeed *
            Time.deltaTime
        );
    }

    private void RotateTowardsMovement(
        Vector3 movement)
    {
        Quaternion targetRotation =
            Quaternion.LookRotation(movement);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded &&
            _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity +=
            gravity * Time.deltaTime;

        Vector3 verticalMovement =
            Vector3.up * _verticalVelocity;

        _characterController.Move(
            verticalMovement *
            Time.deltaTime
        );
    }

   private void UpdateMovementState(
    Vector3 movement)
{
    MovementState newState;

    if (!_characterController.isGrounded)
    {
        newState =
            _verticalVelocity > 0f
                ? MovementState.Jumping
                : MovementState.Falling;
    }
    else if (movement.sqrMagnitude <= 0.001f)
    {
        newState = MovementState.Idle;
    }
    else
    {
        newState =
            _inputReader.SprintInput
                ? MovementState.Running
                : MovementState.Walking;
    }

    if (newState != CurrentState)
    {
        _previousState = CurrentState;

        CurrentState = newState;

        Debug.Log(
            $"[PlayerMovement] " +
            $"{_previousState} -> {CurrentState}"
        );
    }
}

    private void HandleJump()
    {
        if(!_characterController.isGrounded)
        {
            return;
        }
        if (!_inputReader.JumpPressed)
        {
            return;
        }
        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}