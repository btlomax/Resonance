using Unity.Cinemachine;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private CinemachineInputAxisController _cameraInput;

    public static bool IsPaused { get;  private set; } = false;

    private void Update()
    {
        if(_inputHandler.PauseMenuInput)
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1f;
        _cameraInput.enabled = true;
        IsPaused = false;
    }
}
