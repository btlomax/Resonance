using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ray Settings")]
    [Tooltip("How far the raycast checks forward.")]
    [Range(0f, 20f)]
    public float rayLength = 5f;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform; // assign CameraTarget or camera

    [SerializeField]
    private InputHandler _inputHandler;
    private const float _inputDeadzone = 0.01f;
    private CharacterController _charController;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();

        Raycast();
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

    private void Raycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.collider.gameObject.name);
        }
    }
}



