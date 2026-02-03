using Unity.Cinemachine;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputHandler _inputHandler;

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

    private void OnTogglePauseMenu()
    {
        // Implement pause menu toggle logic here
        Debug.Log("Pause menu toggled.");

        pauseMenuUIBase.SetActive(!pauseMenuUIBase.activeSelf);
    }
}
