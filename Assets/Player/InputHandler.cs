using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    private void Awake()
    {
        _controls = new PlayerControls();

        // Movement
        _controls.MouseKeyboard.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Movement.canceled += _ => MoveInput = Vector2.zero;

        _controls.Controller.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Controller.Movement.canceled += _ => MoveInput = Vector2.zero;

        // Look
        _controls.MouseKeyboard.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Look.canceled += _ => LookInput = Vector2.zero;

        _controls.Controller.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        _controls.Controller.Look.canceled += _ => LookInput = Vector2.zero;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();
}
