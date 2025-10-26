using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
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
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _inputHandler != null ? _inputHandler.MoveInput : Vector2.zero;

        if (moveInput.sqrMagnitude < _inputDeadzone * _inputDeadzone)
            return;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.1f);
        transform.TransformDirection(movement * moveSpeed * Time.deltaTime);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Raycast()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0, 2), transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.collider.gameObject.name);
        }
    }
}



