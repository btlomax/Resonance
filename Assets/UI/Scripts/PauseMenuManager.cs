using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private GameObject _firstButton;

    public VoidEventChannel TogglePauseMenuEvent;
    public GameObject pauseMenuUIBase;

    private void OnEnable()
    {
        TogglePauseMenuEvent.OnEventRaised += OnTogglePauseMenu;
    }

    private void OnDisable()
    {
        TogglePauseMenuEvent.OnEventRaised -= OnTogglePauseMenu;
    }

    public void OnTogglePauseMenu()
    {
        _inputHandler.IsPaused = !_inputHandler.IsPaused;
        pauseMenuUIBase.SetActive(!pauseMenuUIBase.activeSelf);

        Time.timeScale = pauseMenuUIBase.activeSelf ? 0f : 1f;

        if(pauseMenuUIBase.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(_firstButton);
            _inputHandler.Controls.ControllerGameplay.Disable();
            _inputHandler.Controls.MouseKeyboard.Disable();
            _inputHandler.Controls.ControllerUI.Enable();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            _inputHandler.Controls.ControllerGameplay.Enable();
            _inputHandler.Controls.MouseKeyboard.Enable();
            _inputHandler.Controls.ControllerUI.Disable();
        }
    }
}
