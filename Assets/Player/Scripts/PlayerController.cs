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
    private Vector3 _movement;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGrounded();

        Debug.DrawRay(transform.position + Vector3.up * groundCheckStartPoint, Vector3.down * groundCheckDistance,
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
        _movement = camForward * moveInput.y + camRight * moveInput.x;

        // Rotate the player toward movement direction
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(_movement),
            0.1f
        );

        _charController.Move(_movement * moveSpeed * Time.deltaTime);
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

        if (!grounded)
        {
           _charController.SimpleMove(Vector3.up * gravity);
        }
    }

    private void HandleFocus()
    {
        _ray.origin = transform.position + new Vector3(0, 1, 0);
        _ray.direction = transform.forward;

        Debug.DrawRay(_ray.origin, _ray.direction * rayLength, Color.red);

        if (Physics.Raycast(_ray, out RaycastHit hitInfo, rayLength))
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



