using Assets.Player.Contracts;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Need to think about breaking this into smaller components later
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Interact Ray Settings")]
    [Tooltip("How far the raycast checks forward.")]
    [Range(0f, 5f)]
    public float rayLength = 5f;

    [Header("Ground Check Settings")]
    [Range(0f, 2f)]
    public float groundCheckDistance = 0.5f;
    [Range(0f, 2f)]
    public float groundCheckStartPoint = 0f;
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 2f;
    public Vector3 verticalVelocity;
    public float gravity = -20f;
    public bool grounded;
    public Vector3 verticalOffset;

    [Header("References")]
    public Transform cameraTransform; // assign CameraTarget or camera
    public CinemachineCamera cam;
    public Transform emitterTeleport;

    [SerializeField]
    private Interactable _currentFocus;
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private Animator _animator;

    private const float _inputDeadzone = 0.01f;
    private CharacterController _charController;
    private Ray _ray;
    private bool _hasInteracted = false;
    private Vector3 _movement;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        PlayerFlute.PlayerCollectsFlute += () =>
        {
            _animator.SetBool("hasFlute", true); // Make sure "hasFlute" exists in your Animator
        };
    }

    private void Update()
    {
        if (_inputHandler.IsPaused)
            return;

        CheckGrounded();

        Debug.DrawRay(transform.position + Vector3.up * groundCheckStartPoint, Vector3.down * groundCheckDistance,
               grounded ? Color.green : Color.red);

        HandleMovement();

        UpdateFocus();

        if (_inputHandler.InteractInput)
        {
            TryInteract();
        }

        if(emitterTeleport != null && _inputHandler.TeleportInput)
        {
            transform.position = emitterTeleport.position + verticalOffset;
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _inputHandler != null ? _inputHandler.MoveInput : Vector2.zero;

        // Get camera forward and right, but flatten to prevent tilting on slopes
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Build movement relative to camera
        _movement = camForward * moveInput.y + camRight * moveInput.x;
        _movement = Vector3.ClampMagnitude(_movement, 1f);

        bool isMoving = _movement.sqrMagnitude > _inputDeadzone * _inputDeadzone;
        _animator.SetBool("isMoving", isMoving);

        if(_movement.sqrMagnitude > _inputDeadzone * _inputDeadzone)
        {
            // Rotate the player toward movement direction
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_movement),
                0.1f
            );
        }

        if (grounded)
        {
            if (verticalVelocity.y < 0)
                verticalVelocity.y = -2f; // small downward force to keep grounded

            if (_inputHandler.JumpInput)
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        // Final movement
        Vector3 finalMove =
            _movement * moveSpeed +
            verticalVelocity;

        _charController.Move(finalMove * Time.deltaTime);
    }

    private void TryInteract()
    {
       if (_currentFocus != null && !_hasInteracted)
       {
           _currentFocus.Interact(gameObject);
           _hasInteracted = true;

           StartCoroutine(WaitAfterInteract(1f));
        }
    }

    private void CheckGrounded()
    {
        if(Physics.Raycast(transform.position + Vector3.up * groundCheckStartPoint, Vector3.down, groundCheckDistance, groundLayer))
            grounded = true;
        else
            grounded = false;
    }

    private void UpdateFocus()
    {
        _ray.origin = transform.position + new Vector3(0, 1, 0);
        _ray.direction = transform.forward;

        Debug.DrawRay(_ray.origin, _ray.direction * rayLength, Color.red);

        if (Physics.Raycast(_ray, out RaycastHit hitInfo, rayLength))
        {
            if (hitInfo.transform.TryGetComponent(out IObjectInteraction objectInteraction))
            {
                if (objectInteraction is Interactable interactable) // Don't think I need Interactable, I can just use the interface instead
                {
                    if (_currentFocus != interactable)
                    {
                        _currentFocus?.OnFocusExit();
                        _currentFocus = interactable;
                        _currentFocus.OnFocusEnter();
                    }
                }
                return;
            }
        }

        if (_currentFocus != null)
        {
            _currentFocus.OnFocusExit();
            _currentFocus = null;
        }
    }

    private IEnumerator WaitAfterInteract(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _hasInteracted = false;
    }
}





