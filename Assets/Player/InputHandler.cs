using Unity.Cinemachine;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerControls _controls;


    public Vector2 MoveInput { get; private set; }
    public Vector2 MenuSelectInput { get; private set; }
    public bool InteractInput { get; private set; }

    public VoidEventChannel openRadialMenuEvent;
    public VoidEventChannel closeRadialMenuEvent;
    public VoidEventChannel toggleRadialMenuEvent;
    public InputSettings inputSettings;

    private void Awake()
    {
        _controls = new PlayerControls();

        // Movement
        _controls.MouseKeyboard.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Movement.canceled += _ => MoveInput = Vector2.zero;

        _controls.Controller.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Controller.Movement.canceled += _ => MoveInput = Vector2.zero;

        // Interact
        _controls.Controller.Interact.performed += ctx => InteractInput = true;
        _controls.Controller.Interact.canceled += _ => InteractInput = false;

        _controls.MouseKeyboard.Interact.performed += ctx => InteractInput = true;
        _controls.MouseKeyboard.Interact.canceled += _ => InteractInput = false;

        //Radial menu
        _controls.Controller.NoteWheel.performed += ctx => OnNoteWheelPerformed();
        _controls.Controller.NoteWheel.canceled += _ => OnNoteWheelCanceled();

        _controls.MouseKeyboard.NoteWheel.performed += ctx => OnNoteWheelPerformed();
        _controls.MouseKeyboard.NoteWheel.canceled += _ => OnNoteWheelCanceled();

        //Radial menu selection
        _controls.Controller.NoteWheelSelection.performed += ctx => MenuSelectInput = ctx.ReadValue<Vector2>();
        _controls.Controller.NoteWheelSelection.canceled += _ => MenuSelectInput = Vector2.zero;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void OnNoteWheelPerformed()
    {
        if (inputSettings.radialMenuHoldToOpen)
        {
            openRadialMenuEvent.RaiseEvent();
        }
        else
        {
            toggleRadialMenuEvent.RaiseEvent();
        }
    }

    private void OnNoteWheelCanceled()
    {
        if (inputSettings.radialMenuHoldToOpen)
        {
            closeRadialMenuEvent.RaiseEvent();
        }
    }
}
