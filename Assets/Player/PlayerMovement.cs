using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 3f;
    public Transform cameraTransform;

    private CharacterController _characterController;
    private PlayerControls _controls;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private float _xRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _controls = new PlayerControls();

        _controls.MouseKeyboard.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Movement.canceled += ctx => _moveInput = Vector2.zero;

        _controls.Controller.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();

        // _controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        // _controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        Raycast();
    }

    private void HandleMovement()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camForward * _moveInput.y + camRight * _moveInput.x;

        if (move.magnitude > 0.1f)
        {
            // Rotate player toward movement direction
            transform.forward = Vector3.Slerp(transform.forward, move, lookSpeed * Time.deltaTime);
        }

           _characterController.Move(move * moveSpeed * Time.deltaTime);
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
