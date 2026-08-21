using UnityEngine;

/// <summary>
/// Controls the third-person camera around a target transform.
/// Camera input is provided by PlayerInputReader so that the camera
/// remains independent of the physical input device.
/// </summary>
[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    private Transform target;

    [Header("Input")]
    [SerializeField]
    private PlayerInputReader inputReader;

    [Header("Distance")]
    [SerializeField]
    private float distance = 5f;

    [Header("Orbit")]
    [SerializeField]
    private float sensitivity = 2f;

    [SerializeField]
    private float minVerticalAngle = -30f;

    [SerializeField]
    private float maxVerticalAngle = 70f;

    [Header("Smoothing")]
    [SerializeField]
    private float positionSmoothTime = 0.08f;

    [Header("Collision")]
    [SerializeField]
    private LayerMask collisionLayers;

    [SerializeField]
    private float collisionRadius = 0.2f;

    [SerializeField]
    private float minimumDistance = 1f;

    private float _yaw;
    private float _pitch;

    private Vector3 _positionVelocity;

    private void Awake()
    {
        if (inputReader == null)
        {
            Debug.LogError(
                "[CameraController] PlayerInputReader is not assigned.",
                this
            );
        }

        if (target == null)
        {
            Debug.LogError(
                "[CameraController] Camera target is not assigned.",
                this
            );
        }
    }

    private void LateUpdate()
    {
        if (target == null || inputReader == null)
        {
            return;
        }

        HandleCameraRotation();
        UpdateCameraPosition();
    }

    /// <summary>
    /// Updates camera yaw and pitch using the configured input system.
    /// </summary>
    private void HandleCameraRotation()
    {
        Vector2 lookInput = inputReader.LookInput;

        _yaw += lookInput.x * sensitivity;
        _pitch -= lookInput.y * sensitivity;

        _pitch = Mathf.Clamp(
            _pitch,
            minVerticalAngle,
            maxVerticalAngle
        );
    }

    /// <summary>
    /// Calculates and smoothly moves the camera to its desired position.
    /// </summary>
    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(
            _pitch,
            _yaw,
            0f
        );

        Vector3 direction =
            -(rotation * Vector3.forward);

        Vector3 desiredPosition =
            target.position +
            direction * distance;

        float actualDistance = distance;

        if (Physics.SphereCast(
            target.position,
            collisionRadius,
            direction,
            out RaycastHit hit,
            distance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            actualDistance = Mathf.Max(
                minimumDistance,
                hit.distance
            );
        }

        desiredPosition =
            target.position +
            direction * actualDistance;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref _positionVelocity,
            positionSmoothTime
        );

        transform.LookAt(target);
    }
}