using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class WorldUIManager : MonoBehaviour
{
   public static WorldUIManager Instance { get; private set; }

    [SerializeField]
    private Canvas worldCanvas;
    [SerializeField]
    private CinemachineCamera mainCamera;
    [SerializeField]
    private GameObject worldUIPrefab;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Create a world space UI element that follows a target transform and displays prompt text on the object.
    /// Displays text passed in promptText parameter.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="promptText"></param>
    /// <returns></returns>
    public GameObject CreateWorldUI(Transform target, string promptText)
    {
        GameObject worldUIInstance = Instantiate(worldUIPrefab, worldCanvas.transform);
        WorldUIFollow followComponent = worldUIInstance.GetComponent<WorldUIFollow>();

        if (followComponent != null)
        {
            followComponent.target = target;
            followComponent.mainCamera = mainCamera;
            followComponent.promptText.text = promptText;
        }

        return worldUIInstance;
    }

    public void HidePrompt(GameObject ui)
    {
        Destroy(ui); // later replace with pooling
    }
}
