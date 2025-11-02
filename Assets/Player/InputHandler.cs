using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool InteractInput { get; private set; }

    public bool RadialMenuInput { get; private set; }

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

        // Interact
        _controls.Controller.Interact.performed += ctx => InteractInput = true;
        _controls.Controller.Interact.canceled += _ => InteractInput = false;

        _controls.MouseKeyboard.Interact.performed += ctx => InteractInput = true;
        _controls.MouseKeyboard.Interact.canceled += _ => InteractInput = false;

        //Open radia menu
        _controls.Controller.NoteWheel.performed += ctx => RadialMenuInput = true;
        _controls.Controller.NoteWheel.canceled += _ => RadialMenuInput = false;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();
}
