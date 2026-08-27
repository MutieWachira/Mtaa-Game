using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public sealed class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Anchors")]
    [SerializeField] private Transform target;
    [SerializeField] private float heightOffset = 1.6f;
    
    [Header("AAA Framing Options")]
    [SerializeField] private float cameraDistance = 5.0f;
    [SerializeField] private float shoulderOffset = 0.4f; 
    [SerializeField] private float horizonLookAhead = 2.5f; 

    [Header("Responsiveness Configuration")]
    [SerializeField] private float mouseSensitivity = 15f;
    [SerializeField] private float gamepadSensitivity = 45f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Auto-Centering (Elden Ring / GTA Style)")]
    [SerializeField] private bool enableAutoCenter = true;
    [SerializeField] private float autoCenterDelay = 1.5f; 
    [SerializeField] private float autoCenterSpeed = 3.5f;    

    [Header("Combat Lock-On System")]
    [SerializeField] private Transform lockOnTarget; 
    [SerializeField] private float lockOnPitch = 15f;   

    [Header("Interpolation Metrics")]
    [SerializeField] private float positionSmoothTime = 0.03f;
    [SerializeField] private float rotationSmoothSpeed = 25f;

    [SerializeField] private PlayerInputReader inputReader;

    private float _yaw;
    private float _pitch;
    private Vector3 _positionVelocity;
    private float _lastInputTime;

    // PUBLIC ACCESSOR: PlayerMovement uses this to safely move relative to the camera view
    public float CameraYaw => _yaw;

    private void Awake()
    {
        InitializeOrientation();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateCameraPosition(true);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleRotationInput();
        UpdateCameraPosition(false);
    }

    private void InitializeOrientation()
    {
        if (target == null) return;
        _yaw = target.root.eulerAngles.y;
        _pitch = 12f;
    }

    private void HandleRotationInput()
    {
        Vector2 lookInput = inputReader.LookInput;
        Vector2 moveInput = inputReader.MoveInput;

        // Reset timer if the player moves the camera OR moves the character left/right/backwards
        if (lookInput.sqrMagnitude > 0.001f || Mathf.Abs(moveInput.x) > 0.1f || moveInput.y < -0.1f)
        {
            _lastInputTime = Time.time;
        }

        // Manual Camera Rotation Execution
        if (lookInput.sqrMagnitude > 0.001f)
        {
            float currentSensitivity = lookInput.sqrMagnitude > 100f ? mouseSensitivity : gamepadSensitivity;
            _yaw += lookInput.x * currentSensitivity * Time.deltaTime;
            _pitch -= lookInput.y * currentSensitivity * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            return;
        }

        // Combat Lock-On Processing
        if (lockOnTarget != null)
        {
            Vector3 directionToEnemy = lockOnTarget.position - target.position;
            directionToEnemy.y = 0; 
            if (directionToEnemy.sqrMagnitude > 0.001f)
            {
                Quaternion targetLook = Quaternion.LookRotation(directionToEnemy);
                _yaw = Mathf.LerpAngle(_yaw, targetLook.eulerAngles.y, rotationSmoothSpeed * Time.deltaTime);
                _pitch = Mathf.LerpAngle(_pitch, lockOnPitch, rotationSmoothSpeed * Time.deltaTime);
            }
            return;
        }

        // AAA Smart Auto-Centering (Only triggers when running forward, with NO side/back adjustments)
        if (enableAutoCenter && (Time.time - _lastInputTime >= autoCenterDelay))
        {
            if (moveInput.y > 0.1f && Mathf.Abs(moveInput.x) < 0.2f)
            {
                float playerCurrentYaw = target.root.eulerAngles.y;
                _yaw = Mathf.LerpAngle(_yaw, playerCurrentYaw, autoCenterSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateCameraPosition(bool instant)
    {
        Quaternion orbitRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 basePivotPosition = target.position + (Vector3.up * heightOffset);

        Vector3 rightOffsetVector = orbitRotation * Vector3.right * shoulderOffset;
        Vector3 backOffsetVector = orbitRotation * Vector3.forward * cameraDistance;
        Vector3 desiredCameraPosition = basePivotPosition + rightOffsetVector - backOffsetVector;

        Vector3 lookAheadTarget = basePivotPosition + rightOffsetVector + (orbitRotation * Vector3.forward * horizonLookAhead);

        if (instant)
        {
            transform.position = desiredCameraPosition;
            transform.rotation = Quaternion.LookRotation(lookAheadTarget - transform.position, Vector3.up);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredCameraPosition, ref _positionVelocity, positionSmoothTime);
            Quaternion targetLookRotation = Quaternion.LookRotation(lookAheadTarget - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetLookRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
}
