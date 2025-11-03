using Assets.Player.Contracts;
using UnityEngine;

/// <summary>
/// Need to think about breaking this into smaller components later
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Ray Settings")]
    [Tooltip("How far the raycast checks forward.")]
    [Range(0f, 20f)]
    public float rayLength = 5f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public Transform cameraTransform; // assign CameraTarget or camera

    [SerializeField]
    private Interactable _currentFocus;
    [SerializeField]
    private InputHandler _inputHandler;

    private const float _inputDeadzone = 0.01f;
    private CharacterController _charController;
    private Ray _ray;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
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

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.1f);
        transform.TransformDirection(movement * moveSpeed * Time.deltaTime);
        // transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        _charController.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void TryInteract()
    {
       if (_currentFocus != null)
       {
           _currentFocus.Interact(gameObject);
       }
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
}



