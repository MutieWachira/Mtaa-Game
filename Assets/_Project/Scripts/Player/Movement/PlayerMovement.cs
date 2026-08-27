using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Locomotion Speeds")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float jogSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 6.5f;

    [Header("Momentum & Weight")]
    [SerializeField] private float accelerationSpeed = 8.0f; 
    [SerializeField] private float decelerationSpeed = 12.0f; 

    [Header("Dynamic AAA Turning")]
    [SerializeField] private float baseRotationSpeed = 12.0f;
    [SerializeField] private float tightTurnThreshold = 110f; 

    [Header("Advanced Grounding")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckOffset = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.28f;

    [Header("Jump & Environment")]
    [SerializeField] private float jumpHeight = 1.6f;
    [SerializeField] private float gravity = -22.0f;
    
    // FIX: Change Transform reference directly to your custom camera component
    [Header("Camera Reference Link")]
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    private CharacterController _characterController;
    private PlayerInputReader _inputReader;

    private Vector3 _currentInputVector;
    private Vector3 _targetInputVector;
    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;
    private bool _isGroundedCleanly;

    public MovementState CurrentState { get; private set; }
    public float CurrentRelativeForwardSpeed => _horizontalVelocity.magnitude;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        EvaluateCustomGroundCheck();
        
        Vector3 movementDirection = CalculateSmoothInputDirection();
        
        ApplyAAAMomentum(movementDirection);
        HandleJump();
        ApplyGravity();

        Vector3 finalizedLocomotionFrame = (_horizontalVelocity + (Vector3.up * _verticalVelocity)) * Time.deltaTime;
        _characterController.Move(finalizedLocomotionFrame);

        RotateTowardsMovement(movementDirection);
        UpdateMovementState(movementDirection);
    }

    private void EvaluateCustomGroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundCheckRadius - groundCheckOffset, transform.position.z);
        _isGroundedCleanly = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private Vector3 CalculateSmoothInputDirection()
    {
        Vector2 rawInput = _inputReader.MoveInput;
        _targetInputVector = new Vector3(rawInput.x, 0f, rawInput.y);

        float currentInterpolationRate = _targetInputVector.sqrMagnitude > 0.001f ? accelerationSpeed : decelerationSpeed;
        _currentInputVector = Vector3.MoveTowards(_currentInputVector, _targetInputVector, currentInterpolationRate * Time.deltaTime);

        if (_currentInputVector.sqrMagnitude <= 0.001f) return Vector3.zero;

        // FIX: Extract stable, independent directional references based on pure angles
        // This stops the camera position from affecting calculation matrices on the same frame.
        float targetCameraAngle = thirdPersonCamera != null ? thirdPersonCamera.CameraYaw : 0f;
        Quaternion cameraRotationSnapshot = Quaternion.Euler(0f, targetCameraAngle, 0f);

        Vector3 cameraForwardFlat = cameraRotationSnapshot * Vector3.forward;
        Vector3 cameraRightFlat = cameraRotationSnapshot * Vector3.right;

        Vector3 finalDirection = (cameraForwardFlat * _currentInputVector.z) + (cameraRightFlat * _currentInputVector.x);
        
        return finalDirection.sqrMagnitude > 1f ? finalDirection.normalized : finalDirection;
    }

    private void ApplyAAAMomentum(Vector3 movementDirection)
    {
        if (movementDirection.sqrMagnitude <= 0.001f)
        {
            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, Vector3.zero, decelerationSpeed * Time.deltaTime);
            return;
        }

        float targetMaxSpeed = walkSpeed;
        if (_inputReader.SprintInput)
        {
            targetMaxSpeed = sprintSpeed;
        }
        else if (_targetInputVector.magnitude > 0.7f)
        {
            targetMaxSpeed = jogSpeed;
        }

        Vector3 idealVelocity = movementDirection * targetMaxSpeed;
        _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, idealVelocity, accelerationSpeed * Time.deltaTime);
    }

    private void RotateTowardsMovement(Vector3 movementDirection)
    {
        if (movementDirection.sqrMagnitude <= 0.001f) return;

        float targetCameraAngle = thirdPersonCamera != null ? thirdPersonCamera.CameraYaw : 0f;
        Quaternion cameraRotationSnapshot = Quaternion.Euler(0f, targetCameraAngle, 0f);
        
        Vector3 rawTargetDir = (cameraRotationSnapshot * Vector3.forward * _targetInputVector.z) + (cameraRotationSnapshot * Vector3.right * _targetInputVector.x);
        if (rawTargetDir.sqrMagnitude <= 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(rawTargetDir.normalized);
        float angleDifference = Vector3.Angle(transform.forward, rawTargetDir.normalized);
        
        float currentRotationSpeed = baseRotationSpeed;
        if (angleDifference > tightTurnThreshold)
        {
            // Slow down the turn speed during rapid u-turns so the player runs in a realistic arc
            currentRotationSpeed *= 0.35f; 
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (!_isGroundedCleanly) return;
        if (!_inputReader.JumpPressed) return;

        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        if (_isGroundedCleanly && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f; 
            return;
        }

        _verticalVelocity += gravity * Time.deltaTime;
    }

    private void UpdateMovementState(Vector3 movementDirection)
    {
        MovementState newState;

        if (!_isGroundedCleanly)
        {
            newState = _verticalVelocity > 0f ? MovementState.Jumping : MovementState.Falling;
        }
        else if (_horizontalVelocity.sqrMagnitude <= 0.1f)
        {
            newState = MovementState.Idle;
        }
        else
        {
            float speed = _horizontalVelocity.magnitude;
            if (speed > jogSpeed + 0.5f) newState = MovementState.Running;
            else if (speed > walkSpeed + 0.2f) newState = MovementState.Walking; 
            else newState = MovementState.Walking;
        }

        CurrentState = newState;
    }
}
