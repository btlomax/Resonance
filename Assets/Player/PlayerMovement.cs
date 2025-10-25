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

        // Early out if no input (optional: still apply gravity if you use it)
        if (moveInput.sqrMagnitude < _inputDeadzone * _inputDeadzone)
            return;

        // Build camera-relative axes and flatten them to XZ plane
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0; // keep movement horizontal
        moveDirection.Normalize();

        // If move is effectively zero after all that (shouldn't be), bail
        if (moveDirection.sqrMagnitude < 0.0001f)
            return;

        // Normalize to avoid faster diagonal movement
        Vector3 moveDir = moveDirection.normalized;

        // Move using CharacterController
        _charController.Move(moveDir * moveSpeed * Time.deltaTime);

        // Smooth rotate the player to face movement direction
        //Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
        //Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
       // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
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



