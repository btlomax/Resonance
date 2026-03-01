using Unity.Cinemachine;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerControls Controls { get => _controls; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 MenuSelectInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool TeleportInput { get; private set; }
    public bool IsPaused { get; set; } = false;
    public bool HasFlute { get; set; } = false;

    public VoidEventChannel OpenRadialMenuEvent;
    public VoidEventChannel CloseRadialMenuEvent;
    public VoidEventChannel ToggleRadialMenuEvent;
    public VoidEventChannel ToggleMajorMinor;
    public VoidEventChannel TogglePauseMenuEvent;
    public InputSettings inputSettings;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Global.Enable();   // Always on
        _controls.ControllerGameplay.Enable(); // Initially active

        PlayerFlute.PlayerCollectsFlute += () => HasFlute = true;

        // Movement
        _controls.MouseKeyboard.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.MouseKeyboard.Move.canceled += _ => MoveInput = Vector2.zero;

        _controls.ControllerGameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.ControllerGameplay.Move.canceled += _ => MoveInput = Vector2.zero;

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

        // Global pause
        _controls.Global.Pause.performed += ctx => OnPausePerformed();

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

        if(!HasFlute) return;

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
    }

    private void OnToggleMinorMajor()
    {
        if(IsPaused) return;

        ToggleMajorMinor.RaiseEvent();
    }
}
