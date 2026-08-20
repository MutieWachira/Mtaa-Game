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
    private float moveSpeed = 5f;

    [SerializeField]
    private float gravity = -20f;

    private CharacterController _characterController;
    private PlayerInputReader _inputReader;

    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector2 input = _inputReader.MoveInput;

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        _characterController.Move(
            movement * moveSpeed * Time.deltaTime
        );
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += gravity * Time.deltaTime;

        Vector3 verticalMovement = Vector3.up * _verticalVelocity;

        _characterController.Move(
            verticalMovement * Time.deltaTime
        );
    }
}