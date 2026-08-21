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

    [Header("Camera")]
    [SerializeField]
    private Transform cameraTransform;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 10f;

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

       if (input.sqrMagnitude <= 0.001f)
       {
        return;
       }
       Vector3 cameraForward = cameraTransform.forward;
       Vector3 cameraRight = cameraTransform.right;

          cameraForward.y = 0f;
          cameraRight.y = 0f;
    
          cameraForward.Normalize();
          cameraRight.Normalize();
    
          Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
          
          if(moveDirection.sqrMagnitude > 1f)
          {
            moveDirection.Normalize();
          }

          if(moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed* Time.deltaTime
            );
        }            
          _characterController.Move(
                moveDirection * moveSpeed * Time.deltaTime
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