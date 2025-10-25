using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform; // assign CameraTarget or camera

    [SerializeField]
    private InputHandler _inputHandler;
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
      if(_inputHandler.MoveInput.sqrMagnitude > 0.01f)
      {
          Vector3 moveDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
          moveDirection = cameraTransform.TransformDirection(moveDirection);
          moveDirection.y = 0; // keep movement horizontal
          moveDirection.Normalize();

          // Move the character
          _charController.Move(moveDirection * moveSpeed * Time.deltaTime);
      }
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



