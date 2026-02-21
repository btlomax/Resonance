using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
   public void OnStartClicked()
   {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Assuming the main game scene is at index 1 in the build settings
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
