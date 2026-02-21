using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private GameObject _firstButton;

    private void Awake()
    {
        _inputHandler.Controls.ControllerUI.Enable();
        // Ensure the first button is selected when the main menu is active
        if (_firstButton != null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_firstButton);
        }
    }

    public void OnStartClicked()
   {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Assuming the main game scene is at index 1 in the build settings
        _inputHandler.Controls.ControllerUI.Disable(); // Disable UI controls when starting the game
    }
   
   public void OnOptionsClicked()
   {
        // Load the options menu scene
        Debug.Log("Options button clicked - open options menu");
   }

   public void OnExitClicked()
   {
        // Exit the application
        Debug.Log("Exit button clicked - quitting application");
        Application.Quit();
    }
}
