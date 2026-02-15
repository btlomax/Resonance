using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
   public void OnStartClicked()
   {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Assuming the main game scene is at index 1 in the build settings
    }
}
