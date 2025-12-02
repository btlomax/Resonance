using Assets.Player.Contracts;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Need to think about breaking this into smaller components later
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Ray Settings")]
    [Tooltip("How far the raycast checks forward.")]
    [Range(0f, 2f)]
    public float rayLength = 5f;
    [Range(0f, 2f)]
    public float groundCheckDistance = 0.5f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public bool grounded;

    [Header("References")]
    public Transform cameraTransform; // assign CameraTarget or camera
    public CinemachineCamera cam;

    [SerializeField]
    private Interactable _currentFocus;
    [SerializeField]
    private InputHandler _inputHandler;

    private const float _inputDeadzone = 0.01f;
    private CharacterController _charController;
    private Ray _ray;
    private bool _hasInteracted = false;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundCheckDistance,
               grounded ? Color.green : Color.red);

        HandleMovement();

        HandleFocus();

        if (_inputHandler.InteractInput)
        {
            TryInteract();
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _inputHandler != null ? _inputHandler.MoveInput : Vector2.zero;

        if (moveInput.sqrMagnitude < _inputDeadzone * _inputDeadzone)
            return;

        // Get camera forward and right, but flatten to prevent tilting on slopes
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Build movement relative to camera
        Vector3 movement = camForward * moveInput.y + camRight * moveInput.x;

        // Rotate the player toward movement direction
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(movement),
            0.1f
        );

        if(CheckGrounded() && movement.y < 0)
        {
            // Apply gravity when grounded
            movement.y = -2f;
        }
        else
        {
            // Apply gravity when in air
            movement.y += gravity * Time.deltaTime;
        }

        _charController.Move(movement * moveSpeed * Time.deltaTime);
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

    private bool CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);

       

        return grounded;
    }

    private void HandleFocus()
    {
        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        Debug.DrawRay(_ray.origin, _ray.direction * rayLength, Color.red);

        if (Physics.Raycast(_ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out IObjectInteraction objectInteraction))
            {
                if (objectInteraction is Interactable interactable)
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



