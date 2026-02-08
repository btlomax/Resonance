using Unity.Cinemachine;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerControls _controls;
    public Vector2 MoveInput { get; private set; }
    public Vector2 MenuSelectInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool TeleportInput { get; private set; }

    public bool IsPaused { get; private set; } = false;

    public VoidEventChannel OpenRadialMenuEvent;
    public VoidEventChannel CloseRadialMenuEvent;
    public VoidEventChannel ToggleRadialMenuEvent;
    public VoidEventChannel ToggleMajorMinor;
    public VoidEventChannel TogglePauseMenuEvent;
    public InputSettings inputSettings;

    private void Awake()
    {
        _controls = new PlayerControls();

        // Movement
        _controls.MouseKeyboard.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Movement.canceled += _ => MoveInput = Vector2.zero;

        _controls.ControllerGameplay.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.ControllerGameplay.Movement.canceled += _ => MoveInput = Vector2.zero;

        // Jumping
        _controls.ControllerGameplay.Jump.performed += ctx => JumpInput = true;
        _controls.ControllerGameplay.Jump.canceled += ctx => JumpInput = false;

        // Interact
        _controls.ControllerGameplay.Interact.performed += ctx => InteractInput = true;
        _controls.ControllerGameplay.Interact.canceled += _ => InteractInput = false;

        _controls.MouseKeyboard.Interact.performed += ctx => InteractInput = true;
        _controls.MouseKeyboard.Interact.canceled += _ => InteractInput = false;

        // Teleport
        _controls.ControllerGameplay.TeleportBackToEmitter.performed += ctx => TeleportInput = true;
        _controls.ControllerGameplay.TeleportBackToEmitter.canceled += _ => TeleportInput = false;

        // Pause menu
        _controls.ControllerGameplay.Pause.performed += ctx => OnPausePerformed();
        _controls.MouseKeyboard.Pause.performed += ctx => OnPausePerformed();

        //Radial menu
        _controls.ControllerGameplay.NoteWheel.performed += ctx => OnNoteWheelPerformed();
        _controls.ControllerGameplay.NoteWheel.canceled += _ => OnNoteWheelCanceled();

        _controls.MouseKeyboard.NoteWheel.performed += ctx => OnNoteWheelPerformed();
        _controls.MouseKeyboard.NoteWheel.canceled += _ => OnNoteWheelCanceled();

        _controls.ControllerGameplay.ToggleMajorMinor.performed += ctx => OnToggleMinorMajor();
       // _controls.Controller.ToggleMajorMinor.canceled += _ => OnToggleMinorMajor();

        //Radial menu selection
        _controls.ControllerGameplay.NoteWheelSelection.performed += ctx => MenuSelectInput = ctx.ReadValue<Vector2>();
        _controls.ControllerGameplay.NoteWheelSelection.canceled += _ => MenuSelectInput = Vector2.zero;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void OnNoteWheelPerformed()
    {
        if(IsPaused) return;

        if (inputSettings.radialMenuHoldToOpen)
        {
            OpenRadialMenuEvent.RaiseEvent();
        }
        else
        {
            ToggleRadialMenuEvent.RaiseEvent();
        }
    }

    private void OnNoteWheelCanceled()
    {
        if (inputSettings.radialMenuHoldToOpen)
        {
            CloseRadialMenuEvent.RaiseEvent();
        }
    }

    private void OnPausePerformed()
    {
        TogglePauseMenuEvent.RaiseEvent();
        IsPaused = !IsPaused;
    }

    private void OnToggleMinorMajor()
    {
        if(IsPaused) return;

        ToggleMajorMinor.RaiseEvent();
    }
}
